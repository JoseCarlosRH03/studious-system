namespace BackendTemplateAPI.Services.Data;

public partial class DataService
{
    public Task<Country?> GetCurrentCountry() =>
        Countries.FirstOrDefaultAsync(c =>
            Companies.FirstOrDefault() != null && c.Id == Companies.FirstOrDefault()!.Id);

    public Task<List<CustomerAddressListView>> GetCustomerAddressListViews(int start, int count, AddressSearchData data) =>
        CustomerAddresses
            .Include(a => a.City)
            .ThenInclude(c => c.State)
            .ThenInclude(s => s.Country)
            .Include(a => a.Customers)
            .ThenInclude(c => c.Customer)
            .Include(a => a.PropertyType)
            .Include(a => a.Subscription)
            .Include(a => a.Meter)
            .Where(x => data.CustomerId == null || x.Customers.Any(c => c.Customer.Id == data.CustomerId))
            .Where(x => string.IsNullOrWhiteSpace(data.SubscriptionNumber) ||
                        (x.Subscription != null && x.Subscription.SubscriptionNumber == data.SubscriptionNumber))
            .Where(x => string.IsNullOrWhiteSpace(data.Filter) ||
                        (x.Name.ToLower().Contains(data.Filter) || x.AddressLine1.ToLower().Contains(data.Filter) ||
                         x.City.Name.ToLower().Contains(data.Filter) || x.Id == Int32.Parse(data.Filter)))
            .OrderByDescending(c => c.Status)
            .ThenByDescending(c => c.CreatedOn)
            .Skip(start)
            .Take(count)
            .Select(x => CustomerAddressListView.From(x))
            .ToListAsync();

    public Task<CustomerAddressDetailedView> GetCustomerAddressView(int id) =>
        CustomerAddresses
            .Include(a => a.City)
            .ThenInclude(c => c.State)
            .ThenInclude(s => s.Country)
            .Include(a => a.Customers)
            .ThenInclude(c => c.Customer)
            .Include(a => a.PropertyType)
            .Include(a => a.Subscription)
            .Include(a => a.Meter)
            .ThenInclude(m => m.MeterModel)
            .Where(c => c.Id == id)
            .Select(c => CustomerAddressDetailedView.From(c))
            .FirstOrDefaultAsync();

    public Task<int> GetCustomerAddressCount(AddressSearchData data) =>
        CustomerAddresses
            .Where(x => data.CustomerId == null || x.Customers.Any(c => c.Customer.Id == data.CustomerId))
            .Where(x => string.IsNullOrWhiteSpace(data.SubscriptionNumber) ||
                        (x.Subscription != null && x.Subscription.SubscriptionNumber == data.SubscriptionNumber))
            .Where(x => string.IsNullOrWhiteSpace(data.Filter) ||
                        (x.Name.ToLower().Contains(data.Filter) || x.AddressLine1.ToLower().Contains(data.Filter) ||
                         x.City.Name.ToLower().Contains(data.Filter)))
            .CountAsync();

    public Task<List<InvoiceListView>> GetInvoiceViews(int start, int count, InvoiceSearchData data) =>
        Invoices
            .Include(i => i.Customer)
            .Include(i => i.Subscription)
            .Include(i => i.Applies)
            .ThenInclude(a => a.Payment)
            .Where(x => data.CustomerId == null || x.CustomerId == data.CustomerId)
            .Where(x => string.IsNullOrWhiteSpace(data.CustomerDocument) ||
                        x.Customer.DocumentNumber.Contains(data.CustomerDocument.ToLower().Trim()))
            .Where(x => string.IsNullOrWhiteSpace(data.SubscriptionNumber) || (x.Subscription != null && x.Subscription.SubscriptionNumber == data.SubscriptionNumber.ToLower().Trim()))
            .Where(x => string.IsNullOrWhiteSpace(data.Filter) ||
                        x.DocumentNumber.ToLower().Contains(data.Filter.ToLower().Trim()))
            .Where(x => data.Status == null || data.Status.Contains(x.StatusId))
            .Where(x => data.ProductId == null || x.Lines.Any(l => l.ProductId == data.ProductId))
            .OrderByDescending(c => c.DocumentDate)
            .Skip(start)
            .Take(count)
            .Select(x => InvoiceListView.From(x)).ToListAsync();

    public Task<int> GetInvoiceCount(InvoiceSearchData data) =>
        Invoices
            .Where(x => data.CustomerId == null || x.CustomerId == data.CustomerId)
            .Where(x => string.IsNullOrWhiteSpace(data.CustomerDocument) ||
                        x.Customer.DocumentNumber.Contains(data.CustomerDocument.ToLower().Trim()))
            .Where(x => string.IsNullOrWhiteSpace(data.SubscriptionNumber) || (x.Subscription != null &&
                                                                               x.Subscription.SubscriptionNumber
                                                                                   .Contains(
                                                                                       data.SubscriptionNumber.ToLower()
                                                                                           .Trim())))
            .Where(x => string.IsNullOrWhiteSpace(data.Filter) ||
                        x.DocumentNumber.ToLower().Contains(data.Filter.ToLower().Trim()))
            .Where(x => data.Status == null || data.Status.Contains(x.StatusId))
            .Where(x => data.ProductId == null || x.Lines.Any(l => l.ProductId == data.ProductId))
            .CountAsync();

    public Task<PaymentDetailedView> GetPaymentView(int Id) =>
        Payments
            .Where(x => x.Id == Id)
            .Select(p => new PaymentDetailedView(
                p.Id,
                p.DocumentNumber,
                p.DocumentDate,
                new PaymentCustomerView(p.Customer.Id, p.Customer.Name, p.Customer.DocumentNumber,
                    p.SubscriptionId == null
                        ? null
                        : new PaymentSubscriptionView(p.SubscriptionId.Value, p.Subscription.SubscriptionNumber)),
                p.DocumentAmount,
                p.DocumentAmount - p.AppliedAmount,
                new CurrencyListView(p.Currency.Id, p.Currency.Name, p.Currency.Symbol,
                    new Item(p.Currency.Status, ((GenericStatus) p.Currency.Status).ToString()),
                    p.Currency.Rates.FirstOrDefault() == null ? null : p.Currency.Rates.First().ExchangeRate,
                    p.Currency.Code),
                new Item(p.PaymentMethodId, p.PaymentMethod.Name),
                new PaymentDataView(p.CardNumber, p.BatchNumber, p.ApprovalNumber,
                    p.CashAmount, p.CashBack, p.BankId == null ? null : new Item(p.BankId.Value, p.Bank.Name), p.Reference),
                p.VoidReason,
                p.VoidedOn,
                p.VoidedById != null? p.VoidedBy.FirstName + " " + p.VoidedBy.LastName : null,
                p.Applies.Select(a => new PaymentInvoiceView(a.Invoice.Id, a.Invoice.DocumentNumber,
                    a.Invoice.DocumentDate, a.Invoice.DocumentAmount, a.Invoice.PaidAmount, a.ApplyAmount)).ToArray(),
                p.PointOfSale
            ))
            .FirstOrDefaultAsync();

    public Task<List<PaymentListView>> GetPaymentViews(int start, int count, PaymentSearchData data) =>
        Payments
            .Include(x => x.Customer)
            .Include(x => x.Applies)
            .ThenInclude(x => x.Invoice)
            .Include(x => x.PaymentMethod)
            .Include(x => x.Subscription)
            .Include(x => x.Currency)
            .ThenInclude(c => c.Rates)
            .Where(x => string.IsNullOrWhiteSpace(data.Filter) ||
                        x.DocumentNumber.Contains(data.Filter.ToLower().Trim()))
            .Where(x => data.CustomerId == null || x.CustomerId == data.CustomerId)
            .Where(x => string.IsNullOrWhiteSpace(data.CustomerDocument) ||
                        x.Customer.DocumentNumber == data.CustomerDocument)
            .Where(x => string.IsNullOrWhiteSpace(data.Subscription) || (x.Subscription != null &&
                                                                         x.Subscription.SubscriptionNumber.Contains(
                                                                             data.Subscription.ToLower().Trim())))
            .Where(x => string.IsNullOrWhiteSpace(data.CustomerName) ||
                        x.Customer.Name.ToLower().Contains(data.CustomerName.ToLower().Trim()))
            .Where(x => data.Status == null || x.StatusId == data.Status)
            .OrderByDescending(c => c.DocumentDate)
            .Skip(start)
            .Take(count)
            .Select(p => PaymentListView.From(p))
            .ToListAsync();

