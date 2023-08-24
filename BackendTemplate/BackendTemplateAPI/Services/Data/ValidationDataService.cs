namespace BackendTemplateAPI.Services.Data;

public partial class DataService
{
    public Task<bool> ExistsUserWithEmail(string email) =>
        Users.AnyAsync(u => u.Email.ToLower() == email.ToLower().Trim());

    public Task<bool> ExistsBankWithName(string name) =>
        Banks.AnyAsync(u => u.Name!.ToLower() == name.ToLower());

    public Task<bool> ExistsCityWithName(string name, int stateId) =>
        Cities.AnyAsync(u => u.Name!.ToLower() == name.ToLower() && u.StateId == stateId);

    public Task<bool> ExistsStateWithName(string name, int countryId) =>
        States.AnyAsync(u => u.Name!.ToLower() == name.ToLower() && u.CountryId == countryId);

    public Task<bool> ExistsCustomerTypeWithName(string name) =>
        CustomerTypes.AnyAsync(u => u.Name!.ToLower() == name.ToLower());

    public Task<bool> ExistsClaimTypeWithName(string name) =>
        ClaimTypes.AnyAsync(u => u.Name!.ToLower() == name.ToLower());

    public Task<bool> ExistsClaimMotiveWithName(string name) =>
        ClaimMotives.AnyAsync(u => u.Name!.ToLower() == name.ToLower());

    public Task<bool> ExistsLegalInstanceWithName(string name) =>
        LegalInstances.AnyAsync(u => u.Name!.ToLower() == name.ToLower());

    public Task<bool> ExistsPaymentTermWithName(string name) =>
        PaymentTerms.AnyAsync(u => u.Name!.ToLower() == name.ToLower());

    public Task<bool> ExistsPaymentMethodWithName(string name) =>
        PaymentMethods.AnyAsync(u => u.Name!.ToLower() == name.ToLower());

    public Task<bool> ExistsCustomerTicketTypeWithName(string name) =>
        CustomerTicketTypes.AnyAsync(u => u.Name!.ToLower() == name.ToLower());

    public Task<bool> ExistsReceivableReasonWithName(string name, int type) =>
        ReceivableReasons.AnyAsync(u => u.Name!.ToLower() == name.ToLower() && u.DocumentType == type);

    public Task<bool> ExistsPropertyTypeWithName(string name) =>
        PropertyTypes.AnyAsync(u => u.Name!.ToLower() == name.ToLower());

    public Task<bool> ExistsTaxScheduleWithName(string name) =>
        TaxSchedules.AnyAsync(u => u.Name!.ToLower() == name.ToLower());

    public Task<bool> ExistsNCFSequenceWithNcfType(int NCFType) =>
        NCFSequenceSettings.AnyAsync(u => u.NCFTypeId == NCFType);

    public Task<bool> ExistsBranchWithCode(string Code) =>
        Branches.AnyAsync(u => u.Code!.ToLower() == Code.ToLower());

    public Task<bool> ExistsCurrencyWithCode(string Code, string Symbol) =>
        Currencies.AnyAsync(u => u.Code!.ToLower() == Code.ToLower() || u.Symbol.ToLower() == Symbol.ToLower());

    public Task<bool> ExistsElectricalEquipmentWithCode(string code) =>
        ElectricalEquipments.AnyAsync(u => u.Code!.ToLower() == code.ToLower());

    public Task<bool> ExistsRoleWithName(string name) =>
        Roles.AnyAsync(u => u.Name!.ToLower() == name.ToLower());

    public Task<bool> ExistsCustomerWithData(CustomerData data, int Id = 0) =>
        Customers.AnyAsync(c =>
            c.DocumentNumber == data.Document && c.DocumentTypeId == data.DocumentTypeId && (Id == 0 || c.Id != Id));

    public Task<bool> ExistsProductWithNameOrBarcode(ProductData data, int? Id) =>
        Products.AnyAsync(p =>
            (p.Name!.ToLower() == data.Name.ToLower().Trim() ||
             p.Barcode!.ToLower() == data.Barcode.ToLower().Trim()) && p.Id != Id);

