using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;

namespace BackendTemplateAPI.Services.Data;

public partial class DataService: DbContext, IDataService
{
    public DataService(DbContextOptions<DataService> options, IHostEnvironment env, IAuditService auditService) :
        base(options)
    {
        this.env = env;
        this.auditService = auditService;
    }

    private readonly IAuditService auditService;
    private readonly IHostEnvironment env;
    private static readonly SemaphoreSlim invoiceSemaphore = new(1, 1);
    private static readonly SemaphoreSlim ncfSemaphore = new(1, 1);
    private static readonly SemaphoreSlim paymentSemaphore = new(1, 1);

    public async Task<T?> GetByIdAsync<T>(params object[] keys) where T : class =>
        await Set<T>().FindAsync(keys);

    public async Task<T?> GetAsync<T>(Expression<Func<T, bool>>? expression = null,
        params Expression<Func<T, object>>[] includes) where T : class
    {
        var query = Set<T>().Where(expression ?? (_ => true)).AsQueryable();
        var response = includes.Any()
            ? includes.Aggregate(query, (current, include) => current.Include(include)).FirstOrDefaultAsync()
            : query.FirstOrDefaultAsync();
        return await response;
    }

    public async Task<List<T>> GetAll<T>(Expression<Func<T, bool>>? expression = null,
        params Expression<Func<T, object>>[] includes) where T : class
    {
        var query = Set<T>().Where(expression ?? (_ => true)).AsQueryable();
        query = includes.Any()
            ? includes.Aggregate(query, (current, include) => current.Include(include))
            : query;

        var response =
            await ((query is IQueryable<AuditableEntity> entities
                ? entities.OrderByDescending(x => x.CreatedOn) as IQueryable<T>
                : query) ?? query).ToListAsync();
        return response;
    }

    public async Task<int> Count<T>(Expression<Func<T, bool>>? expression = null) where T : class =>
        expression is not null ? await Set<T>().Where(expression).CountAsync() : await Set<T>().CountAsync();

    public Task<T?> GetLast<T>(Expression<Func<T, bool>>? expression = null) where T : AuditableEntity =>
        Set<T>().Where(expression ?? (_ => true)).OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();

    public new async Task<T> Add<T>(T data) where T : class
    {
        List<AuditEntry> auditEntries = new();
        if (data is AuditableEntity auditableEntity)
        {
            auditableEntity.CreatedOn = DateTime.Now;
            auditableEntity.CreatedBy = auditService.GetCurrentUser()?.Id ?? auditableEntity.CreatedBy;
        }

        var entity = (await Set<T>().AddAsync(data)).Entity;
        await SaveChangesAsync();
        return entity;
    }

    public new async Task Update<T>(T data, Guid? userId = null) where T : class
    {
        List<AuditEntry> auditEntries = new();
        if (data is AuditableEntity auditableEntity)
        {
            auditableEntity.LastModifiedOn = DateTime.Now;
            auditableEntity.LastModifiedBy = userId ?? auditService.GetCurrentUser()?.Id;
        }

        Set<T>().Update(data);
        await SaveChangesAsync();
    }

    public async Task Delete<T>(T data) where T : class
    {
        Set<T>().Remove(data);
        await SaveChangesAsync();
    }

    public async Task AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        List<AuditEntry> auditEntries = new();
        var enumerable = entities as TEntity[] ?? entities.ToArray();
        foreach (var entity in enumerable)
        {
            if (entity is AuditableEntity auditableEntity)
            {
                auditableEntity.CreatedOn = DateTime.Now;
                auditableEntity.CreatedBy = auditService.GetCurrentUser()?.Id ?? auditableEntity.CreatedBy;
            }
        }