    public Task<int> GetPaymentCount(PaymentSearchData data) =>
        Payments
            .Where(x => string.IsNullOrWhiteSpace(data.Filter) ||
                        x.DocumentNumber.Contains(data.Filter.ToLower().Trim()))
            .Where(x => data.CustomerId == null || x.CustomerId == data.CustomerId)
            .Where(x => string.IsNullOrWhiteSpace(data.CustomerDocument) ||
                        x.Customer.DocumentNumber == data.CustomerDocument)
            .Where(x => string.IsNullOrWhiteSpace(data.Subscription) || (x.Subscription != null &&
                                                                         x.Subscription.SubscriptionNumber.Contains(
                                                                             data.Subscription.ToLower().Trim())))
            .Where(x => string.IsNullOrWhiteSpace(data.CustomerName) ||
                        x.Customer.Name.ToLower().Contains(data.CustomerName.ToLower().Trim()))
            .Where(x => data.Status == null || x.StatusId == data.Status)
            .CountAsync();

    public Task<List<Invoice>> GetInvoiceListForDate(DateTime data) =>
        Invoices
            .Where(i => i.DocumentDate.Date == data.Date)
            .Include(i => i.Lines)
            .ThenInclude(l => l.Product)
            .ThenInclude(pr => pr.TaxSchedule)
            .ToListAsync();

    public Task<List<PaymentApply>> GetPaymentListForDate(DateTime date) =>
        PaymentApplies
            .Where(p => p.ApplyDate.Date == date.Date)
            .Include(p => p.Payment)
            .ThenInclude(pay => pay.PaymentMethod)
            .Include(p => p.Invoice)
            .ThenInclude(i => i.Lines)
            .ThenInclude(l => l.Product)
            .ThenInclude(pr => pr.TaxSchedule)
            .ToListAsync();

    public Task<List<Payment>> GetAdvancedPaymentListForDate(DateTime date) =>
        Payments
            .Where(p => p.DocumentDate.Date == date.Date)
            .Where(p => p.TypeId == (int)Core.Enums.PaymentTypes.Advanced)
            .Include(p => p.PaymentMethod)
            .Include(p => p.Subscription)
            .ThenInclude(s => s.Lines)
            .ThenInclude(l => l.Product)
            .ThenInclude(pr => pr.TaxSchedule)
            .ToListAsync();
    public Task<bool> GetAddressIdsWithData(int cityId, string addressLine1, out int[]? ids)
    {
        ids = CustomerAddresses
            .Where(ca => ca.CityId == cityId && ca.AddressLine1.ToLower() == addressLine1.ToLower().Trim())
            .Select(ca => ca.Id).ToArray();
        return ids.Any() ? Task.FromResult(true) : Task.FromResult(false);
    }

    public async Task<InvoicePaymentSummaryView> GetInvoiceSummariesForPayments(InvoiceSearchData data)
    {
        var summaryList = Invoices
            .Where(x => data.CustomerId == null || x.CustomerId == data.CustomerId)
            .Where(x => string.IsNullOrWhiteSpace(data.CustomerDocument) || x.DocumentNumber == data.CustomerDocument)
            .Where(x => string.IsNullOrWhiteSpace(data.SubscriptionNumber) || (x.Subscription != null &&
                                                                               x.Subscription.SubscriptionNumber ==
                                                                               data.SubscriptionNumber))
            .Where(x => string.IsNullOrWhiteSpace(data.Filter) ||
                        x.Customer.Name.ToLower().Contains(data.Filter) || x.DocumentNumber.Contains(data.Filter))
            .Where(x => x.StatusId != (int) Core.Enums.InvoiceStatuses.Pagado &&
                        x.StatusId != (int) Core.Enums.InvoiceStatuses.IncluidaEnAcuerdo &&
                        x.StatusId != (int) Core.Enums.InvoiceStatuses.Anulado);

        return new InvoicePaymentSummaryView(
            new InvoicePaymentCustomerView(
                await summaryList.Select(x => x.CustomerId).Distinct().CountAsync() > 1
                    ? throw new InvalidParameter(
                        "Se encontro mas de un cliente, intente ingresar diferentes criterios de busqueda")
                    : await summaryList.Select(x => x.CustomerId).Distinct().CountAsync() == 0
                        ? throw new InvalidParameter(
                            "No se encontro un cliente con facturas pendientes (Estados facturado o pagado parcialmente), intente ingresar diferentes criterios de busqueda")
                        : await summaryList.Select(x => x.CustomerId).FirstAsync(),
                await summaryList.Select(x => x.Customer.Name).FirstAsync(),
                await summaryList.Select(x => x.Customer.DocumentNumber).FirstAsync(),
                await summaryList.Where(x =>
                        string.IsNullOrWhiteSpace(data.SubscriptionNumber) || (x.Subscription != null &&
                                                                               x.Subscription.SubscriptionNumber ==
                                                                               data.SubscriptionNumber))
                    ?.Select(x => x.SubscriptionId == null ? null : x.Subscription.ShippingAddress.AddressLine1)
                    .FirstAsync(),
                await summaryList.Where(x =>
                        string.IsNullOrWhiteSpace(data.SubscriptionNumber) || (x.Subscription != null &&
                                                                               x.Subscription.SubscriptionNumber ==
                                                                               data.SubscriptionNumber))
                    ?.Select(x => x.SubscriptionId == null ? null : x.Subscription.SubscriptionNumber).FirstAsync()),
            await summaryList
                .Include(i => i.Customer)
                .Include(i => i.Subscription)
                .Include(i => i.Applies)
                .ThenInclude(a => a.Payment)
                .Select(x => InvoiceListView.From(x)).ToListAsync()
        );
    }

    public Task<List<BillingBatchListView>> GetBillingBatches(int start, int count, string? filter) =>
        BillingBatches
            .Include(x => x.Lines)
            .Where(b => string.IsNullOrWhiteSpace(filter) || b.Number.ToLower().Contains(filter.ToLower().Trim()))
            .OrderByDescending(c => c.CreatedOn)
            .Skip(start)
            .Take(count)
            .Select(x => new BillingBatchListView
            {
                Id = x.Id,
                Number = x.Number,
                Date = x.Date,
                DateProcessed = x.InvoicedOn,
                Count = x.Lines.Count,
                Pending = x.Lines.Count(l => l.Status == (int) BillingBatchLineStatuses.Pending),
                Success = x.Lines.Count(l => l.Status == (int) BillingBatchLineStatuses.Success),
                Error = x.Lines.Count(l => l.Status == (int) BillingBatchLineStatuses.Failed),
                Status = new Item(x.Status, ((BillingBatchStatuses) x.Status).ToString()),
            })
            .ToListAsync();

    public Task<List<BillingScheduleListView>> GetBillingSchedules(int start, int count, string? filter) =>
        BillingSchedules
            .Include(bs => bs.Periods)
            .Include(bs => bs.Frequency)
            .Where(bs => string.IsNullOrWhiteSpace(filter) || bs.Name.ToLower().Contains(filter.ToLower().Trim()))
            .OrderByDescending(c => c.Status)
            .ThenByDescending(c => c.CreatedOn)
            .Skip(start)
            .Take(count)
            .Select(bs => BillingScheduleListView.From(bs))
            .ToListAsync();

    public Task<List<BillingCycleListView>> GetBillingCycles(int start, int count, string? filter, int? status) =>
        BillingCycles
            .Include(bc => bc.BillingCyclePeriods)
            .Include(bc => bc.BillingSchedule)
            .ThenInclude(bs => bs.Frequency)
            .Include(bc => bc.BillingSchedule)
            .ThenInclude(bs => bs.Periods)
            .Where(bc => string.IsNullOrWhiteSpace(filter) || bc.Name.ToLower().Contains(filter.ToLower().Trim()) ||
                         bc.Description.ToLower().Contains(filter.ToLower().Trim()))
            .Where(bc => status == null || bc.Status == status)
            .OrderByDescending(c => c.Status)
            .ThenByDescending(c => c.CreatedOn)
            .Skip(start)
            .Take(count)
            .Select(bc => BillingCycleListView.From(bc))
            .ToListAsync();

    public Task<List<CustomerListView>> GetCustomerViews(int start, int count, string? filter, int? status) =>
        Customers
            .Include(c => c.DocumentType)
            .Include(c => c.Invoices)
            .Include(c => c.Subscriptions)
            .Include(c => c.CustomerType)
            .Where(c => string.IsNullOrWhiteSpace(filter) || c.Name.ToLower().Contains(filter.ToLower().Trim()) ||
                        c.DocumentNumber.ToLower().Contains(filter.ToLower().Trim()))
            .Where(c => status == null || c.Status == status)
            .OrderByDescending(c => c.Status)
            .ThenByDescending(c => c.CreatedOn)
            .Skip(start)
            .Take(count)
            .Select(c => CustomerListView.From(c))
            .ToListAsync();

