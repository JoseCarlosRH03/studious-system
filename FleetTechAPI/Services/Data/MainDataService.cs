using System.Linq.Expressions;
using FleetTechCore;
using FleetTechCore.Models;
using FleetTechCore.Models.Address;
using FleetTechCore.Models.Company;
using FleetTechCore.Models.Extensions;
using FleetTechCore.Models.Fleet;
using FleetTechCore.Models.Fuel;
using FleetTechCore.Models.Inventory;
using FleetTechCore.Models.User;
using FleetTechCore.Models.WorkShop;
using FleetTechCore.Services;
using FleetTechCore.Services.Model_Related_Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FleetTechAPI.Services.Data;

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
        //if (Database.IsInMemory())
        //{
        //    await operation();
        //    return;
        //}

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

    }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<BranchType> BranchTypes { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanySetting> CompanySettings { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<ExtensionProperty> ExtensionProperties { set; get; }
    public DbSet<Extension> Extensions { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<MaterialConsumption> MaterialConsumptions { get; set; }
    public DbSet<MaterialExistence> MaterialExistences { get; set; }
    public DbSet<PermissionArea> PermissionAreas { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<PermissionType> PermissionTypes { get; set; }
    public DbSet<RolePermission> RoleClaims { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<UserPermission> UserClaims { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<MaintenanceScheduling> MaintenanceSchedulings { get; set; }
    public DbSet<Mechanic> Mechanics { get; set; }
    public DbSet<MechanicSpecialty> MechanicSpecialtys { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<FuelPrice> FuelPrices { get; set; }
    public DbSet<FuelStation> FuelStations { get; set; }
    public DbSet<LicenseDrivers> Licenses { get; set; }
}