        await Set<TEntity>().AddRangeAsync(enumerable);
        await SaveChangesAsync();
    }

    public async Task DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        Set<TEntity>().RemoveRange(entities);
        await SaveChangesAsync();
    }

    public async Task Atomic(Func<Task> operation)
    {
        if (Database.IsInMemory())
        {
            await operation();
            return;
        }

        using var transaction = await Database.BeginTransactionAsync();
        try
        {
            await operation();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Exception in atomic data operation.", ex);
        }
    }

    protected override void OnModelCreating(ModelBuilder m)
    {
        var entities = m.Model.GetEntityTypes().Select(et => et.ClrType)
            .Where(t => !(t.Namespace?.StartsWith("System") ?? false));
        foreach (var (type, builder) in entities.Select(t => (t, m.Entity(t))).ToArray())
        {
            if (type.GetProperties().Any(p => p.Name == "Id"))
                builder.HasKey("Id");
            else if (type.GetProperties().Any(p => p.Name.EndsWith("Id")))
                builder.HasKey(type.GetProperties()
                    .Where(p => p.Name.EndsWith("Id"))
                    .Select(p => p.Name)
                    .ToArray());
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                    builder.Property(property.Name).HasColumnType("decimal(18,6)");
            }

            foreach (var readonly_property in type.GetProperties().Where(p => !p.CanWrite))
                builder.Ignore(readonly_property.Name);
            foreach (var foreign_key in builder.Metadata.GetForeignKeys())
                foreign_key.DeleteBehavior = DeleteBehavior.NoAction;
        }

        foreach (var (type, data) in Seeder.Seeds(pass => new PasswordHasher<User>().HashPassword(null, pass)))
            m.Entity(type, e => e.HasData(data));

        if (env.IsDevelopment())
        {
            foreach (var (type, data) in Seeder.DevelopmentSeeds())
                m.Entity(type, e => e.HasData(data));
        }

        // ##################################################
        m.Entity<Subscription>(s =>
        {
            s.HasOne(s => s.ShippingAddress)
                .WithOne(a => a.Subscription)
                .OnDelete(DeleteBehavior.NoAction);
            s.HasOne(a => a.BillingAddress)
                .WithMany(a => a.BillingSubscriptions)
                .OnDelete(DeleteBehavior.NoAction);

            s.Property(s => s.Balance)
                .HasComputedColumnSql("dbo.GetSubscriptionBalance([Id])");
            s.ToTable(tbl => tbl.HasTrigger("dbo.GetSubscriptionBalance([Id])"));
        });
        m.Entity<Invoice>(s =>
        {
            s.HasMany(i => i.Lines)
                .WithOne(l => l.Invoice)
                .OnDelete(DeleteBehavior.Cascade);

            s.HasIndex(e => e.StatusId).IsUnique(false).HasDatabaseName(null);
            s.Property(i => i.DocumentAmount)
                .HasComputedColumnSql("dbo.GetInvoiceTotalAmount([Id])");
            s.Property(i => i.PaidAmount)
                .HasComputedColumnSql("dbo.GetInvoicePaidAmount([Id])");
            s.Property(i => i.StatusId)
                .HasComputedColumnSql("dbo.GetInvoiceStatus([Id])");
            s.ToTable(tbl => tbl.HasTrigger("dbo.GetInvoiceTotalAmount([Id])"));
            s.ToTable(tbl => tbl.HasTrigger("dbo.GetInvoicePaidAmount([Id])"));
            s.ToTable(tbl => tbl.HasTrigger("dbo.GetInvoiceStatus([Id])"));
        });
        m.Entity<CreditMemo>()
            .Property(p => p.AppliedAmount)
            .HasComputedColumnSql("dbo.GetCreditMemoAppliedAmount([Id])");
        m.Entity<CreditMemo>()
            .ToTable(tbl => tbl.HasTrigger("dbo.GetCreditMemoAppliedAmount([Id])"));
        m.Entity<DebitMemo>()
            .Property(p => p.AppliedAmount)
            .HasComputedColumnSql("dbo.GetDebitMemoAppliedAmount([Id])");
        m.Entity<DebitMemo>()
            .ToTable(tbl => tbl.HasTrigger("dbo.GetDebitMemoAppliedAmount([Id])"));
        m.Entity<Payment>()
            .Property(p => p.AppliedAmount)
            .HasComputedColumnSql("dbo.GetPaymentAppliedAmount([Id])");
        m.Entity<Payment>()
            .ToTable(tbl => tbl.HasTrigger("dbo.GetPaymentAppliedAmount([Id])"));

        m.Entity<Branch>()
            .HasKey(b => b.Id);
        m.Entity<Branch>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();
    }

    public DbSet<Anomaly> Anomalies { get; set; }
    public DbSet<AnomalyResolutionType> AnomalyResolutionTypes { get; set; }
    public DbSet<AnomalyStatus> AnomalyStatuses { get; set; }
    public DbSet<AnomalyType> AnomalyTypes { get; set; }
    public DbSet<AreaType> AreaTypes { get; set; }
    public DbSet<Audit> Audits { get; set; }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<BillingBatch> BillingBatches { get; set; }
    public DbSet<BillingBatchLine> BillingBatchLines { get; set; }
    public DbSet<BillingCyclePeriod> BillingCyclePeriods { get; set; }
    public DbSet<BillingCycle> BillingCycles { get; set; }
    public DbSet<BillingPeriod> BillingPeriods { get; set; }
    public DbSet<BillingSchedule> BillingSchedules { get; set; }
    public DbSet<BillingType> BillingTypes { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<BranchType> BranchTypes { get; set; }
    public DbSet<BrigadeGeolocation> BrigadeGeolocations { get; set; }
    public DbSet<Brigade> Brigades { get; set; }
    public DbSet<BrigadeMember> BrigadeMembers { get; set; }
    public DbSet<BrigadeStatus> BrigadeStatuses { get; set; }
    public DbSet<BrigadeType> BrigadeTypes { get; set; }
    public DbSet<Charge> Charges { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<ClaimInvoice> ClaimInvoices { get; set; }
    public DbSet<ClaimMotive> ClaimMotives { get; set; }
    public DbSet<ClaimResult> ClaimResults { get; set; }
    public DbSet<Claim> Claims { get; set; }
    public DbSet<ClaimType> ClaimTypes { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanySetting> CompanySettings { get; set; }
    public DbSet<ConnectionType> ConnectionTypes { get; set; }
    public DbSet<ContactType> ContactTypes { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<CreditMemoApply> CreditMemoApplies { get; set; }
    public DbSet<CreditMemo> CreditMemos { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<CurrencyRate> CurrencyRates { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerNote> CustomerNotes { get; set; }
    public DbSet<CustomerTicket> CustomerTickets { get; set; }
    public DbSet<CustomerTicketType> CustomerTicketTypes { get; set; }
    public DbSet<CustomerType> CustomerTypes { get; set; }
    public DbSet<DebitMemoApply> DebitMemoApplies { get; set; }
    public DbSet<DebitMemo> DebitMemos { get; set; }
    public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    public DbSet<ElectricalEquipment> ElectricalEquipments { get; set; }
    public DbSet<ERPCodeDescription> ERPCodeDescriptions { get; set; }
    public DbSet<ExtensionProperty> ExtensionProperties { set; get; }
    public DbSet<Extension> Extensions { get; set; }
    public DbSet<Frequency> Frequencies { get; set; }
    public DbSet<Import> Imports { get; set; }
    public DbSet<InvoiceLine> InvoiceLines { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceStatus> InvoiceStatuses { get; set; }
    public DbSet<Issue> Issues { get; set; }
    public DbSet<IssueType> IssueTypes { get; set; }
    public DbSet<Itinerary> Itineraries { get; set; }
    public DbSet<ItineraryRoutePoint> ItineraryRoutePoints { get; set; }
    public DbSet<LegalInstance> LegalInstances { get; set; }
    public DbSet<LotBalance> LotBalances { get; set; }
    public DbSet<Lot> Lots { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<MaterialConsumption> MaterialConsumptions { get; set; }
    public DbSet<MaterialExistence> MaterialExistences { get; set; }
    public DbSet<MeasuringType> MeasuringTypes { get; set; }
    public DbSet<MeterExistence> MeterExistences { get; set; }
    public DbSet<MeterModelReadingType> MeterModelReadingTypes { get; set; }
    public DbSet<MeterModel> MeterModels { get; set; }
    public DbSet<Meter> Meters { get; set; }
    public DbSet<NCFSequenceSetting> NCFSequenceSettings { get; set; }
    public DbSet<NCFType> NCFTypes { get; set; }
    public DbSet<NonDebtCertificate> NonDebtCertificates { get; set; }
    public DbSet<OpeningAmount> OpeningAmounts { get; set; }
    public DbSet<Opening> Openings { get; set; }
    public DbSet<OrderTypeOrderTask> OrderTypeOrderTasks { get; set; }
    public DbSet<PaymentAgreementInstallment> PaymentAgreementInstallments { get; set; }
    public DbSet<PaymentAgreement> PaymentAgreements { get; set; }
    public DbSet<PaymentApply> PaymentApplies { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<PaymentMethodType> PaymentMethodTypes { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentStatus> PaymentStatuses { get; set; }
    public DbSet<PaymentTerm> PaymentTerms { get; set; }
    public DbSet<PermissionArea> PermissionAreas { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<PermissionType> PermissionTypes { get; set; }
    public DbSet<PointReadout> PointReadouts { get; set; }
    public DbSet<PriceType> PriceTypes { get; set; }
    public DbSet<ProductRateExtraCharge> ProductRateExtraCharges { get; set; }
    public DbSet<ProductRate> ProductRates { get; set; }
    public DbSet<ProductRateScale> ProductRateScales { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<PropertyType> PropertyTypes { get; set; }
    public DbSet<ReadingType> ReadingTypes { get; set; }
    public DbSet<ReceivableReason> ReceivableReasons { get; set; }
    public DbSet<RelatedPerson> RelatedPersons { get; set; }
    public DbSet<Relationship> Relationships { get; set; }
    public DbSet<RolePermission> RoleClaims { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Core.Models.Route> Routes { get; set; }
    public DbSet<RouteType> RouteTypes { get; set; }
    public DbSet<Seals> Seals { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<StorageFile> StorageFiles { get; set; }
    public DbSet<SubscriptionAnomalies> SubscriptionAnomalies { get; set; }
    public DbSet<SubscriptionAttachment> SubscriptionAttachments { get; set; }
    public DbSet<SubscriptionEquipment> SubscriptionEquipments { get; set; }
    public DbSet<SubscriptionLine> SubscriptionLines { get; set; }
    public DbSet<SubscriptionNote> SubscriptionNotes { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
    public DbSet<TaxSchedule> TaxSchedules { get; set; }
    public DbSet<UserPermission> UserClaims { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Voltage> Voltages { get; set; }
    public DbSet<WorkOrderHistory> WorkOrderHistories { get; set; }
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<WorkOrderStatus> WorkOrderStatuses { get; set; }
    public DbSet<WorkOrderTask> WorkOrderTasks { get; set; }
    public DbSet<WorkOrderTaskResult> WorkOrderTaskResults { get; set; }
    public DbSet<WorkOrderType> WorkOrderTypes { get; set; }
}