    public Task<List<TicketListView>> GetCustomerTickets(int start, int count, string? filter, int? status) =>
        CustomerTickets
            .Include(ct => ct.CustomerTicketType)
            .Where(ct => string.IsNullOrWhiteSpace(filter) || ct.FullName.ToLower().Contains(filter.ToLower().Trim()) ||
                         ct.Subject.ToLower().Contains(filter.ToLower().Trim()))
            .Where(ct => status == null || ct.Status == status)
            .OrderBy(c => c.Status)
            .ThenByDescending(c => c.CreatedOn)
            .Skip(start)
            .Take(count)
            .Select(ct => TicketListView.From(ct))
            .ToListAsync();

    public Task<List<MeterView>> GetMeters(int start, int count, string? filter) =>
        Meters
            .Include(m => m.MeterModel)
            .ThenInclude(mm => mm.ReadingTypes)
            .ThenInclude(rt => rt.ReadingType)
            .Include(mm => mm.CustomerAddress)
            .Where(m => string.IsNullOrWhiteSpace(filter) ||
                        m.MeterNumber.ToLower().Contains(filter.ToLower().Trim()) ||
                        m.CustomerAddress.City.Name.ToLower().Contains(filter.ToLower().Trim()) ||
                        m.CustomerAddress.Id.ToString() == filter)
            .Skip(start)
            .Take(count)
            .Select(x => MeterView.From(x))
            .ToListAsync();

    public Task<MeterView> GetMeterView(int id) =>
        Meters
            .Include(m => m.MeterModel)
            .ThenInclude(mm => mm.ReadingTypes)
            .ThenInclude(rt => rt.ReadingType)
            .Include(mm => mm.CustomerAddress)
            .ThenInclude(ca => ca.City)
            .Where(m => m.Id == id)
            .Select(x => MeterView.From(x))
            .FirstOrDefaultAsync();


    public Task<List<MeterModelView>> GetMeterModels(string filter) =>
        MeterModels
            .Include(mm => mm.ReadingTypes)
            .ThenInclude(rt => rt.ReadingType)
            .Include(mm => mm.Voltage)
            .Where(mm => string.IsNullOrWhiteSpace(filter) || mm.Brand.ToLower().Contains(filter.ToLower().Trim()) ||
                         mm.Model.ToLower().Contains(filter.ToLower().Trim()) ||
                         mm.ReadingTypes.Any(rt => rt.ReadingType.Name.ToLower().Contains(filter.ToLower().Trim())))
            .Select(mm => MeterModelView.From(mm))
            .ToListAsync();

