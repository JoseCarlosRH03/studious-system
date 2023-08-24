using System.Text.Json;

namespace BackendTemplateAPI.Services.Data;

public partial class DataService
{
    
    #region User Management
    public async Task RegisterLogin(Guid userId)
    {
        await Audits.AddAsync(new Audit
        {
            UserId = userId,
            Type = (int) AuditTypes.Login,
            TableName = "Users",
            DateTime = DateTime.Now,
            PrimaryKey = JsonSerializer.Serialize(new Dictionary<string, Guid> {{"Id", userId}})
        });
        await SaveChangesAsync();
    }

    public async Task<Guid> LockoutUser(User user, int days = 0, int hours = 0)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (days == 0 && hours == 0) user.LockoutEnd = DateTime.Now.AddYears(100);
        else user.LockoutEnd = DateTime.Now.AddDays(days).AddHours(hours);

        Users.Update(user);

        await Audits.AddAsync(new Audit
        {
            UserId = user.Id,
            Type = (int) AuditTypes.Lockout,
            TableName = "Users",
            DateTime = DateTime.Now,
            OldValues = JsonSerializer.Serialize(new Dictionary<string, object?> {{"LockoutEnd", null}}),
            NewValues = JsonSerializer.Serialize(new Dictionary<string, object?> {{"LockoutEnd", user.LockoutEnd}}),
            AffectedColumns = JsonSerializer.Serialize(new List<string> {"LockoutEnd"}),
            PrimaryKey = JsonSerializer.Serialize(new Dictionary<string, Guid> {{"Id", user.Id}})
        });
        await SaveChangesAsync();
        return user.Id;
    }

    public async Task<Guid> UnlockoutUser(User user)
    {
        await Audits.AddAsync(new Audit
        {
            UserId = user.Id,
            Type = (int) AuditTypes.Unlockout,
            TableName = "Users",
            DateTime = DateTime.Now,
            OldValues = JsonSerializer.Serialize(new Dictionary<string, object?> {{"LockoutEnd", user.LockoutEnd}}),
            NewValues = JsonSerializer.Serialize(new Dictionary<string, object?> {{"LockoutEnd", DateTime.Now}}),
            AffectedColumns = JsonSerializer.Serialize(new List<string> {"LockoutEnd"}),
            PrimaryKey = JsonSerializer.Serialize(new Dictionary<string, Guid> {{"Id", user.Id}})
        });
        user.LockoutEnd = DateTime.Now;
        Users.Update(user);
        await SaveChangesAsync();
        return user.Id;
    }
    #endregion

    #region API
    public Task<List<APIRouteView>> GetAPIRouteViews() =>
        Routes
            .Select(r => new APIRouteView {
                Id = r.Id,
                CompanyId = null,
                Number = r.Id.ToString("000000"),
                Description = r.Name,
                Type = r.RouteType.Name,
                Points = r.Addresses.Select(a => new APIRoutePointView
                {
                    Id = a.Id,
                    RouteId = r.Id,
                    AddressId = a.Id,
                    Subscription = a.Subscription == null ? "" : a.Subscription.SubscriptionNumber,
                    Address = new APIAddressDataView
                    {
                        Id = a.Id,
                        AddressId = a.Id.ToString(),
                        Address = $"{a.AddressLine1}\n{a.AddressLine2}\n{a.AddressLine3}\n{a.City.Name}",
                        Sector = a.AddressLine3,
                        GeoLongitude = a.Longitude == null ? null : (decimal) a.Longitude,
                        GeoLatitude = a.Latitude == null ? null : (decimal) a.Latitude,
                        MeterId = a.Meter.Id,
                        MeterNumber = a.Meter.MeterNumber,
                        Client = a.Subscription.Customer.Name,
                        Transformer = a.Transformer
                    }
                }).ToList(),
                BillingDates = r.BillingCycle.BillingCyclePeriods.Select(bcp => new APIRouteBillingDateView
                {
                    Id = bcp.Id,
                    RouteId = r.Id,
                    Month = bcp.BillingDate.Month,
                    Day = bcp.BillingDate.Day,
                }).ToList()
            })
            .ToListAsync();
    #endregion

    #region Invoices
    public async Task ValidateInvoiceBalance(ICollection<PaymentApplyData> applies, int paymentId, ReceivableReasonTypes type)
    {
        if (applies.Count == 0)
            throw new InvalidParameter("No se encontraron facturas para aplicar el pago");
        if (applies.GroupBy(x => x.Id).Any(g => g.Count() > 1))
            throw new InvalidParameter("No se puede aplicar el pago a la misma factura mas de una vez");

        var applies_ids = applies.Select(a => a.Id).ToArray();
        var invoices = await Invoices.Where(i => applies_ids.Contains(i.Id)).ToArrayAsync();

        foreach (var apply in applies)
        {
            var invoice = invoices.FirstOrDefault(i => i.Id == apply.Id);
            if (invoice is null)
                throw new InvalidParameter($"No se encontro la factura con id {apply.Id}");
            if (invoice.StatusId == (int) Core.Enums.InvoiceStatuses.Anulado)
                throw new InvalidParameter($"La factura con id {apply.Id} se encuentra anulada");
            if (type is not (ReceivableReasonTypes.Payment or ReceivableReasonTypes.CreditMemo))
                continue;
            if (invoice.PendingBalance < apply.AmountToPay)
                throw new InvalidParameter(
                    $"El monto a aplicar es mayor al saldo de la factura {invoice.DocumentNumber}");
            if (apply.AmountToPay <= 0)
                throw new InvalidParameter($"El monto a aplicar debe ser mayor a cero");
        }

        switch (type)
        {
            case ReceivableReasonTypes.Payment:
                await AddRange(applies.Select(x => new PaymentApply
                {
                    ApplyAmount = x.AmountToPay,
                    ApplyDate = DateTime.Now,
                    InvoiceId = x.Id,
                    PaymentId = paymentId
                }));
                break;
            case ReceivableReasonTypes.CreditMemo:
                await AddRange(applies.Select(x => new CreditMemoApply
                {
                    ApplyAmount = x.AmountToPay,
                    ApplyDate = DateTime.Now,
                    InvoiceId = x.Id,
                    CreditMemoId = paymentId
                }));
                break;
            case ReceivableReasonTypes.DebitMemo:
                await AddRange(applies.Select(x => new DebitMemoApply
                {
                    ApplyAmount = x.AmountToPay,
                    ApplyDate = DateTime.Now,
                    InvoiceId = x.Id,
                    DebitMemoId = paymentId
                }));
                break;
        }
    }
    
    public async Task<Invoice> CreateInvoice(Invoice invoice)
    {
        try
        {
            await invoiceSemaphore.WaitAsync();
            await ncfSemaphore.WaitAsync();
            var sequence = NCFSequenceSettings.FirstOrDefault(n => n.NCFTypeId == invoice.NCFTypeId && n.Status == (int)GenericStatus.Activo);
            if (sequence == null)
                throw new Exception("No se ha configurado el tipo de comprobante fiscal o no se encuentra activo para ncf " + invoice.NCFTypeId);
            var result = sequence.LastSequence += 1;
            if (result > sequence.MaxSequence)
                throw new Exception("El número de secuencia ha alcanzado el máximo permitido para el tipo de comprobante fiscal " + sequence.Series + sequence.NCFTypeId.ToString().PadLeft(2, '0'));
            
            invoice.NCFNumber = $"{sequence.Series}{sequence.NCFTypeId.ToString().PadLeft(2, '0')}{result.ToString().PadLeft(8, '0')}";
            if (string.IsNullOrWhiteSpace(invoice.DocumentNumber))
                invoice.DocumentNumber = (Convert.ToInt32((await GetLast<Invoice>(x => x.PaymentAgreementId == null))?.DocumentNumber ?? "0") + 1).ToString();
            
            return await Add(invoice);
        }
        finally
        {
            invoiceSemaphore.Release();
            ncfSemaphore.Release();
        }
    }
    #endregion
    
    #region Payments
    public async Task<Payment> CreatePayment(Payment payment)
    {
        try
        {
            await paymentSemaphore.WaitAsync();
            payment.DocumentNumber =
                (Convert.ToInt32((await GetLast<Payment>())?.DocumentNumber ?? "0") + 1).ToString();
            return await Add(payment);
        }
        finally
        {
            paymentSemaphore.Release();
        }
    }
    
    public async Task<DebitMemo> CreateDebitMemo(DebitMemo debitMemo)
    {
        try
        {
            await ncfSemaphore.WaitAsync();
            debitMemo.DocumentNumber = "ND-" + (await Count<DebitMemo>() + 1);
            var sequence = NCFSequenceSettings.FirstOrDefault(n => n.NCFTypeId == (int)Core.Enums.NCFTypes.NotaDeDebito && n.Status == (int)GenericStatus.Activo);
            if (sequence == null)
                throw new Exception("No se ha configurado el tipo de comprobante fiscal o no se encuentra activo para ncf 0" + (int)Core.Enums.NCFTypes.NotaDeDebito);
            var result = sequence.LastSequence += 1;
            if (result > sequence.MaxSequence)
                throw new Exception("El número de secuencia ha alcanzado el máximo permitido para el tipo de comprobante fiscal " + sequence.Series + sequence.NCFTypeId.ToString().PadLeft(2, '0'));
            
            debitMemo.NCFNumber = $"{sequence.Series}{sequence.NCFTypeId.ToString().PadLeft(2, '0')}{result.ToString().PadLeft(8, '0')}";
            
            return await Add(debitMemo);
        }
        finally
        {
            ncfSemaphore.Release();
        }
    }

    public async Task<CreditMemo> CreateCreditMemo(CreditMemo creditMemo)
    {
        try
        {
            await ncfSemaphore.WaitAsync();
            creditMemo.DocumentNumber = "NC-" + (await Count<CreditMemo>() + 1);
            var sequence = NCFSequenceSettings.FirstOrDefault(n => n.NCFTypeId == (int)Core.Enums.NCFTypes.NotaDeCredito && n.Status == (int)GenericStatus.Activo);
            if (sequence == null)
                throw new Exception("No se ha configurado el tipo de comprobante fiscal o no se encuentra activo para ncf 0" + (int)Core.Enums.NCFTypes.NotaDeCredito);
            var result = sequence.LastSequence += 1;
            if (result > sequence.MaxSequence)
                throw new Exception("El número de secuencia ha alcanzado el máximo permitido para el tipo de comprobante fiscal " + sequence.Series + sequence.NCFTypeId.ToString().PadLeft(2, '0'));
            
            creditMemo.NCFNumber = $"{sequence.Series}{sequence.NCFTypeId.ToString().PadLeft(2, '0')}{result.ToString().PadLeft(8, '0')}";
            
            return await Add(creditMemo);
        }
        finally
        {
            ncfSemaphore.Release();
        }
    }
    #endregion
    
    #region Billing
    public Task GenerateBillingBatch(int bbatchId, DateTime date)
    {
        var cycleIds = BillingCycles
            .Where(bc => bc.BillingCyclePeriods.Any(bcp => bcp.BillingDate.Date == date.Date) && bc.Status == (int)GenericStatus.Activo)
            .Select(bc => bc.Id);
        
        if (!cycleIds.Any())
            throw new NotFound("No hay ciclos de facturación activos para la fecha seleccionada");
        
        var subscriptionsQuery = Subscriptions
            .Where(s => s.BillingCycleId != null 
                        && cycleIds.Any(c => c == s.BillingCycleId.Value))
            .Select(x => x.Id);

        if (!subscriptionsQuery.Any())
            throw new NotFound("No hay suscripciones activas para la fecha seleccionada con los ciclos de facturación activos seleccionados");
        
        var billingBatchPruneQuery = BillingBatches
            .Where(bb => bb.Date.Date == date)
            .Include(c => c.Lines);
        var subscriptionsPrunedQuery = subscriptionsQuery;
        if (billingBatchPruneQuery.Any())
            subscriptionsPrunedQuery = subscriptionsPrunedQuery
                .Where(s => !billingBatchPruneQuery.Any(bb => bb.Lines.Any(bbl => bbl.SubscriptionId == s && bbl.InvoiceId != null)));

        return AddRange(subscriptionsPrunedQuery.Select(s => new BillingBatchLine
        {
            BillingBatchId = bbatchId,
            SubscriptionId = s,
            Status = (int) BillingBatchLineStatuses.Pending,
        }));
    }
    
    public async Task VoidBatch(int bbatchId, Guid userId)
    {
        await Invoices.Where(i => i.BillingBatchLine != null && i.BillingBatchLine.BillingBatchId == bbatchId)
            .ExecuteUpdateAsync(i => i
                .SetProperty(i => i.VoidedOn, DateTime.Now)
                .SetProperty(i => i.VoidedById, userId));
    }
    
    public async Task<int> CreateAnomaly(AnomalyTypes type, Guid userId, int subscriptionId, string description, int? batchId)
    {
        var anomaly = new Anomaly
        {
            CreatedBy = userId,
            Description = description,
            BillingBatchId = batchId,
            Date = DateTime.Now,
            TypeId = (int)type,
            SubscriptionId = subscriptionId,
            StatusId = (int)Core.Enums.AnomalyStatuses.Generada,
        };
        return (await Add(anomaly)).Id;
    }

    #endregion
    
    #region Reports
    public Task<SubscriptionVoidReportData?> GetSubscriptionVoidReportData(int subscriptionId) =>
        Subscriptions
            .Where(s => s.Id == subscriptionId)
            .Select(s => new SubscriptionVoidReportData
            {
                Number = s.SubscriptionNumber,
                Reason = s.VoidReason,
                CancelDate = s.LastStatusChange.Value,
                Customer = new CustomerSubscriptionVoidReportData
                {
                    Name = s.Customer.Name,
                    Country = s.ShippingAddress.City.State.Country.Name,
                    City = s.ShippingAddress.City.Name,
                    State = s.ShippingAddress.City.State.Name,
                    Document = s.Customer.DocumentNumber,
                }
            })
            .FirstOrDefaultAsync();
    
    public Task<SubscriptionReportData> GetSubscriptionReportData(int subscriptionId)
    {
        var user_id = Subscriptions.Where(s => s.Id == subscriptionId).Select(s => s.CreatedBy).FirstOrDefault();
        var user = Users
            .Where(u => u.Id == user_id)
            .Include(u => u.Branch)
            .ThenInclude(b => b.City)
            .FirstOrDefault();
        return Subscriptions
            .Where(s => s.Id == subscriptionId)
            .Select(s => new SubscriptionReportData
            {
                Branch = user.Branch == null
                    ? null
                    : new ReportBranchData
                    {
                        Name = user.Branch.Code,
                        Code = user.Branch.Code,
                        Address = user.Branch.Address,
                        Phone = user.Branch.Phone,
                        City = user.Branch.City.Name,
                    },
                Customer = new SubscriptionCustomerReportData
                {
                    Id = s.Customer.Id,
                    Type = s.Customer.CustomerType.Name,
                    Name = s.Customer.Name,
                    DocumentNumber = s.Customer.DocumentNumber,
                    DocumentType = s.Customer.DocumentType.Name,
                    Country = s.Customer.DocumentTypeId == (int) Core.Enums.DocumentTypes.RNC
                        ? (s.Customer.LegalRepresentativeCountry == null
                            ? "República Dominicana"
                            : s.Customer.LegalRepresentativeCountry.Name)
                        : (s.Customer.DocumentCountry == null
                            ? "República Dominicana"
                            : s.Customer.DocumentCountry.Name),
                    Address = s.Customer.BillingAddress.AddressLine1,
                    Phone = s.Customer.MainPhone,
                    Email = s.Customer.Email,
                    RepresentativeName = s.Customer.LegalRepresentativeName == null
                        ? s.Customer.Name
                        : s.Customer.LegalRepresentativeName,
                    RepresentativeDocumentNumber = s.Customer.LegalRepresentativeDocumentNumber == null
                        ? s.Customer.DocumentNumber
                        : s.Customer.LegalRepresentativeDocumentNumber,
                },
                SubscriptionOwner = s.Customer.LegalRepresentativeName == null
                    ? s.Customer.Name
                    : s.Customer.LegalRepresentativeName,
                Address = s.ShippingAddress.AddressLine1,
                City = s.ShippingAddress.City.Name,
                State = s.ShippingAddress.City.State.Name,
                AreaType = s.ShippingAddress.AreaType.Name,
                PostalCode = s.ShippingAddress.PostalCode,
                Number = s.SubscriptionNumber,
                Product = s.Lines.Where(l => l.Product.PriceTypeId != (int) Core.Enums.PriceTypes.Fijo).Any()
                    ? s.Lines.Where(l => l.Product.PriceTypeId != (int) Core.Enums.PriceTypes.Fijo).First().Product.Name
                    : s.Lines.Any()
                        ? s.Lines.First().Product.Name
                        : "(Sin Productos)",
                ConnectionType = s.IsElectrical ? s.ConnectionType.Name : "No Eléctrico",
                Frequency = s.Frequency.Name,
                CompanyName = s.ShippingAddress.CompanyName ?? s.BillingAddress.CompanyName,
                CreatedBy = s.CreatedBy,
                CreatedByName = user.FirstName + " " + user.LastName,
            })
            .FirstOrDefaultAsync();
    }

    public Task<PaymentAgreementReportData> GetPaymentAgreementReportData(int paymentAgreementId) =>
        PaymentAgreements
            .Where(pa => pa.Id == paymentAgreementId)
            .Select(pa => new PaymentAgreementReportData
            {
                Number = pa.Number,
                SubscriptionNumber = pa.Subscription.SubscriptionNumber,
                TotalAmount = pa.Amount,
                Quantity = pa.Installments.Count,
                InterestRate = pa.InterestRate,
                Customer = new PaymentAgreementCustomerReportData
                {
                    Id = pa.CustomerId,
                    Document = pa.Customer.DocumentNumber,
                    Name = pa.Customer.Name,
                },
                Installments = pa.Installments.Select(i => new PaymentAgreementInstallmentReportData
                {
                    Id = i.Id,
                    Number = i.InstallmentNumber,
                    Date = i.InstallmentDate,
                    Capital = i.CapitalAmount,
                    InterestRate = i.InterestAmount,
                    Amount = i.InstallmentAmount
                }).ToList()
            })
            .FirstOrDefaultAsync();

    public Task<ClaimReportData> GetClaimReportData(int claimId)
    {
        var workOrders = Claims.Where(c => c.Id == claimId).SelectMany(c => c.WorkOrders);
        var workOrdersMapped = new List<ClaimWorkOrderReportData>();
        var users = Users.Where(u => workOrders.Select(w => w.CreatedBy).Distinct().Contains(u.Id)).Distinct().ToList();
        var branch = Users
            .Where(u => u.Id == Claims.Where(c => c.Id == claimId).Select(c => c.CreatedBy).FirstOrDefault())
            .Select(u => u.Branch).FirstOrDefault();
        foreach (var wo in workOrders)
        {
            workOrdersMapped.Add(new ClaimWorkOrderReportData
            {
                Id = wo.Id,
                Date = wo.CreatedOn,
                OrderAction =
                    $"Orden {wo.Number} creada de tipo {wo.WorkOrderType.Description} por {users.Where(u => u.Id == wo.CreatedBy).Select(u => u.FirstName + " " + u.LastName).FirstOrDefault()}",
                Description = wo.Description,
            });
            if (!wo.WorkOrderHistory.Any()) continue;
            foreach (var woh in wo.WorkOrderHistory)
            {
                workOrdersMapped.Add(new ClaimWorkOrderReportData
                {
                    Id = wo.Id,
                    Date = woh.CreatedDate,
                    OrderAction = $"<div><b>{wo.Number}</b></div>" + woh.Comments,
                    Description = woh.Status.Name,
                });
            }
        }

        workOrdersMapped = workOrdersMapped.OrderBy(wo => wo.Date).ToList();
        return Claims
            .Where(c => c.Id == claimId)
            .Select(c => new ClaimReportData
            {
                Branch = branch == null
                    ? null
                    : new ReportBranchData
                    {
                        Name = branch.Code,
                        Code = branch.Code,
                        Address = branch.Address,
                        Phone = branch.Phone,
                        City = branch.City.Name,
                    },
                CustomerName = c.Customer.Name,
                CustomerAddress = c.Subscription.ShippingAddress.AddressLine1,
                SubscriptionNumber = c.Subscription.SubscriptionNumber,
                Number = c.ClaimNumber,
                Motive = c.ClaimMotive.Name,
                Date = c.Date,
                Description = c.Comment,
                ClosingDate = c.ClosedDate,
                ResultType = ((Core.Enums.ClaimResults) c.Result).ToString(),
                ResultComment = c.ResultComment,
                Invoices = c.Invoices.Select(i => new ClaimInvoiceReportData
                {
                    Id = i.Invoice.Id,
                    Number = i.Invoice.DocumentNumber,
                    Date = i.Invoice.DocumentDate,
                    Amount = i.Invoice.DocumentAmount
                }).ToList(),
                WorkOrders = workOrdersMapped.ToList()
            })
            .FirstOrDefaultAsync();
    }

    public Task<List<InvoiceReportData>> GetInvoiceReportData(params int[] ids) =>
        Invoices
            .AsNoTracking()
            .Where(i => ids.Contains(i.Id))
            .Include(i => i.ShippingAddress)
            .ThenInclude(a => a.City)
            .Include(i => i.Subscription)
            .ThenInclude(s => s.ShippingAddress)
            .Include(i => i.Subscription)
            .ThenInclude(s => s.BillingAddress)
            .Include(i => i.NCFType)
            .Include(i => i.Applies)
            .ThenInclude(a => a.Payment)
            .Include(i => i.Customer)
            .ThenInclude(c => c.Invoices)
            .Include(i => i.Customer)
            .ThenInclude(c => c.DocumentType)
            .Include(i => i.Lines)
            .ThenInclude(l => l.Product)
            .OrderBy(i => i.ShippingAddress.City)
            .ThenBy(i => i.ShippingAddress.AddressLine1)
            .Select(i => new InvoiceReportData
            {
                Number = i.DocumentNumber,
                Description = i.Description,
                DocumentDate = i.DocumentDate,
                DueDate = i.NCFTypeId == (int)Core.Enums.NCFTypes.Gubernamental || i.NCFTypeId == (int)Core.Enums.NCFTypes.CreditoFiscal ? new DateTime(2024, 12, 31) : i.DueDate,
                FullShippingAddress = string.IsNullOrWhiteSpace(i.ShippingAddress.AddressLine1) ? i.ShippingAddress.PlainAddress :
                    $"{i.ShippingAddress.AddressLine1}{(string.IsNullOrWhiteSpace(i.ShippingAddress.AddressLine2) ? "" : ", " + i.ShippingAddress.AddressLine2)}{(string.IsNullOrWhiteSpace(i.ShippingAddress.AddressLine3) ? "" : ", " + i.ShippingAddress.AddressLine3)}{(string.IsNullOrWhiteSpace(i.ShippingAddress.City.Name) ? "" : ", " + i.ShippingAddress.City.Name)}",
                Phone = i.ShippingAddress.Phone,
                CompanyName = i.Subscription == null
                    ? (string.IsNullOrWhiteSpace(i.ShippingAddress.CompanyName)
                        ? i.BillingAddress.CompanyName
                        : i.ShippingAddress.CompanyName)
                    : (string.IsNullOrWhiteSpace(i.Subscription.ShippingAddress.CompanyName)
                        ? i.Subscription.BillingAddress.CompanyName
                        : i.Subscription.ShippingAddress.CompanyName),
                SubscriptionNumber = i.Subscription == null ? "" : i.Subscription.SubscriptionNumber,
                BillingCycle = i.SubscriptionId == null ? "" :
                    i.Subscription.BillingCycleId == null ? "" : i.Subscription.BillingCycle.Name,
                NCFType = i.NCFType.Name,
                NCFNumber = i.NCFNumber,
                AdvancedPayment = i.Applies
                    .Where(x => x.Payment.StatusId == (int) Core.Enums.PaymentStatuses.PagoASuscripcion)
                    .Sum(x => x.ApplyAmount),
                TotalDebt = i.Customer.Invoices.Where(ci => ci.Id != i.Id && ci.StatusId != (int)Core.Enums.InvoiceStatuses.Anulado).Sum(x => x.DocumentAmount - x.PaidAmount),
                Total = i.DocumentAmount,
                Customer = new ReportCustomerData
                {
                    Id = i.CustomerId,
                    Name = i.Customer.Name,
                    Document = i.Customer.DocumentNumber,
                    DocumentType = i.Customer.DocumentType.Name
                },
                Lines = i.Lines.Select(l => new InvoiceReportLineData
                {
                    ERPCode = l.Product.ERPCode ?? "",
                    Product = l.Product.Name,
                    Description = l.Product.Description,
                    Quantity = l.Quantity,
                    Price = l.TotalAmount
                }).ToList()
            })
            .ToListAsync();

    public Task<PaymentReportData> GetPaymentReportData(int id) =>
        Payments
            .Where(p => p.Id == id)
            .Include(p => p.Applies)
            .ThenInclude(a => a.Invoice)
            .ThenInclude(i => i.Subscription)
            .Include(p => p.Customer)
            .ThenInclude(c => c.DocumentType)
            .Include(p => p.Currency)
            .Include(p => p.PaymentMethod)
            .Include(p => p.Bank)
            .Select(p => new PaymentReportData
            {
                DocumentNumber = p.DocumentNumber,
                DocumentDateTime = p.DocumentDate,
                PaymentMethod = p.PaymentMethod.Name,
                CashAmount = p.CashAmount,
                CashBack = p.CashBack,
                BatchNumber = p.BatchNumber,
                CardNumber = p.CardNumber,
                Bank = p.BankId == null ? null : p.Bank.Name,
                Reference = p.Reference,
                Notes = p.Notes,
                PointOfSale = p.PointOfSale,
                Amount = p.DocumentAmount,
                Applies = p.Applies.Select(a => new PaymentApplyReportData
                {
                    InvoiceNumber = a.Invoice.DocumentNumber,
                    SubscriptionNumber = a.Invoice.SubscriptionId == null
                        ? null
                        : a.Invoice.Subscription.SubscriptionNumber,
                    Product = a.Invoice.Lines
                        .Where(l => !string.IsNullOrWhiteSpace(l.Product.ERPCode))
                        .Select(l => l.Product.ERPCode + "\n " + (!string.IsNullOrWhiteSpace(l.Product.Name)? l.Product.Name : l.Product.Description))
                        .Join("\n"),
                    Amount = a.ApplyAmount
                }).ToList(),
                Customer = new ReportCustomerData
                {
                    Id = p.CustomerId,
                    Name = p.Customer.Name,
                    Document = p.Customer.DocumentNumber,
                    DocumentType = p.Customer.DocumentType.Name
                }
            })
            .FirstOrDefaultAsync();

    public Task<PaymentVoidReportData> GetPaymentVoidReportData(int id) => 
        Payments
            .Where(p => p.Id == id)
            .Include(p => p.Applies)
            .ThenInclude(a => a.Invoice)
            .ThenInclude(i => i.Subscription)
            .Include(p => p.Customer)
            .ThenInclude(c => c.DocumentType)
            .Include(p => p.Currency)
            .Include(p => p.PaymentMethod)
            .Include(p => p.Bank)
            .Include(p => p.VoidedBy)
            .Select(p => new PaymentVoidReportData
            {
                DocumentNumber = p.DocumentNumber,
                DocumentDateTime = p.DocumentDate,
                PaymentMethod = p.PaymentMethod.Name,
                CashAmount = p.CashAmount,
                CashBack = p.CashBack,
                BatchNumber = p.BatchNumber,
                CardNumber = p.CardNumber,
                Bank = p.BankId == null ? null : p.Bank.Name,
                Reference = p.Reference,
                Notes = p.Notes,
                PointOfSale = p.PointOfSale,
                VoidReason = p.VoidReason,
                VoidedBy = p.VoidedById == null? null: p.VoidedBy.FirstName + " " + p.VoidedBy.LastName,
                VoidedOnDate = p.VoidedOn,
                Applies = p.Applies.Select(a => new PaymentApplyReportData
                {
                    InvoiceNumber = a.Invoice.DocumentNumber,
                    SubscriptionNumber = a.Invoice.SubscriptionId == null
                        ? null
                        : a.Invoice.Subscription.SubscriptionNumber,
                    Amount = a.ApplyAmount
                }).ToList(),
                Customer = new ReportCustomerData
                {
                    Id = p.CustomerId,
                    Name = p.Customer.Name,
                    Document = p.Customer.DocumentNumber,
                    DocumentType = p.Customer.DocumentType.Name
                }
            })
            .FirstOrDefaultAsync();

    public Task<PaymentSummaryReportData> GetPaymentSummaryReportData(DateTime start, DateTime end, string? cashier)
    {
        var query = PaymentApplies.Where(p => p.ApplyDate >= start.Date && p.ApplyDate <= end.Date)
            .Where(p => string.IsNullOrWhiteSpace(cashier) || p.Payment.PointOfSale == cashier);
        return Task.FromResult(new PaymentSummaryReportData
        {
            Date = DateTime.Now,
            StartDate = start,
            EndDate = end,
            Total = query
                .Where(p => p.Payment.StatusId != (int) Core.Enums.PaymentStatuses.Anulado &&
                            p.Payment.StatusId != (int) Core.Enums.PaymentStatuses.Pendiente).Sum(p => p.ApplyAmount),
            PaymentMethods = query.Where(p =>
                    p.Payment.StatusId != (int) Core.Enums.PaymentStatuses.Anulado &&
                    p.Payment.StatusId != (int) Core.Enums.PaymentStatuses.Pendiente)
                .GroupBy(p => p.Payment.PaymentMethodId)
                .Select(g => new PaymentSummaryReportMethodData
                {
                    MethodDescription = g.Key == null
                        ? "Efectivo"
                        : g.First()
                            .Payment.PaymentMethod.Name,
                    Quantity = g.Count(),
                    Amount = g.Sum(p => p.ApplyAmount),
                }).ToArray(),
            Payments = query.Where(p =>
                p.Payment.StatusId != (int) Core.Enums.PaymentStatuses.Anulado &&
                p.Payment.StatusId != (int) Core.Enums.PaymentStatuses.Pendiente).Select(p =>
                new PaymentSummaryReportPaymentData
                {
                    Number = p.Payment.DocumentNumber,
                    ApprovalNumber = p.Payment.ApprovalNumber ?? "",
                    Date = p.ApplyDate,
                    Invoice = p.Invoice.DocumentNumber,
                    Customer = p.Payment.Customer.Name,
                    MethodDescription = p.Payment.PaymentMethodId == null
                        ? "Efectivo"
                        : p.Payment.PaymentMethod.Name,
                    Amount = p.ApplyAmount,
                    PointOfSale = p.Payment.PointOfSale
                }).ToArray(),
            Annulled = query.Where(p => p.Payment.StatusId == (int) Core.Enums.PaymentStatuses.Anulado).Select(p =>
                new PaymentSummaryReportPaymentData
                {
                    Number = p.Payment.DocumentNumber,
                    ApprovalNumber = p.Payment.ApprovalNumber ?? "",
                    Date = p.ApplyDate,
                    Invoice = p.Invoice.DocumentNumber,
                    Customer = p.Payment.Customer.Name,
                    MethodDescription = p.Payment.PaymentMethodId == null
                        ? "Efectivo"
                        : p.Payment.PaymentMethod.Name,
                    Amount = p.ApplyAmount,
                    PointOfSale = p.Payment.PointOfSale
                }).ToArray()
        });
    }

    public Task<PaymentERPReportData> GetPaymentERPReportData(DateTime start, DateTime end)
    {
        var query = Payments.Where(p => p.DocumentDate >= start.Date && p.DocumentDate <= end.Date);
        var queryPayments = query.Where(p => p.StatusId != (int) Core.Enums.PaymentStatuses.Anulado);
        var queryPaymentApplies = queryPayments.Where(x => x.TypeId == (int)Core.Enums.PaymentTypes.Normal).SelectMany(p => p.Applies)
            .Include(x => x.Invoice)
            .ThenInclude(x => x.Lines)
            .ThenInclude(x => x.Product)
            .Include(x => x.Payment)
            .ThenInclude(x => x.Customer)
            .Include(x => x.Payment)
            .ThenInclude(x => x.PaymentMethod)
            .ToList();
        var queryPaymentAdvanced = queryPayments.Where(x => x.TypeId == (int) Core.Enums.PaymentTypes.Advanced)
            .Include(x => x.Subscription)
            .ThenInclude(x => x.Lines)
            .ThenInclude(x => x.Product)
            .Include(x => x.Customer)
            .Include(x => x.PaymentMethod)
            .ToList();
        var queryAnnulled = query.Where(p => p.StatusId == (int) Core.Enums.PaymentStatuses.Anulado);
        var erpCodes = ERPCodeDescriptions.ToList();

        var queryERPData = queryPaymentApplies.Where(p => p.Invoice.Lines.Any())
            .SelectMany(p => p.Invoice.Lines
                .Where(i => i.Product.ERPCode != null)
                .Select(i => new PaymentERPSummaryReportData
                {
                    ERPCode = i.Product.ERPCode.Trim(),
                    Applies = new List<PaymentERPApplyReportData>
                    {
                        new()
                        {
                            Number = p.Payment.DocumentNumber,
                            ExternalId = p.Payment.ExternalId ?? "",
                            DateValue = p.Payment.DocumentDate,
                            Name = p.Payment.Customer.Name,
                            PaymentMethod = p.Payment.PaymentMethod.Name,
                            AmountValue = p.ApplyAmount / (p.Invoice.DocumentAmount + Math.Abs(p.Invoice.Lines
                                .Where(x => x.Product.ERPCode == null)
                                .Sum(x => p.ApplyAmount / p.Invoice.DocumentAmount * x.TotalAmount))) * i.TotalAmount,
                            PointOfSale = p.Payment.PointOfSale ?? ""
                        }
                    }
                })).ToList();
        var queryERPAdvancedData = queryPaymentAdvanced.Where(p => p.Subscription.Lines.Any())
            .SelectMany(p => p.Subscription.Lines
                .Where(i => i.Product.ERPCode != null)
                .Select(i => new PaymentERPSummaryReportData
                {
                    ERPCode = i.Product.ERPCode.Trim(),
                    Applies = new List<PaymentERPApplyReportData>
                    {
                        new()
                        {
                            Number = p.DocumentNumber,
                            ExternalId = p.ExternalId ?? "",
                            DateValue = p.DocumentDate,
                            Name = p.Customer.Name,
                            PaymentMethodId = p.PaymentMethodId,
                            PaymentMethod = p.PaymentMethod.Name,
                            AmountValue = p.DocumentAmount /
                                          (p.Subscription.Lines.Sum(x => x.Quantity * x.Product.UnitPrice)
                                           + Math.Abs(p.Subscription.Lines.Where(x => x.Product.ERPCode == null)
                                               .Sum(x => p.DocumentAmount /
                                                         p.Subscription.Lines.Sum(x =>
                                                             x.Quantity * x.Product.UnitPrice) *
                                                         x.Quantity * x.Product.UnitPrice)))
                                          * i.Quantity * i.Product.UnitPrice,
                            PointOfSale = p.PointOfSale ?? ""
                        }
                    }
                })).ToList();
        queryERPData.AddRange(queryERPAdvancedData);
        queryERPData = queryERPData
            .GroupBy(
                m => m.ERPCode,
                (c, a) => new PaymentERPSummaryReportData
                {
                    ERPCode = c + " - " + (erpCodes.Where(e => int.TryParse(c ?? "", out var code) && e.Code == code).Select(e => e.Description).FirstOrDefault() ?? "N/A"),
                    Quantity = a.Count(),
                    TotalValue = a.Sum(x => x.Applies.Sum(y => y.AmountValue)),
                    Applies = a.SelectMany(x => x.Applies).ToList()
                })
            .ToList();
        return Task.FromResult(new PaymentERPReportData
        {
            Date = DateTime.Now,
            StartDate = start,
            EndDate = end,
            TotalValue = queryERPData.Sum(x => x.TotalValue),
            Quantity = queryERPData.Sum(x => x.Quantity),
            PaymentMethods = queryERPData.SelectMany(x => x.Applies).GroupBy(
                x => x.PaymentMethodId,
                (c, a) => new PaymentSummaryReportMethodData
                {
                    MethodDescription = a.FirstOrDefault()?.PaymentMethod ?? "",
                    Quantity = a.Count(),
                    Amount = a.Sum(x => x.AmountValue)
                }).ToList(),
            ERPs = queryERPData
        });
    }
    
    public async Task<List<NonAccountingReportLinesData>> GetProductReportData(DateTime from, DateTime to)
    {
        var products = Products;
        var subscriptionLines = SubscriptionLines
            .Include(s => s.Subscription)
            .Include(x => x.Product)
            .ThenInclude(p => p.Rates)
            .ThenInclude(r => r.RateScales);
        var subscriptions = Subscriptions
            .Include(s => s.Customer)
            .Include(s => s.Lines)
            .ThenInclude(sl => sl.Product)
            .ThenInclude(p => p.Rates)
            .ThenInclude(r => r.RateScales);

        var lines = new List<NonAccountingReportLinesData>();
        var tasks = new List<Task>();

        var contractTariffConsumptions = subscriptionLines
            .Where(p => p.Product.Rates.Any() &&
                        p.Product.Rates.Where(r => r.StartDate >= DateTime.Now && r.EndDate <= DateTime.Now).Any() &&
                        p.Product.Rates.Where(r => r.StartDate >= DateTime.Now && r.EndDate <= DateTime.Now).First()
                            .RateScales.Any())
            .SelectMany(p =>
                p.Product.Rates.Where(r => r.StartDate >= DateTime.Now && r.EndDate <= DateTime.Now)
                    .SelectMany(r => r.RateScales.Select(s => new {p.Product, RateScale = s})))
            .GroupBy(ps => new {ps.Product, ps.RateScale})
            .Select((g, i) => new NonAccountingReportLinesData
            {
                Number = i.ToString().PadLeft(2, '0'),
                Concept = $"{g.Key.Product.Barcode} (" +
                          g.Key.RateScale.LowerQuantity +
                          (g.Key.RateScale.UpperQuantity.HasValue
                              ? " - " + g.Key.RateScale.UpperQuantity.Value
                              : " o mayor ") +
                          $"{g.Key.Product.Rates.Where(r => r.StartDate >= DateTime.Now && r.EndDate <= DateTime.Now).First().UnitOfMeasure})",
                Value = g.Count().ToString(),
            });

        var contractsTariffs = subscriptionLines
            .Where(p => p.Product.Rates.Any() && p.Product.Rates
                .Where(r => r.StartDate >= DateTime.Now && r.EndDate <= DateTime.Now).First().ExtraCharges.Any())
            .SelectMany(p => p.Product.Rates.Select(r => new {Product = p.Product, Rate = r}))
            .GroupBy(pr => pr.Rate)
            .Select((g, i) => new NonAccountingReportLinesData
            {
                Number = i.ToString().PadLeft(2, '0'),
                Concept = $"Clientes {g.Key.Name}",
                Value = g.Count().ToString(),
            });

        var contractCustomerTypes = subscriptions
            .GroupBy(s => s.Customer.CustomerType)
            .Select((g, i) => new NonAccountingReportLinesData
            {
                Number = i.ToString().PadLeft(2, '0'),
                Concept = $"Clientes {g.Key.Name}",
                Value = g.Count().ToString(),
            });

        var contractMeasurementTechnology = subscriptions
            .Where(s => s.ConnectionTypeId != null)
            .GroupBy(s => s.ConnectionType)
            .Select((g, i) => new NonAccountingReportLinesData
            {
                Number = i.ToString().PadLeft(2, '0'),
                Concept = $"Clientes {g.Key.Name}",
                Value = g.Count().ToString(),
            });

        var contractConsumptionRanges = subscriptionLines
            .Where(s => s.Product.Rates.Any() && s.Product.Rates.Any(r => r.RateScales.Any()))
            .SelectMany(s => s.Product.Rates.SelectMany(r => r.RateScales.Select(rs => new {RateScale = rs})))
            .GroupBy(s => new {s.RateScale.UpperQuantity, s.RateScale.LowerQuantity})
            .Select((g, i) => new NonAccountingReportLinesData
            {
                Number = i.ToString().PadLeft(2, '0'),
                Concept = $"Rango " + g.Key.LowerQuantity + (g.Key.UpperQuantity.HasValue
                    ? " - " + g.Key.UpperQuantity.Value
                    : " o mayor "),
                Value = g.Count().ToString(),
            });
        // var contractInvoicedTariffs = subscriptions
        //  .Where(s => s.Invoices.Any(s =>))
        return lines;
    }
    
    public Task<LotCloseReportData> GetLotReportData(Guid lotId)
    {
        var paymentSequence = Lots
            .Include(l => l.Payments)
            .First(l => l.Id == lotId)
            .Payments
            .OrderBy(p => p.CreatedOn)
            .Select((p, i) => new
            {
                p.Id,
                Index = i
            })
            .ToList()
            .ToDictionary(p => p.Id, p => p.Index);

        return Lots
            .Where(l => l.Id == lotId)
            .Include(x => x.User)
            .Include(x => x.Payments)
            .ThenInclude(x => x.PaymentMethod)
            .Include(x => x.Payments)
            .ThenInclude(x => x.Currency)
            .Include(x => x.Payments)
            .ThenInclude(x => x.Status)
            .Include(x => x.Payments)
            .ThenInclude(x => x.Customer)
            .Select(l => new LotCloseReportData
            {
                LotId = l.Id,
                ServiceAgentName = l.User.FirstName + " " + l.User.LastName,
                StartDateTime = l.DateCreated,
                EndDateTime = l.DateClosed,
                Total = l.Payments.Any(x =>
                    x.StatusId == (int) Core.Enums.PaymentStatuses.PagoAFactura ||
                    x.StatusId == (int) Core.Enums.PaymentStatuses.PagoASuscripcion)
                    ? l.Payments
                        .Where(x => x.StatusId == (int) Core.Enums.PaymentStatuses.PagoAFactura ||
                                    x.StatusId == (int) Core.Enums.PaymentStatuses.PagoASuscripcion)
                        .Sum(b => b.DocumentAmount)
                    : 0,
                PaymentMethods = l.Payments.Where(x =>
                        x.StatusId == (int) Core.Enums.PaymentStatuses.PagoAFactura ||
                        x.StatusId == (int) Core.Enums.PaymentStatuses.PagoASuscripcion)
                    .GroupBy(x => x.PaymentMethodId)
                    .Select(x => new LotCloseReportPaymentMethodData
                    {
                        PaymentMethod = x.First().PaymentMethod.Name,
                        Amount = x.Sum(y => y.DocumentAmount),
                        Currency = x.First().Currency.Name,
                    }).ToList(),
                Transactions = l.Payments.Where(x =>
                        x.StatusId == (int) Core.Enums.PaymentStatuses.PagoAFactura ||
                        x.StatusId == (int) Core.Enums.PaymentStatuses.PagoASuscripcion)
                    .GroupBy(x => x.PaymentMethodId)
                    .Select(x => new LotCloseReportTransactionData
                    {
                        PaymentMethod = x.First().PaymentMethod.Name,
                        Details = x.Select((p, i) => new LotCloseReportTransactionDetailData
                        {
                            Sequence = paymentSequence[p.Id] + 1,
                            Number = p.DocumentNumber,
                            DateT = p.DocumentDate,
                            Amount = p.DocumentAmount,
                            Document = p.Customer.DocumentNumber
                        }).ToList()
                    }).ToList(),
                Annulled = l.Payments.Where(x =>
                        x.StatusId == (int) Core.Enums.PaymentStatuses.Anulado)
                    .GroupBy(x => x.PaymentMethodId)
                    .Select(x => new LotCloseReportTransactionData
                    {
                        PaymentMethod = x.First().PaymentMethod.Name,
                        Details = x.Select((p, i) => new LotCloseReportTransactionDetailData
                        {
                            Sequence = paymentSequence[p.Id] + 1,
                            Number = p.DocumentNumber,
                            DateT = p.DocumentDate,
                            Amount = p.DocumentAmount,
                            Document = p.Customer.DocumentNumber
                        }).ToList()
                    }).ToList(),
            })
            .FirstAsync();
    }

    public Task<List<WorkOrderSummaryReportLinesData>> GetWorkOrderReportData(DateTime from, DateTime to)
    {
        throw new NotImplementedException();
    }
    
    public Task<int[]> GetInvoiceReportIds(int? billingCycleId, int? routeId, DateTime start) =>
        Invoices
            .Where(x => billingCycleId == null || (x.SubscriptionId != null && x.Subscription.BillingCycleId == billingCycleId))
            .Where(x => routeId == null || (x.ShippingAddressId != null && x.ShippingAddress.RouteId == routeId))
            .Where(x => x.StatusId == (int)Core.Enums.InvoiceStatuses.Facturado)
            .Where(x => x.DocumentDate.Date == start.Date)
            .Select(x => x.Id)
            .ToArrayAsync();

    public Task<MemoReportData> GetMemoReportData(int id, ReceivableReasonTypes type) =>
        type == ReceivableReasonTypes.CreditMemo
            ? CreditMemos
                .Where(cm => cm.Id == id)
                .Select(cm => new MemoReportData
                {
                    Number = cm.DocumentNumber,
                    NoteType = "Nota de Crédito",
                    SubscriptionNumber = cm.Applies.Where(a => a.Invoice.Subscription != null).Select(a => a.Invoice.Subscription.SubscriptionNumber).ToList(),
                    NCFNumber = cm.NCFNumber,
                    InvoiceNCF = cm.Applies.Select(a => a.Invoice.NCFNumber).ToList(),
                    Reason = cm.ReceivableReason.Name,
                    Observation = cm.Observation,
                    CompanyName = cm.Applies.Where(a => a.Invoice.ShippingAddress != null).Select(a => a.Invoice.ShippingAddress.CompanyName).FirstOrDefault(),
                    FullShippingAddress = cm.Applies.Where(a => a.Invoice.ShippingAddress != null).Select(a =>
                            string.IsNullOrWhiteSpace(a.Invoice.ShippingAddress.AddressLine1)
                                ? a.Invoice.ShippingAddress.PlainAddress
                                : $"{a.Invoice.ShippingAddress.AddressLine1}{(string.IsNullOrWhiteSpace(a.Invoice.ShippingAddress.AddressLine2) ? "" : ", " + a.Invoice.ShippingAddress.AddressLine2)}{(string.IsNullOrWhiteSpace(a.Invoice.ShippingAddress.AddressLine3) ? "" : ", " + a.Invoice.ShippingAddress.AddressLine3)}{(string.IsNullOrWhiteSpace(a.Invoice.ShippingAddress.City.Name) ? "" : ", " + a.Invoice.ShippingAddress.City.Name)}")
                        .FirstOrDefault(),
                    Phone = cm.Applies.Where(a => a.Invoice.ShippingAddress != null).Select(a => a.Invoice.ShippingAddress.Phone).FirstOrDefault(),
                    Lines = cm.Applies.Select(a => new MemoReportLineData
                    {
                        Number = a.Invoice.DocumentNumber,
                        Value = a.ApplyAmount,
                        Total = cm.DocumentAmount
                    }).ToList(),
                    Customer = new ReportCustomerData
                    {
                        Id = cm.CustomerId,
                        Name = cm.Customer.Name,
                        Document = cm.Customer.DocumentNumber,
                        DocumentType = cm.Customer.DocumentType.Name,
                    }
                }).FirstOrDefaultAsync()
            : DebitMemos
                .Where(dm => dm.Id == id)
                .Select(dm => new MemoReportData
                {
                    Number = dm.DocumentNumber,
                    NoteType = "Nota de Débito",
                    SubscriptionNumber = dm.Applies.Where(a => a.Invoice.Subscription != null).Select(a => a.Invoice.Subscription.SubscriptionNumber).ToList(),
                    NCFNumber = dm.NCFNumber,
                    InvoiceNCF = dm.Applies.Select(a => a.Invoice.NCFNumber).ToList(),
                    Reason = dm.ReceivableReason.Name,
                    Observation = dm.Observation,
                    CompanyName = dm.Applies.Where(a => a.Invoice.ShippingAddress != null).Select(a => a.Invoice.ShippingAddress.CompanyName).FirstOrDefault(),
                    FullShippingAddress = dm.Applies.Where(a => a.Invoice.ShippingAddress != null).Select(a =>
                            string.IsNullOrWhiteSpace(a.Invoice.ShippingAddress.AddressLine1)
                                ? a.Invoice.ShippingAddress.PlainAddress
                                : $"{a.Invoice.ShippingAddress.AddressLine1}{(string.IsNullOrWhiteSpace(a.Invoice.ShippingAddress.AddressLine2) ? "" : ", " + a.Invoice.ShippingAddress.AddressLine2)}{(string.IsNullOrWhiteSpace(a.Invoice.ShippingAddress.AddressLine3) ? "" : ", " + a.Invoice.ShippingAddress.AddressLine3)}{(string.IsNullOrWhiteSpace(a.Invoice.ShippingAddress.City.Name) ? "" : ", " + a.Invoice.ShippingAddress.City.Name)}")
                        .FirstOrDefault(),
                    Phone = dm.Applies.Where(a => a.Invoice.ShippingAddress != null).Select(a => a.Invoice.ShippingAddress.Phone).FirstOrDefault(),
                    Lines = dm.Applies.Select(a => new MemoReportLineData
                    {
                        Number = a.Invoice.DocumentNumber,
                        Value = a.ApplyAmount,
                        Total = dm.DocumentAmount
                    }).ToList() ,
                    Customer = new ReportCustomerData
                    {
                        Id = dm.CustomerId,
                        Name = dm.Customer.Name,
                        Document = dm.Customer.DocumentNumber,
                        DocumentType = dm.Customer.DocumentType.Name,
                    }
                }).FirstOrDefaultAsync();

    #endregion
}