    public Task<bool> ExistsProductRateWithNameAndId(ProductRateData data) =>
        ProductRates.AnyAsync(p => p.Name!.ToLower() == data.Name.ToLower().Trim() && p.ProductId != data.ProductId);

    public Task<bool> AddressIsAlreadyAssociated(int ShippingAddressId, int? SubscriptionId = null) =>
        Subscriptions.AnyAsync(s => s.ShippingAddressId == ShippingAddressId && s.Id != SubscriptionId);

    public Task<bool> AddressIsAssociatedWithCustomer(int addressId, int customerId) =>
        Customers.Where(s => s.Addresses.Any(a => a.CustomerAddressId == addressId) && s.Id == customerId).AnyAsync();

    public Task<bool> ExistsRouteWithName(string name) =>
        Routes.AnyAsync(u => u.Name.ToLower() == name.ToLower().Trim());

    public Task<bool> ExistsRouteTypeWithName(string name) =>
        RouteTypes.AnyAsync(u => u.Name.ToLower() == name.ToLower().Trim());

    public Task<bool> ExistsUserWithPhone(string phone) =>
        Users.AnyAsync(u => u.Phone == phone.Trim() && !string.IsNullOrWhiteSpace(u.Phone));

    public Task<bool> ExistsVoltageWithName(string name) =>
        Voltages.AnyAsync(v => v.Name.ToLower() == name.ToLower().Trim());

    public Task<bool> ExistsConnectionTypeWithName(string name) =>
        ConnectionTypes.AnyAsync(v => v.Name.ToLower() == name.ToLower().Trim());

    public Task<bool> ExistsBillingTypeWithName(string name) =>
        BillingTypes.AnyAsync(v => v.Name.ToLower() == name.ToLower().Trim());

    public Task<bool> ExistsChargeInSubscription(ChargeData data) =>
        Charges.Where(c => c.SubscriptionId == data.SubscriptionId && c.ProductId == data.ProductId).AnyAsync();

    public Task<bool> ExistsBillingScheduleWithName(string name) =>
        BillingSchedules.AnyAsync(bs => bs.Name.ToLower() == name.ToLower().Trim());

    public Task<bool> ExistsBillingCycleWithName(string name) =>
        BillingCycles.AnyAsync(bc => bc.Name.ToLower() == name.ToLower().Trim());

    public Task<bool> ExistsWorkOrderTypeWithName(string name) =>
        WorkOrderTypes.AnyAsync(x => x.Description.ToLower() == name.ToLower().Trim());

    public Task<bool> ExistsReadingType(int dataReadingTypeId) =>
        ReadingTypes.AnyAsync(x => x.Id == dataReadingTypeId);
    
    public Task<bool> ExistsMeter(string meterNumber, int id) =>
        Meters.AnyAsync(m => m.Id != id && m.MeterNumber.ToLower().Trim() == meterNumber.ToLower().Trim());

    public Task<bool> PeriodsGenerated(int id, int year) =>
        BillingPeriods.Where(p => p.BillingScheduleId == id && p.Name.Contains($"{year}")).AnyAsync();
    
    public Task<bool> ExistsMeterModel(string brand, string model, int id) =>
        MeterModels.AnyAsync(mm =>
                mm.Id != id && mm.Brand.ToLower().Trim() == brand.ToLower().Trim() &&
                mm.Model.ToLower().Trim() == model.ToLower().Trim());
    public Task IsLotPossible(Guid userId)
    {
        var lots = Lots.Where(l => l.UserId == userId);

        var lot = lots.OrderByDescending(l => l.DateCreated).First();
        switch (lot.DateClosed.HasValue)
        {
            case false when lot.DateCreated == DateTime.Today:
                throw new AlreadyExists("Ya existe un lote abierto para hoy");
            case false when lot.DateCreated != DateTime.Today:
                throw new InvalidParameter("No se puede crear un nuevo lote porque hay uno abierto");
            case true when lot.DateCreated == DateTime.Today:
                throw new AlreadyExists("No se puede crear un nuevo lote porque hay uno cerrado para hoy");
        }

        return Task.CompletedTask;
    }
}