    public Task<User> GetUserWithRoles(Guid userId) =>
        Users
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Id == userId);
    
    public Task<User?> GetUserLogin(string login) =>
        Users
            .Include(u => u.Roles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .Include(u => u.Claims)
            .ThenInclude(c => c.Permission)
            .Where(u => u.Username == login || u.Email.ToLower() == login.ToLower())
            .FirstOrDefaultAsync();

    public Task<List<RoleView>> GetRoleViews(int start, int count, string? filter) =>
        Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .Where(r => string.IsNullOrWhiteSpace(filter) || r.Name.ToLower().Contains(filter.ToLower().Trim()))
            .Skip(start)
            .Take(count)
            .Select(r => RoleView.From(r))
            .ToListAsync();

    public Task<List<User>> GetUserViews(string? filter, int start, int count) =>
        Users
            .Include(r => r.Roles)
            .ThenInclude(ur => ur.Role)
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .Include(r => r.Branch)
            .ThenInclude(br => br.BranchType)
            .Include(r => r.Branch)
            .ThenInclude(br => br.City)
            .Include(r => r.Claims)
            .ThenInclude(c => c.Permission)
            .Where(u => string.IsNullOrWhiteSpace(filter) || u.Username.ToLower().Contains(filter.ToLower().Trim()) ||
                        (u.FirstName + " " + u.LastName).ToLower().Contains(filter.ToLower().Trim()))
            .ToListAsync();

    public Task<TicketDetailedView> GetCustomerTicketView(int id) =>
        CustomerTickets
            .Include(ct => ct.CustomerTicketType)
            .Include(ct => ct.WorkOrders)
            .ThenInclude(wo => wo.Customer)
            .ThenInclude(c => c.Subscriptions)
            .Include(ct => ct.WorkOrders)
            .ThenInclude(ct => ct.WorkOrderHistory)
            .Include(ct => ct.WorkOrders)
            .ThenInclude(ct => ct.WorkOrderType)
            .Where(ct => ct.Id == id)
            .Select(ct => TicketDetailedView.From(ct))
            .FirstOrDefaultAsync();

    public Task<LotDetailedView> GetDetailedLotView(Guid lotId) =>
        Lots
            .Where(l => l.Id == lotId)
            .Select(l => new LotDetailedView
            {
                Id = l.Id,
                Start = l.DateCreated,
                End = l.DateClosed,
                Total = l.Payments
                    .Where(l => l.StatusId != (int) Core.Enums.PaymentStatuses.Pendiente &&
                                l.StatusId != (int) Core.Enums.PaymentStatuses.Anulado).Sum(l => l.DocumentAmount),
                User = new UserSummaryView(l.User.Id, l.User.FirstName + " " + l.User.LastName, l.User.Email,
                    l.User.Username),
                Status = new Item(l.DateClosed.HasValue ? 0 : 1, l.DateClosed.HasValue ? "Cerrado" : "Abierto"),
                Transactions = l.Payments.Select(p => new PaymentListView
                (
                    p.Id,
                    p.DocumentNumber,
                    p.DocumentDate,
                    new CustomerSummaryView(p.CustomerId, p.Customer.DocumentNumber, p.Customer.Name),
                    p.DocumentAmount,
                    p.SubscriptionId != null? p.Subscription.SubscriptionNumber : "",
                    p.Applies.Select(a => a.Invoice.DocumentNumber).ToList(),
                    p.PaymentMethod.Name,
                    new CurrencyListView(
                        p.CurrencyId,
                        p.Currency.Name,
                        p.Currency.Symbol,
                        new Item(p.Currency.Status, ((GenericStatus) p.Currency.Status).ToString()),
                        !p.Currency.Rates.Any(x =>
                            x.StartDate.Date <= DateTime.Now.Date && x.EndDate.Date >= DateTime.Now.Date)
                            ? null
                            : p.Currency.Rates.FirstOrDefault(x =>
                                    x.StartDate.Date <= DateTime.Now.Date && x.EndDate.Date >= DateTime.Now.Date)
                                .ExchangeRate,
                        p.Currency.Code),
                    Item.From((PaymentStatuses) p.StatusId),
                    p.PointOfSale
                )).ToList(),
                AmountsBoth = l.Payments
                    .Where(p => p.StatusId != (int) Core.Enums.PaymentStatuses.Pendiente &&
                                p.StatusId != (int) Core.Enums.PaymentStatuses.Anulado)
                    .GroupBy(p => new {p.PaymentMethodId, p.CurrencyId})
                    .Select(p => new LotDetailsLotView(
                            p.Key.PaymentMethodId,
                            new Item(p.Key.PaymentMethodId, p.First().PaymentMethod.Name),
                            new Item(p.First().CurrencyId, p.First().Currency.Name),
                            p.Count(),
                            p.Sum(x => x.DocumentAmount)
                        )
                    )
                    .ToList(),
                Amounts = l.Payments
                    .Where(p => p.StatusId != (int) Core.Enums.PaymentStatuses.Pendiente &&
                                p.StatusId != (int) Core.Enums.PaymentStatuses.Anulado)
                    .GroupBy(p => p.PaymentMethodId)
                    .Select(p => new LotDetailsLotView(
                            p.Key,
                            new Item(p.Key, p.First().PaymentMethod.Name),
                            new Item(p.First().CurrencyId, p.First().Currency.Name),
                            p.Count(),
                            p.Sum(x => x.DocumentAmount)
                        )
                    )
                    .ToList(),
            })
            .FirstOrDefaultAsync();

    public Task<List<ClaimListView>> GetClaims(int start, int count, string? filter) =>
        Claims
            .Include(c => c.Customer)
            .Include(c => c.Subscription)
            .Include(c => c.ClaimType)
            .Include(c => c.ClaimMotive)
            .Include(c => c.LegalInstance)
            .Include(c => c.Invoices)
            .Where(c => string.IsNullOrWhiteSpace(filter) ||
                        c.ClaimNumber.ToLower().Contains(filter.ToLower().Trim()) ||
                        c.Customer.Name.ToLower().Contains(filter.ToLower().Trim()))
            .OrderByDescending(c => c.Status)
            .ThenByDescending(c => c.Date)
            .Skip(start)
            .Take(count)
            .Select(c => ClaimListView.From(c))
            .ToListAsync();

    public Task<List<CityView>> GetCities(int start, int count, string? filter) =>
        Cities
            .Include(c => c.State)
            .Where(c => string.IsNullOrWhiteSpace(filter) || c.Name.ToLower().Contains(filter.ToLower().Trim()))
            .OrderByDescending(c => c.Status)
            .ThenByDescending(c => c.CreatedOn)
            .Skip(start)
            .Take(count)
            .Select(c => new CityView(c.Id, c.Name, StateView.From(c.State), Item.From((GenericStatus) c.Status)))
            .ToListAsync();

    public Task<List<StateView>> GetStates(string? filter) =>
        States
            .Include(s => s.Country)
            .Include(s => s.Cities)
            .Where(s => string.IsNullOrWhiteSpace(filter) || s.Name.ToLower().Contains(filter.ToLower().Trim()))
            .OrderByDescending(c => c.Status)
            .ThenByDescending(c => c.CreatedOn)
            .Select(s => new StateView(s.Id, s.Name, s.Country.Name, Item.From((GenericStatus) s.Status)))
            .ToListAsync();

    public Task<List<StatusItemView>> GetCountries(string? filter) =>
        Countries
            .Include(c => c.States)
            .Where(c => string.IsNullOrWhiteSpace(filter) || c.Name.ToLower().Contains(filter.ToLower().Trim()))
            .OrderByDescending(c => c.Status)
            .ThenByDescending(c => c.CreatedOn)
            .Select(c => new StatusItemView(c.Id, c.Name, Item.From((GenericStatus) c.Status)))
            .ToListAsync();

    public Task<List<RouteListView>> GetRoutes(int? statusId, int start, int count, string? filter) =>
        Routes
            .Include(r => r.RouteType)
            .Include(r => r.BillingCycle)
            .Where(r => statusId == null || r.Status == statusId)
            .Where(r => string.IsNullOrWhiteSpace(filter) || r.Name.ToLower().Contains(filter.ToLower().Trim()))
            .OrderByDescending(c => c.Status)
            .ThenByDescending(c => c.CreatedOn)
            .Skip(start)
            .Take(count)
            .Select(r => RouteListView.From(r))
            .ToListAsync();

    public Task<RouteDetailedView> GetRouteView(int id) =>
        Routes
            .Where(r => r.Id == id)
            .Include(r => r.RouteType)
            .Include(r => r.BillingCycle)
            .Include(r => r.Addresses)
            .ThenInclude(r => r.City)
            .Select(r => RouteDetailedView.From(r))
            .FirstOrDefaultAsync();

    public Task<PaymentAgreementView> GetPaymentAgreementView(int Id) =>
        PaymentAgreements
            .Where(pa => pa.Id == Id)
            .Include(pa => pa.Customer)
            .Include(pa => pa.Subscription)
            .Include(pa => pa.Invoices)
            .Include(pa => pa.Installments)
            .Select(pa => PaymentAgreementView.From(pa))
            .FirstOrDefaultAsync();


    public Task<List<PaymentAgreementListView>> GetPaymentAgreements(int start, int count, string? filter) =>
        PaymentAgreements
            .Include(pa => pa.Customer)
            .Include(pa => pa.Subscription)
            .Where(pa =>
                string.IsNullOrWhiteSpace(filter) || pa.Number.Contains(filter) || pa.Customer.Name.Contains(filter))
            .OrderByDescending(c => c.Status)
            .ThenByDescending(c => c.CreatedOn)
            .Skip(start)
            .Take(count)
            .Select(x => PaymentAgreementListView.From(x))
            .ToListAsync();

    public Task<List<NonDebtCertificateListView>> GetNonDebtCertificates(int start, int count, string? filter) =>
        NonDebtCertificates
            .Include(ndc => ndc.Customer)
            .Include(ndc => ndc.Subscription)
            .Where(ndc => string.IsNullOrWhiteSpace(filter) ||
                          ndc.CertificateNumber.ToLower().Contains(filter.ToLower().Trim()) ||
                          ndc.Subscription.SubscriptionNumber.ToLower().Contains(filter.ToLower().Trim()) ||
                          ndc.Customer.Name.ToLower().Contains(filter.ToLower().Trim()))
            .OrderByDescending(c => c.CreatedOn)
            .Skip(start)
            .Take(count)
            .Select(x => NonDebtCertificateListView.From(x))
            .ToListAsync();

    public Task<List<MemoListView>> GetCreditMemos(int start, int count, string? filter) =>
        CreditMemos
            .Include(cm => cm.Customer)
            .Where(cm =>
                string.IsNullOrWhiteSpace(filter) || cm.DocumentNumber.ToLower().Contains(filter.ToLower().Trim()) ||
                cm.Customer.Name.ToLower().Contains(filter.ToLower().Trim()))
            .OrderByDescending(c => c.DocumentDate)
            .Skip(start)
            .Take(count)
            .Select(x => MemoListView.From(x))
            .ToListAsync();
    
    public Task<List<MemoListView>> GetDebitMemos(int start, int count, string? filter) =>
        DebitMemos
            .Include(dm => dm.Customer)
            .Include(dm => dm.ReceivableReason)
            .Include(dm => dm.Applies)
            .ThenInclude(a => a.Invoice)
            .ThenInclude(i => i.Applies)
            .ThenInclude(a => a.Payment)
            .Include(cm => cm.Applies)
            .ThenInclude(a => a.Invoice)
            .ThenInclude(i => i.Subscription)
            .Where(dm =>
                string.IsNullOrWhiteSpace(filter) || dm.DocumentNumber.ToLower().Contains(filter.ToLower().Trim()) ||
                dm.Customer.Name.ToLower().Contains(filter.ToLower().Trim()))
            .OrderByDescending(c => c.DocumentDate)
            .Skip(start)
            .Take(count)
            .Select(x => MemoListView.From(x))
            .ToListAsync();

    public Task<List<SubscriptionListView>> GetSubscriptions(int start, int count, string? filter, int? customer_id,
        int? status) =>
        Subscriptions
            .Include(s => s.Customer)
            .Include(s => s.Frequency)
            .Include(s => s.ShippingAddress)
            .ThenInclude(x => x.City)
            .Where(s => string.IsNullOrWhiteSpace(filter) ||
                        s.SubscriptionNumber.ToLower().Contains(filter.ToLower().Trim()) ||
                        s.Customer.Name.ToLower().Contains(filter.ToLower().Trim()))
            .Where(s => !customer_id.HasValue || s.CustomerId == customer_id.Value)
            .Where(s => status == null || s.Status == status)
            .OrderByDescending(c => c.Status)
            .ThenByDescending(c => c.CreatedOn)
            .Skip(start)
            .Take(count)
            .Select(x => SubscriptionListView.From(x))
            .ToListAsync();

    public Task<List<ChargeListView>> GetCharges(int start, int count, string? filter) =>
        Charges
            .Include(c => c.Customer)
            .Include(c => c.Subscription)
            .Include(c => c.Product)
            .ThenInclude(c => c.TaxSchedule)
            .Where(c => string.IsNullOrWhiteSpace(filter) ||
                        c.ChargeNumber.ToLower().Contains(filter.ToLower().Trim()) ||
                        c.Customer.Name.ToLower().Contains(filter.ToLower().Trim()))
            .OrderByDescending(c => c.Status)
            .ThenByDescending(c => c.CreatedOn)
            .Skip(start)
            .Take(count)
            .Select(x => ChargeListView.From(x))
            .ToListAsync();

    public Task<List<WorkOrderView>> GetWorkOrders(int start, int count, string? filter) =>
        WorkOrders
            .Include(wo => wo.Customer)
            .Include(wo => wo.CustomerAddress)
            .Include(wo => wo.Subscription)
            .Include(wo => wo.WorkOrderType)
            .Include(wo => wo.WorkOrderHistory)
            .Where(wo => string.IsNullOrWhiteSpace(filter) || wo.Number.Contains(filter.ToLower()) ||
                         wo.Customer.Name.ToLower().Contains(filter.ToLower().Trim()))
            .OrderByDescending(c => c.CreatedOn)
            .Skip(start)
            .Take(count)
            .Select(x => WorkOrderView.From(x))
            .ToListAsync();

    public Task<Customer360View> GetCustomer360View(int id) =>
        Customers
            .Where(c => c.Id == id)
            .Select(c => new Customer360View
            {
                Id = c.Id,
                Name = c.Name,
                Type = new Item(c.CustomerTypeId, c.CustomerType.Name),
                NcfType = new Item(c.NCFTypeId, c.NCFType.Name),
                Email = c.Email,
                Phone = c.MainPhone,
                Document = new DocumentView
                (
                    c.DocumentNumber,
                    new Item(c.DocumentTypeId, c.DocumentType.Name),
                    c.LegalRepresentativeCountryId == null
                        ? c.DocumentCountryId == null
                            ? null
                            : new Item(c.DocumentCountryId.Value, c.DocumentCountry.Name)
                        : new Item(c.LegalRepresentativeCountryId.Value, c.LegalRepresentativeCountry.Name)
                ),
                Status = new Item(c.Status, ((GenericStatus) c.Status).ToString()),
                MainAddress = c.BillingAddressId != null
                    ? new Item(c.BillingAddressId.Value, c.BillingAddress.AddressLine1)
                    : null,
                Balance = c.Invoices.Where(i => i.Status.Id != (int) Core.Enums.InvoiceStatuses.Anulado)
                    .Sum(i => i.DocumentAmount - i.PaidAmount),
                DateLastPayment = c.Payments.OrderByDescending(p => p.DocumentDate).FirstOrDefault() != null
                    ? c.Payments.OrderByDescending(p => p.DocumentDate).FirstOrDefault().DocumentDate
                    : null,
                DateLastInvoice = c.Invoices.OrderByDescending(i => i.DocumentDate).FirstOrDefault() != null
                    ? c.Invoices.OrderByDescending(i => i.DocumentDate).FirstOrDefault().DocumentDate
                    : null,
                Counts = new Customer360CountsView
                {
                    PendingClaims = c.Claims.Count(cl => cl.Result == null),
                    PendingPaymentArrangements =
                        c.PaymentAgreements.Count(pa => pa.Status == (int) Core.Enums.PaymentStatuses.Pendiente)
                },
                Notes = !c.Notes.Any()
                ? new List<NoteView>()
                : c.Notes.Select(n => new NoteView
                {
                    Id = n.Id,
                    Date = n.CreatedOn,
                    Note = n.Note,
                    Username = n.Username
                }).ToList(),
                Addresses = !c.Addresses.Any()
                    ? new List<CustomerAddressSummaryView>()
                    : c.Addresses.Select(a => new CustomerAddressSummaryView
                    {
                        Id = a.CustomerAddressId,
                        Name = a.CustomerAddress.Name,
                        Address = string.IsNullOrWhiteSpace(a.CustomerAddress.AddressLine1)? a.CustomerAddress.PlainAddress :
                            string.Join('\n', a.CustomerAddress.AddressLine1 + ", " +
                                                    (string.IsNullOrWhiteSpace(a.CustomerAddress.AddressLine2)
                                                        ? ""
                                                        : a.CustomerAddress.AddressLine2 + ", ") +
                                                    (string.IsNullOrWhiteSpace(a.CustomerAddress.AddressLine3)
                                                        ? ""
                                                        : a.CustomerAddress.AddressLine3 + ", ")),
                        City = a.CustomerAddress.City.Name,
                        PostalCode = a.CustomerAddress.PostalCode
                    }).ToList(),
                Subscriptions = !c.Subscriptions.Any()
                    ? new List<SubscriptionWindowView>()
                    : c.Subscriptions.Select(s => new SubscriptionWindowView
                    {
                        Id = s.Id,
                        Number = s.SubscriptionNumber,
                        Products = s.Lines.Select(l => new SubscriptionProductWindowView
                        {
                            Id = l.ProductId,
                            Name = l.Product.Name,
                        }).ToList(),
                        Status = new Item(s.Status, ((GenericStatus) s.Status).ToString())
                    }).ToList(),
                PendingInvoices = c.Invoices.Where(i =>
                    i.StatusId != (int) Core.Enums.InvoiceStatuses.Pagado &&
                    i.StatusId != (int) Core.Enums.InvoiceStatuses.IncluidaEnAcuerdo &&
                    i.StatusId != (int) Core.Enums.InvoiceStatuses.Anulado).Any()
                    ? c.Invoices
                        .Where(i => i.StatusId != (int) Core.Enums.InvoiceStatuses.Pagado &&
                                    i.StatusId != (int) Core.Enums.InvoiceStatuses.IncluidaEnAcuerdo &&
                                    i.StatusId != (int) Core.Enums.InvoiceStatuses.Anulado)
                        .OrderByDescending(i => i.DocumentDate)
                        .Take(5)
                        .Select(i => new InvoiceWindowView
                        {
                            Id = i.Id,
                            Number = i.DocumentNumber,
                            SubscriptionNumber = i.SubscriptionId != null ? i.Subscription.SubscriptionNumber : null,
                            Date = i.DocumentDate,
                            Amount = i.DocumentAmount,
                            PendingAmount = i.DocumentAmount - i.PaidAmount,
                            Status = new Item(i.StatusId, i.Status.Name)
                        })
                        .ToList()
                    : new List<InvoiceWindowView>(),
                LastPayments = !c.Payments.Any()
                    ? new List<PaymentWindowView>()
                    : c.Payments
                        .OrderByDescending(c => c.DocumentDate)
                        .Take(3)
                        .Select(p => new PaymentWindowView
                        {
                            Id = p.Id,
                            Type = new Item(p.PaymentMethodId, p.PaymentMethod.Name),
                            Invoices = p.Applies.Select(a => new PaymentInvoiceWindowView
                            {
                                Id = a.InvoiceId,
                                Number = a.Invoice.DocumentNumber
                            }).ToList(),
                            Amount = p.DocumentAmount,
                            Status = new Item(p.StatusId, p.Status.Name),
                            Date = p.DocumentDate
                        }).ToList(),
                ActiveTickets = !c.Tickets.Any()
                    ? new List<TicketWindowView>()
                    : c.Tickets
                        .OrderByDescending(c => c.CreatedOn)
                        .Take(5)
                        .Select(t => new TicketWindowView
                        {
                            Id = t.Id,
                            Type = new Item(t.CustomerTicketTypeId, t.CustomerTicketType.Name),
                            Subject = t.Subject,
                            Date = t.CreatedOn,
                            Status = new Item(t.Status, ((Core.Enums.CustomerTicketStatus) t.Status).ToString())
                        }).ToList()
            })
            .FirstOrDefaultAsync();

    public Task<InvoiceDetailedView> GetInvoiceView(int invoiceId) =>
        Invoices
            .Where(i => i.Id == invoiceId)
            .Select(i => new InvoiceDetailedView
            {
                Id = i.Id,
                Number = i.DocumentNumber,
                Date = i.DocumentDate,
                Customer = new InvoiceCustomerView()
                {
                    Id = i.CustomerId,
                    Name = i.Customer.Name,
                    Document = i.Customer.DocumentNumber,
                    Subscription = i.Subscription != null ? i.Subscription.SubscriptionNumber : null,
                },
                Amount = i.DocumentAmount,
                Paid = i.PaidAmount,
                DateLastPayment =
                    i.Applies.Any(p =>
                        p.Payment.StatusId != (int) Core.Enums.PaymentStatuses.Pendiente &&
                        p.Payment.StatusId != (int) Core.Enums.PaymentStatuses.Anulado)
                        ? i.Applies.Where(p =>
                                p.Payment.StatusId != (int) Core.Enums.PaymentStatuses.Pendiente &&
                                p.Payment.StatusId != (int) Core.Enums.PaymentStatuses.Anulado)
                            .OrderByDescending(p => p.ApplyDate).FirstOrDefault().ApplyDate
                        : null,
                Status = new Item(i.StatusId, i.Status.Name),
                Description = i.Description,
                Observation = i.Observation ?? "",
                Term = new Item(i.PaymentTermId, i.PaymentTerm.Name),
                NCFType = new Item(i.NCFTypeId, i.NCFType.Name),
                NCFDocument = i.TaxRegistrationNumber,
                NCFNumber = i.NCFNumber,
                BillingAddress = i.BillingAddressId != null? new CustomerAddressListView
                {
                    Id = i.BillingAddressId.Value,
                    Number = i.BillingAddress.Name,
                    FullAddress = string.IsNullOrWhiteSpace(i.BillingAddress.AddressLine1)? i.BillingAddress.PlainAddress : 
                        string.Join('\n', i.BillingAddress.AddressLine1 + ", " +
                                                    (string.IsNullOrWhiteSpace(i.BillingAddress.AddressLine2)
                                                        ? ""
                                                        : i.BillingAddress.AddressLine2 + ", ") +
                                                    (string.IsNullOrWhiteSpace(i.BillingAddress.AddressLine3)
                                                        ? ""
                                                        : i.BillingAddress.AddressLine3 + ", "),
                        i.BillingAddress.City.Name + ", " + i.BillingAddress.City.State.Name + ", " +
                        i.BillingAddress.City.State.Country.Name),
                    Line1 = string.IsNullOrWhiteSpace(i.BillingAddress.AddressLine1)? i.BillingAddress.PlainAddress : i.BillingAddress.AddressLine1,
                    City = new Item(i.BillingAddress.CityId, i.BillingAddress.City.Name),
                    SiteType = new Item(i.BillingAddress.PropertyTypeId, i.BillingAddress.PropertyType.Name),
                    Customer = new CustomerSummaryView()
                    {
                        Id = i.CustomerId,
                        Name = i.Customer.Name,
                        Document = i.Customer.DocumentNumber,
                    },
                    Status = new Item(i.BillingAddress.Status, ((GenericStatus) i.BillingAddress.Status).ToString()),
                } : null,
                ShippingAddress = i.ShippingAddressId != null? new CustomerAddressListView
                {
                    Id = i.ShippingAddressId.Value,
                    Number = i.ShippingAddress.Name,
                    FullAddress = string.IsNullOrWhiteSpace(i.ShippingAddress.AddressLine1)? i.ShippingAddress.PlainAddress : string.Join('\n', i.ShippingAddress.AddressLine1 + ", " +
                                                    (string.IsNullOrWhiteSpace(i.ShippingAddress.AddressLine2)
                                                        ? ""
                                                        : i.ShippingAddress.AddressLine2 + ", ") +
                                                    (string.IsNullOrWhiteSpace(i.ShippingAddress.AddressLine3)
                                                        ? ""
                                                        : i.ShippingAddress.AddressLine3 + ", "),
                        i.ShippingAddress.City.Name + ", " + i.ShippingAddress.City.State.Name + ", " +
                        i.ShippingAddress.City.State.Country.Name),
                    Line1 = string.IsNullOrWhiteSpace(i.ShippingAddress.AddressLine1)? i.ShippingAddress.PlainAddress : i.ShippingAddress.AddressLine1,
                    City = new Item(i.ShippingAddress.CityId, i.ShippingAddress.City.Name),
                    SiteType = new Item(i.ShippingAddress.PropertyTypeId, i.ShippingAddress.PropertyType.Name),
                    Customer = new CustomerSummaryView()
                    {
                        Id = i.CustomerId,
                        Name = i.Customer.Name,
                        Document = i.Customer.DocumentNumber,
                    },
                    Status = new Item(i.ShippingAddress.Status, ((GenericStatus) i.ShippingAddress.Status).ToString()),
                } : null,
                Subtotal = i.Lines.Sum(i => i.SubtotalAmount),
                Tax = i.Lines.Sum(i => i.TaxAmount),
                Details = i.Lines.Select(l => new InvoiceLineView
                {
                    Product = new ProductListView
                    {
                        Id = l.Product.Id,
                        Number = l.Product.Barcode,
                        Name = l.Product.Name,
                        Type = new Item(l.Product.PriceTypeId, l.Product.PriceType.Name),
                        Description = l.Product.Description,
                        Price = l.Product.UnitPrice,
                        Status = Item.From((GenericStatus) l.Product.Status)
                    },
                    Quantity = l.Quantity,
                    Tax = l.TaxAmount,
                    Total = l.TotalAmount
                }).ToList(),
                Applies = i.Applies.Select(a => new InvoiceApplyView
                {
                    Date = a.ApplyDate,
                    Amount = a.ApplyAmount,
                    Status = new Item(a.Payment.StatusId, a.Payment.Status.Name),
                }).ToList()
            })
            .FirstOrDefaultAsync();

    public Task<List<DayLotsView>> GetLotViews(int start, int count, LotSearchData data) =>
        Lots
            .Include(l => l.User)
            .Where(l => data.From == null || l.DateCreated.Date >= data.From.Value.Date)
            .Where(l => data.UserId == null || l.UserId == data.UserId)
            .Where(l => data.To == null || l.DateCreated.Date <= data.To.Value.Date)
            .Where(l => data.Status == null || (l.DateClosed.HasValue ? 1 : 0) == data.Status)
            .GroupBy(l => l.DateCreated.Date)
            .Select(l => new DayLotsView(
                    l.Key,
                    l.Select(l => new LotSummaryView(
                            l.Id,
                            l.DateCreated,
                            l.DateClosed,
                            l.Payments.Sum(p => p.AppliedAmount),
                            new UserSummaryView(
                                l.User.Id,
                                l.User.FirstName + " " + l.User.LastName,
                                l.User.Email,
                                l.User.Username
                            ),
                            new Item(l.DateClosed.HasValue ? 1 : 0, l.DateClosed.HasValue ? "Cerrado" : "Abierto")
                        )
                    ).ToList()
                )
            )
            .Skip(start)
            .Take(count)
            .ToListAsync();

    public Task<int> GetLotCount(LotSearchData data) =>
        Lots
            .Where(l => l.DateCreated.Date >= data.From)
            .Where(l => data.UserId == null || l.UserId == data.UserId)
            .Where(l => data.To == null || l.DateCreated.Date <= data.To)
            .Where(l => data.Status == null || (l.DateClosed.HasValue ? 1 : 0) == data.Status)
            .GroupBy(l => l.DateCreated.Date)
            .CountAsync();

    public Task<LotView> GetLotView(Guid id) =>
        Lots
            .Include(l => l.User)
            .Include(l => l.Payments)
            .ThenInclude(p => p.PaymentMethod)
            .Include(l => l.Balances)
            .ThenInclude(b => b.Currency)
            .Where(l => l.Id == id)
            .Select(l => LotView.From(l))
            .FirstOrDefaultAsync();

    public Task<LotBalanceClosingView> GetLotBalanceView(Guid id) =>
        Lots
            .Where(l => l.UserId == id && !l.DateClosed.HasValue)
            .Include(l => l.Payments)
            .ThenInclude(p => p.PaymentMethod)
            .Include(l => l.Payments)
            .ThenInclude(p => p.Currency)
            .Select(l => LotBalanceClosingView.From(l))
            .FirstOrDefaultAsync();
    

    public Task<MemoDetailedView> GetMemoView(int id, ReceivableReasonTypes type) =>
        type == ReceivableReasonTypes.CreditMemo
            ? CreditMemos
                .Where(m => m.Id == id)
                .Include(cm => cm.Customer)
                .Include(cm => cm.ReceivableReason)
                .Include(dm => dm.Applies)
                .ThenInclude(a => a.Invoice)
                .ThenInclude(i => i.Applies)
                .ThenInclude(a => a.Payment)
                .Include(dm => dm.Applies)
                .ThenInclude(a => a.Invoice)
                .ThenInclude(i => i.Subscription)
                .Select(m => MemoDetailedView.From(m))
                .FirstOrDefaultAsync()
            : DebitMemos
                .Where(m => m.Id == id)
                .Include(cm => cm.Customer)
                .Include(cm => cm.ReceivableReason)
                .Include(dm => dm.Applies)
                .ThenInclude(a => a.Invoice)
                .ThenInclude(i => i.Applies)
                .ThenInclude(a => a.Payment)
                .Include(dm => dm.Applies)
                .ThenInclude(a => a.Invoice)
                .ThenInclude(i => i.Subscription)
                .Select(m => MemoDetailedView.From(m))
                .FirstOrDefaultAsync();

    public Task<ClaimView> GetClaimView(int claimId) =>
        Claims
            .Where(c => c.Id == claimId)
            .Include(c => c.Customer)
            .Include(c => c.Invoices)
            .ThenInclude(i => i.Invoice)
            .ThenInclude(i => i.Customer)
            .Include(c => c.Invoices)
            .ThenInclude(i => i.Invoice)
            .ThenInclude(i => i.Applies)
            .ThenInclude(a => a.Payment)
            .Include(c => c.Subscription)
            .Include(c => c.ClaimType)
            .Include(c => c.ClaimMotive)
            .Include(c => c.LegalInstance)
            .Include(c => c.WorkOrders)
            .ThenInclude(c => c.WorkOrderType)
            .Include(c => c.WorkOrders)
            .ThenInclude(c => c.WorkOrderHistory)
            .Select(c => ClaimView.From(c))
            .FirstOrDefaultAsync();

    public Task<SubscriptionDetailedView> GetSubscriptionView(int id) =>
        Subscriptions
            .Where(s => s.Id == id)
            .Select(s => new SubscriptionDetailedView
            {
                Id = s.Id,
                Number = s.SubscriptionNumber,
                Customer = new CustomerSummaryView()
                {
                    Id = s.CustomerId,
                    Name = s.Customer.Name,
                    Document = s.Customer.DocumentNumber,
                },
                DateStart = s.SubscribedDate,
                Frequency = new Item(s.FrequencyId, s.Frequency.Name),
                Status = new Item(s.Status, ((GenericStatus) s.Status).ToString()),
                LastStatusUser = s.LastStatusByUserId == null
                    ? null
                    : new UserSummaryView()
                    {
                        Id = s.LastStatusByUserId.Value,
                        Name = s.LastStatusByUser.FirstName + " " + s.LastStatusByUser.LastName,
                        Email = s.LastStatusByUser.Email ?? "",
                        Username = s.LastStatusByUser.Username,
                    },
                LastStatusChange = s.LastStatusChange,
                VoidReason = s.VoidReason,
                DateEnd = s.DueDate,
                Type = new Item(s.SubscriptionTypeId, s.SubscriptionType.Name),
                
                Address = new SubscriptionAddressView()
                {
                    Billing = new CustomerAddressListView(
                        s.BillingAddressId,
                        s.BillingAddress.Name,
                        string.IsNullOrWhiteSpace(s.BillingAddress.AddressLine1)? s.BillingAddress.PlainAddress :
                            string.Join('\n', s.BillingAddress.AddressLine1 + ", " +
                                          (string.IsNullOrWhiteSpace(s.BillingAddress.AddressLine2)
                                              ? ""
                                              : s.BillingAddress.AddressLine2 + ", ") +
                                          (string.IsNullOrWhiteSpace(s.BillingAddress.AddressLine3)
                                              ? ""
                                              : s.BillingAddress.AddressLine3 + ", "),
                            s.BillingAddress.City.Name + ", " + s.BillingAddress.City.State.Name + ", " +
                            s.BillingAddress.City.State.Country.Name),
                        string.IsNullOrWhiteSpace(s.BillingAddress.AddressLine1)? s.BillingAddress.PlainAddress : s.BillingAddress.AddressLine1,
                        new Item(s.BillingAddress.CityId, s.BillingAddress.City.Name),
                        new Item(s.BillingAddress.PropertyTypeId, s.BillingAddress.PropertyType.Name),
                        new CustomerSummaryView(s.CustomerId, s.Customer.Name, s.Customer.DocumentNumber),
                        new Item(s.BillingAddress.Status, ((GenericStatus) s.BillingAddress.Status).ToString()), 
                        null,
                        null),
                    Shipping = new CustomerAddressListView(
                        s.ShippingAddressId,
                        s.ShippingAddress.Name,
                        string.IsNullOrWhiteSpace(s.ShippingAddress.AddressLine1)? s.ShippingAddress.PlainAddress : 
                            string.Join('\n', s.ShippingAddress.AddressLine1 + ", " +
                                          (string.IsNullOrWhiteSpace(s.ShippingAddress.AddressLine2)
                                              ? ""
                                              : s.ShippingAddress.AddressLine2 + ", ") +
                                          (string.IsNullOrWhiteSpace(s.ShippingAddress.AddressLine3)
                                              ? ""
                                              : s.ShippingAddress.AddressLine3 + ", "),
                            s.ShippingAddress.City.Name + ", " + s.ShippingAddress.City.State.Name + ", " +
                            s.ShippingAddress.City.State.Country.Name),
                        string.IsNullOrWhiteSpace(s.ShippingAddress.AddressLine1)? s.ShippingAddress.PlainAddress : s.ShippingAddress.AddressLine1,
                        new Item(s.ShippingAddress.CityId, s.ShippingAddress.City.Name),
                        new Item(s.ShippingAddress.PropertyTypeId, s.ShippingAddress.PropertyType.Name),
                        new CustomerSummaryView(s.CustomerId, s.Customer.Name, s.Customer.DocumentNumber),
                        new Item(s.ShippingAddress.Status, ((GenericStatus) s.ShippingAddress.Status).ToString()),
                        null,
                        null)
                },
                Term = new Item(s.PaymentTermId, s.PaymentTerm.Name),
                Meter = s.ShippingAddressId != null && s.ShippingAddress.MeterId != null ? new SubscriptionMeterView(
                    s.ShippingAddress.MeterId.Value,
                    s.ShippingAddress.Meter.MeterNumber,
                    s.ShippingAddress.Meter.MeterModel.Brand,
                    s.ShippingAddress.Meter.MeterModel.Model,
                    s.ShippingAddress.Meter.MeterModel.Telemetry,
                    s.ShippingAddress.Meter.MeterModel.ReadingTypes.Any() ? s.ShippingAddress.Meter.MeterModel.ReadingTypes.Select(r => new Item( r.ReadingTypeId, r.ReadingType.Name)).ToArray() : null
                ) : null,
                BillingCycle = s.BillingCycleId == null
                    ? null
                    : new BillingCycleListView(
                        s.BillingCycle.Id,
                        s.BillingCycle.Name,
                        s.BillingCycle.Description,
                        s.BillingCycle.BillingCyclePeriods.Where(bcp => bcp.BillingDate >= DateTime.Now)
                            .FirstOrDefault() == null
                            ? null
                            : s.BillingCycle.BillingCyclePeriods.Where(bcp => bcp.BillingDate >= DateTime.Now)
                                .OrderBy(bcp => bcp.BillingDate).FirstOrDefault().BillingDate,
                        new Item
                        {
                            Id = s.BillingCycle.BillingSchedule.Id,
                            Description = s.BillingCycle.BillingSchedule.Name,
                        },
                        Item.From((GenericStatus) s.BillingCycle.Status),
                        s.BillingCycle.BillingCyclePeriods.Any()
                    ),
                DeliveryMethod = new Item(s.DeliveryMethodId, s.DeliveryMethod.Name),
                BillingType = new Item(s.BillingTypeId, s.BillingType.Name),
                WorkOrderCount = s.WorkOrders.Count,
                ClaimCount = s.Customer.Claims.Count,
                PaymentArrangementCount = s.Invoices.Where(x => x.PaymentAgreementId != null)
                    .Select(x => x.PaymentAgreementId).Distinct().Count(),
                TicketCount = s.Customer.Tickets.Count,
                Notes = s.Notes.Any()
                    ? s.Notes.Select(n => new NoteView(n.Id, n.CreatedOn, n.Note, n.Username)).ToList()
                    : new List<NoteView>(),
                Invoices = !s.Invoices.Any()
                    ? new List<SubscriptionInvoiceView>()
                    : s.Invoices.Select(i => new SubscriptionInvoiceView(
                        i.Id,
                        i.DocumentNumber,
                        i.DocumentDate,
                        i.DocumentAmount,
                        i.PendingBalance,
                        i.PaidAmount,
                        Item.From((InvoiceStatuses) i.StatusId)
                    )).ToList(),
                Payments = !s.Payments.Any()
                    ? new List<SubscriptionPaymentView>()
                    : s.Payments.Select(i => new SubscriptionPaymentView(
                        i.Id,
                        i.DocumentNumber,
                        i.Applies.Select(a => a.Invoice).Select(x => x.DocumentNumber).ToList(),
                        i.DocumentAmount,
                        i.DocumentDate,
                        Item.From((PaymentStatuses) i.StatusId)
                    )).ToList(),
                RelatedPersons = !s.RelatedPersons.Any()
                    ? new List<SubscriptionRelatedPersonView>()
                    : s.RelatedPersons.Select(i => new SubscriptionRelatedPersonView(
                        i.Id,
                        i.FullName,
                        Item.From((Relationships) i.RelationshipId),
                        i.PhoneNumber
                    )).ToList(),
                Files = !s.Files.Any()
                    ? new List<SubscriptionFilesView>()
                    : s.Files.Select(i => new SubscriptionFilesView(
                        i.Id,
                        i.Name,
                        i.FileName.Substring(i.FileName.LastIndexOf('.') + 1)
                    )).ToList(),
                Products = s.Lines.Select(l => new SubscriptionLineView(
                    new ProductListView(
                        l.ProductId,
                        l.Product.Barcode,
                        l.Product.Name,
                        l.Product.Description,
                        new Item(l.Product.PriceTypeId, l.Product.PriceType.Name),
                        l.Product.UnitPrice,
                        l.Product.TaxSchedule.TaxRate,
                        new Item(l.Product.Status, ((GenericStatus) l.Product.Status).ToString())),
                    l.Quantity)).ToList(),
                Electrical = s.IsElectrical
                    ? new SubscriptionElectricalView(
                        s.Power.Value,
                        new Item(s.VoltageId.Value, s.Voltage.Name),
                        new Item(s.ConnectionTypeId.Value, s.ConnectionType.Name),
                        new Item(s.BranchId.Value, s.Branch.Code)
                    )
                    : null,
            })
            .FirstOrDefaultAsync();

    public Task<List<AnomalyListView>> GetAnomalies(int start, int count, string filter, int[]? status_id) =>
        Anomalies
            .Where(a => string.IsNullOrWhiteSpace(filter) || a.Description.ToLower().Contains(filter.ToLower()))
            .Where(a => status_id == null || status_id.Contains(a.StatusId))
            .OrderByDescending(a => a.CreatedOn)
            .Skip(start)
            .Take(count)
            .Include(a => a.Subscription)
            .Select(a => AnomalyListView.From(a))
            .ToListAsync();
    
    public Task<int> GetAnomalyCount(string filter, int[]? status_id) =>
        Anomalies
            .Where(a => string.IsNullOrWhiteSpace(filter) || a.Description.ToLower().Contains(filter.ToLower()))
            .Where(a => status_id == null || status_id.Contains(a.StatusId))
            .CountAsync();

    public Task<AnomalyView> GetAnomaly(int id) =>
        Anomalies
            .Where(a => a.Id == id)
            .Include(a => a.Subscription)
            .ThenInclude(s => s.Customer)
            .Include(a => a.Invoice)
            .Include(a =>a.ResolvedByUser)
            .Select(a => AnomalyView.From(a))
            .FirstOrDefaultAsync();
    public Task<List<string>> GetCashiers() =>
        Payments
            .Select(p => p.PointOfSale)
            .Distinct()
            .ToListAsync();

    public Task<Subscription> GetSubscriptionBillingData(int subscriptionId) =>
        Subscriptions
            .Include(s => s.Lines)
            .ThenInclude(l => l.Product)
            .ThenInclude(p => p.TaxSchedule)
            .Include(s => s.Lines)
            .ThenInclude(l => l.Product)
            .ThenInclude(p => p.Rates)
            .ThenInclude(r => r.RateScales)
            .Include(s => s.Lines)
            .ThenInclude(l => l.Product)
            .ThenInclude(p => p.Rates)
            .ThenInclude(r => r.ExtraCharges)
            .ThenInclude(ec => ec.Product)
            .ThenInclude(p => p.Rates)
            .ThenInclude(r => r.RateScales)
            .Include(s => s.Lines)
            .ThenInclude(l => l.Product)
            .ThenInclude(p => p.Rates)
            .ThenInclude(r => r.ExtraCharges)
            .ThenInclude(ec => ec.Product)
            .ThenInclude(p => p.TaxSchedule)
            .Include(s => s.Branch)
            .Include(s => s.PaymentTerm)
            .Include(s => s.Customer)
            .Include(s => s.BillingCycle)
            .ThenInclude(bc => bc.BillingCyclePeriods)
            .ThenInclude(bcp => bcp.BillingPeriod)
            .Where(s => s.Id == subscriptionId)
            .AsNoTracking()
            .FirstAsync();
    
    public Task<ProductDetailedView> GetProductView(int id) =>
        Products
            .Where(p => p.Id == id)
            .Include(p => p.Rates)
            .ThenInclude(r => r.RateScales)
            .Include(p => p.Rates)
            .ThenInclude(r => r.ExtraCharges)
            .ThenInclude(ec => ec.Product)
            .Include(p => p.PriceType)
            .Include(p => p.TaxSchedule)
            .Select(p => ProductDetailedView.From(p))
            .FirstOrDefaultAsync();

    public Task<BillingBatch> GetBillingBatchForProcessing(int batchId) =>
        BillingBatches
            .Where(b => b.Id == batchId)
            .Include(b => b.Lines)
            .AsNoTracking()
            .FirstOrDefaultAsync();

    public Task<BillingBatchDetailedView> GetBillingBatchView(int bbatchId) =>
        BillingBatches
            .Where(b => b.Id == bbatchId)
            .Select(b => new BillingBatchDetailedView
            {
                Id = b.Id,
                Number = b.Number,
                Date = b.Date,
                DateProcessed = b.InvoicedOn,
                Count = b.Lines.Count,
                Pending = b.Lines.Count(l => l.Status == (int)BillingBatchLineStatuses.Pending),
                Success = b.Lines.Count(l => l.Status == (int)BillingBatchLineStatuses.Success),
                Error = b.Lines.Count(l => l.Status == (int)BillingBatchLineStatuses.Failed),
                Status = new Item(b.Status, ((BillingBatchStatuses)b.Status).ToString()),
                Lines = b.Lines.Where(l => l.Status != (int)BillingBatchLineStatuses.Failed).Select(l => new BillingBatchLineView
                {
                    Id = l.Id,
                    Subscription = new Item(l.SubscriptionId, l.Subscription.SubscriptionNumber),
                    Invoice = l.InvoiceId != null? new Item(l.InvoiceId.Value, l.Invoice.DocumentNumber) : null,
                    Result = l.Result,
                    Status = new Item(l.Status, ((BillingBatchLineStatuses)l.Status).ToString())
                }).ToList(),
                Errors = b.Lines.Where(l => l.Status == (int)BillingBatchLineStatuses.Failed).Select(l => new BillingBatchLineView
                {
                    Id = l.Id,
                    Subscription = new Item(l.SubscriptionId, l.Subscription.SubscriptionNumber),
                    Invoice = l.InvoiceId != null ? new Item(l.InvoiceId.Value, l.Invoice.DocumentNumber) : null,
                    Result = l.Result,
                    Status = new Item(l.Status, ((BillingBatchLineStatuses)l.Status).ToString())
                }).ToList()
            })
            .FirstOrDefaultAsync();

    public Task<List<MeterQueryView>> GetMeterQuery(string? number, string? subscription, int? clientId) =>
        Meters
            .Where(m => m.CustomerAddress != null)
            .Where(m => (number == null || m.MeterNumber == number) &&
                        (subscription == null || (m.CustomerAddress.Subscription != null && m.CustomerAddress.Subscription.SubscriptionNumber == subscription)) &&
                        (clientId == null || m.CustomerAddress.Customers.Any(x => x.CustomerId == clientId)))
            .Select(m => new MeterQueryView(
                m.Id,
                m.MeterNumber,
                string.IsNullOrWhiteSpace(m.CustomerAddress.AddressLine1)? m.CustomerAddress.PlainAddress : 
                    string.Join('\n', m.CustomerAddress.AddressLine1 + ", " +
                                      (string.IsNullOrWhiteSpace(m.CustomerAddress.AddressLine2)
                                          ? ""
                                          : m.CustomerAddress.AddressLine2 + ", ") +
                                      (string.IsNullOrWhiteSpace(m.CustomerAddress.AddressLine3)
                                          ? ""
                                          : m.CustomerAddress.AddressLine3 + ", "),
                        m.CustomerAddress.City.Name + ", " + m.CustomerAddress.City.State.Name + ", " +
                        m.CustomerAddress.City.State.Country.Name),
                m.CustomerAddress.Subscription != null? m.CustomerAddress.Subscription.SubscriptionNumber : null,
                m.CustomerAddress.Customers.Any()? m.CustomerAddress.Customers.Select(c => new Item(c.CustomerId, c.Customer.Name)).ToList() : null))
            .ToListAsync();

    public Task<DetailedMeterQueryView> GetDetailedMeterQuery(int meterId) =>
        Meters
            .Where(m => m.Id == meterId)
            .Select(m => new DetailedMeterQueryView
            {
                Id = m.Id,
                Number = m.MeterNumber,
                Brand = m.MeterModel.Brand,
                Model = m.MeterModel.Model,
                Types = m.MeterModel.ReadingTypes.Select(r => new Item(r.ReadingTypeId, r.ReadingType.Name)).ToList(),
                InstallationDate = m.CreatedOn,
                Rates = m.CustomerAddress.Subscription != null
                    ? m.CustomerAddress.Subscription.Lines != null && m.CustomerAddress.Subscription.Lines
                        .Where(l => l.Product.Rates != null && l.Product.Rates.Any()).Any()
                        ? m.CustomerAddress.Subscription.Lines.Where(l => l.Product.Rates != null && l.Product.Rates.Any())
                            .Select(l => new Item(l.ProductId, l.Product.Name)).ToList()
                        : null
                    : null,
            })
            .FirstOrDefaultAsync();
}