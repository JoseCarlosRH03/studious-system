using BackendTemplateCore.DTOs.Shared;
using BackendTemplateCore.Errors;
using BackendTemplateCore.Models;
using BackendTemplateCore.Models.Billings;
using BackendTemplateCore.Models.Claims;
using BackendTemplateCore.Models.Customers;
using BackendTemplateCore.Models.Invoices;
using BackendTemplateCore.Models.Payments;
using BackendTemplateCore.Models.Products;
using BackendTemplateCore.Models.Subscriptions;
using BackendTemplateCore.Services;
using BackendTemplateCore.Services.Infrastructure;
using BackendTemplateCore.Services.Model_Related_Services;

namespace BackendTemplateCore;

//Naming conventions for regions: Plural names
public partial class Logic
{
    public Logic(IAuditService audit, IDataService data, IAuthenticationService authentication, 
        IEmailService emails, IResourceService resources, IRNCService rncService, ICedulaService cedulaService,
        IReadoutService readoutService, IBackgroundBiller backgroundBiller, IMunicipiaService municipiaService, ISicflexService sicflexService)
    {
        Audit = audit;
        Data = data;
        Authentication = authentication;
        Email = emails;
        Resources = resources;
        RNC = rncService;
        Cedula = cedulaService;
        Readout = readoutService;
        Biller = backgroundBiller;
        Municipia = municipiaService;
        Sicflex = sicflexService;
    }

    readonly IAuditService Audit;
    readonly IDataService Data;
    readonly IAuthenticationService Authentication;
    readonly IEmailService Email;
    readonly IResourceService Resources;
    readonly IRNCService RNC;
    readonly ICedulaService Cedula;
    readonly IReadoutService Readout;
    readonly IBackgroundBiller Biller;
    readonly IMunicipiaService Municipia;
    readonly ISicflexService Sicflex;

    #region Banks

    public async Task<int> CreateBank(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacío");
        if (await Data.ExistsBankWithName(Name))
            throw new AlreadyExists("Ya existe un banco con este nombre");
        var result = await Data.Add(new Bank {Name = Name.Trim(), Status = (int)GenericStatus.Activo});
        return result.Id;
    }

    public async Task<int> DeleteBank(int Id)
    {
        var bank = await Data.GetByIdAsync<Bank>(Id);
        if (bank == null)
            throw new NotFound("No existe este banco");
        await Data.Delete(bank);
        return bank.Id;
    }

    public async Task<int> UpdateBank(int Id, string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacío");
        if (await Data.ExistsBankWithName(Name))
            throw new AlreadyExists("Ya existe un banco con este nombre");
        var bank = await Data.GetByIdAsync<Bank>(Id);
        if (bank == null)
            throw new NotFound("No existe este banco");
        bank.Name = Name.Trim();
        await Data.Update(bank);
        return bank.Id;
    }
    
    public async Task<int> InactivateBank(int Id)
    {
        var bank = await Data.GetByIdAsync<Bank>(Id);
        if (bank == null)
            throw new NotFound("No existe este banco");
        bank.Status = (int)GenericStatus.Inactivo;
        await Data.Update(bank);
        return bank.Id;
    }

    public async Task<Item> GetBank(int Id)
    {
        var bank = await Data.GetByIdAsync<Bank>(Id);
        if (bank == null)
            throw new NotFound("No existe este banco");
        return new Item(bank.Id, bank.Name);
    }

    public async Task<List<StatusItemView>> GetBanks(int? status, string? filter = "") =>
        (await Data.GetAll<Bank>(string.IsNullOrWhiteSpace(filter)
            ? null
            : x => x.Name.ToLower().Contains(filter.ToLower().Trim())))?.Select(b => new StatusItemView(b.Id, b.Name, Item.From((GenericStatus)b.Status)))
        .ToList() ?? throw new NotFound("No se encontro ningun banco registrado");

    #endregion

    #region Cities

    public async Task<int> CreateCity(CityData data)
    {
        Validation.ValidateCityData(data);
        if (await Data.ExistsCityWithName(data.Name, data.StateId))
            throw new AlreadyExists("Ya existe una ciudad con este nombre en esta provincia/estado");
        var result = await Data.Add(new City {Name = data.Name.Trim(), StateId = data.StateId, Status = (int)GenericStatus.Activo});
        return result.Id;
    }

    public async Task<int> InactivateCity(int Id)
    {
        var city = await Data.GetByIdAsync<City>(Id);
        if (city == null)
            throw new NotFound("No existe esta ciudad");
        city.Status = (int) GenericStatus.Inactivo;
        await Data.Update(city);
        return city.Id;
    }

    public async Task<int> UpdateCity(int Id, CityData data)
    {
        Validation.ValidateCityData(data);
        if (await Data.ExistsCityWithName(data.Name, data.StateId))
            throw new AlreadyExists("Ya existe una ciudad con este nombre en esta provincia/estado");
        var city = await Data.GetByIdAsync<City>(Id);
        if (city == null)
            throw new NotFound("No existe esta ciudad");
        city.Name = data.Name.Trim();
        city.StateId = data.StateId;
        await Data.Update(city);
        return city.Id;
    }

    public async Task<CityView> GetCity(int Id)
    {
        var city = await Data.GetAsync<City>(x => x.Id == Id, x => x.State);
        if (city == null)
            throw new NotFound("No existe esta ciudad");
        return CityView.From(city);
    }

    public async Task<List<CityView>> GetCities(int start = 0, int count = 20, string? filter = "") =>
        await Data.GetCities(start, count, filter);

    public async Task<int> GetCityCount(string? filter = "") =>
        await Data.Count<City>(string.IsNullOrWhiteSpace(filter)
            ? null
            : x => x.Name.ToLower().Contains(filter.ToLower().Trim()));

    #endregion

     #region Provinces

    public async Task<int> CreateState(StateData data)
    {
        Validation.ValidateStateData(data);
        if (await Data.ExistsStateWithName(data.Name, data.CountryId))
            throw new AlreadyExists("Ya existe una provincia/estado con este nombre en este pais");
        var result = await Data.Add(new State {Name = data.Name.Trim(), CountryId = data.CountryId, Status = (int)GenericStatus.Activo});
        return result.Id;
    }

    public async Task<int> InactivateState(int Id)
    {
        var state = await Data.GetByIdAsync<State>(Id);
        if (state == null)
            throw new NotFound("No existe esta provincia/estado");
        state.Status = (int) GenericStatus.Inactivo;
        await Data.Update(state);
        return state.Id;
    }

    public async Task<int> UpdateState(int Id, StateData data)
    {
        Validation.ValidateStateData(data);
        if (await Data.ExistsStateWithName(data.Name, data.CountryId))
            throw new AlreadyExists("Ya existe una provincia/estado con este nombre en este pais");
        var state = await Data.GetByIdAsync<State>(Id);
        if (state == null)
            throw new NotFound("No existe esta provincia/estado");
        state.Name = data.Name.Trim();
        state.CountryId = data.CountryId;
        await Data.Update(state);
        return state.Id;
    }

    public async Task<StateView> GetState(int Id)
    {
        var state = await Data.GetAsync<State>(x => x.Id == Id, x => x.Country, x => x.Cities);
        if (state == null)
            throw new NotFound("No existe esta provincia/estado");
        return StateView.From(state);
    }

    public async Task<List<StateView>> GetStates(string? filter = "") =>
        await Data.GetStates(filter);

    public async Task<int> GetStateCount(string? filter = "") =>
        await Data.Count<State>(string.IsNullOrWhiteSpace(filter)
            ? null
            : s => s.Name.ToLower().Contains(filter.ToLower().Trim()));

    #endregion
    
    #region Countries

    public async Task<List<StatusItemView>> GetCountries(string? filter = "") =>
        await Data.GetCountries(filter);

    public async Task<int> GetCountryCount(string? filter = "") =>
        await Data.Count<Country>(string.IsNullOrWhiteSpace(filter)
            ? null
            : x => x.Name.ToLower().Contains(filter.ToLower().Trim()));

    #endregion

    #region Customer Type

    public async Task<int> CreateCustomerType(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsCustomerTypeWithName(Name))
            throw new AlreadyExists("Ya existe este tipo de cliente");
        var result = await Data.Add(new CustomerType {Name = Name.Trim(), Status = (int) GenericStatus.Activo});
        return result.Id;
    }

    public async Task<int> UpdateCustomerType(int Id, string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsCustomerTypeWithName(Name))
            throw new AlreadyExists("Ya existe este tipo de cliente");
        var customerType = await Data.GetByIdAsync<CustomerType>(Id);
        if (customerType == null)
            throw new NotFound("No existe este tipo de cliente");
        customerType.Name = Name.Trim();
        await Data.Update(customerType);
        return customerType.Id;
    }

    public async Task<int> UpdateCustomerTypeStatus(int Id, int Status)
    {
        var customerType = await Data.GetByIdAsync<CustomerType>(Id);
        if (customerType == null)
            throw new NotFound("No existe este tipo de cliente");
        customerType.Status = Status;
        await Data.Update(customerType);
        return customerType.Id;
    }

    public async Task<CustomerTypeView> GetCustomerType(int Id)
    {
        var customerType = await Data.GetAsync<CustomerType>(x => x.Id == Id);
        if (customerType == null)
            throw new NotFound("No existe este tipo de cliente");
        return CustomerTypeView.From(customerType);
    }

    public async Task<List<StatusItemView>> GetCustomerTypes(int? status, string? filter = "") =>
        (await Data.GetAll<CustomerType>(status != null ? x => x.Status == status : null))
        ?.Where(c => string.IsNullOrWhiteSpace(filter) ? true : c.Name.ToLower().Contains(filter.ToLower().Trim()))
        .Select(x => new StatusItemView(x.Id, x.Name, new Item(x.Status, ((GenericStatus)x.Status).ToString()))).ToList() ??
        throw new NotFound("No se encontro ningun tipo de cliente registrado");

    #endregion

    #region Claim Types

    public async Task<int> CreateClaimType(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsClaimTypeWithName(Name))
            throw new AlreadyExists("Ya existe este tipo de reclamo");
        var result = await Data.Add(new ClaimType {Name = Name.Trim(), Status = (int) GenericStatus.Activo});
        return result.Id;
    }

    public async Task<int> UpdateClaimType(int Id, string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsClaimTypeWithName(Name))
            throw new AlreadyExists("Ya existe este tipo de reclamo");
        var claimType = await Data.GetByIdAsync<ClaimType>(Id);
        if (claimType == null)
            throw new NotFound("No existe este tipo de reclamo");
        claimType.Name = Name.Trim();
        await Data.Update(claimType);
        return claimType.Id;
    }

    public async Task<int> UpdateClaimTypeStatus(int Id, int Status)
    {
        var claimType = await Data.GetByIdAsync<ClaimType>(Id);
        if (claimType == null)
            throw new NotFound("No existe este tipo de reclamo");
        claimType.Status = Status;
        await Data.Update(claimType);
        return claimType.Id;
    }

    public async Task<ClaimTypeView> GetClaimType(int Id)
    {
        var claimType = await Data.GetAsync<ClaimType>(x => x.Id == Id);
        if (claimType == null)
            throw new NotFound("No existe este tipo de reclamo");
        return ClaimTypeView.From(claimType);
    }

    public async Task<List<StatusItemView>> GetClaimTypes(string? filter = "") =>
        (await Data.GetAll<ClaimType>(string.IsNullOrWhiteSpace(filter)
            ? null
            : c => c.Name.ToLower().Contains(filter.ToLower().Trim())))?.Select(s => new StatusItemView(s.Id, s.Name, Item.From((GenericStatus)s.Status))).ToList() ??
        throw new NotFound("No se encontro ningun tipo de reclamo registrado");

    #endregion

    #region Claim Motives

    public async Task<int> CreateClaimMotive(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsClaimMotiveWithName(Name))
            throw new AlreadyExists("Ya existe este motivo de reclamación");
        var result = await Data.Add(new ClaimMotive {Name = Name.Trim(), Status = (int) GenericStatus.Activo});
        return result.Id;
    }

    public async Task<int> UpdateClaimMotive(int Id, string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsClaimMotiveWithName(Name))
            throw new AlreadyExists("Ya existe este motivo de reclamación");
        var claimMotive = await Data.GetByIdAsync<ClaimMotive>(Id);
        if (claimMotive == null)
            throw new NotFound("No existe este motivo de reclamación");
        claimMotive.Name = Name.Trim();
        await Data.Update(claimMotive);
        return claimMotive.Id;
    }

    public async Task<int> UpdateClaimMotiveStatus(int Id, int Status)
    {
        var claimMotive = await Data.GetByIdAsync<ClaimMotive>(Id);
        if (claimMotive == null)
            throw new NotFound("No existe este motivo de reclamación");
        claimMotive.Status = Status;
        await Data.Update(claimMotive);
        return claimMotive.Id;
    }

    public async Task<ClaimMotiveView> GetClaimMotive(int Id)
    {
        var claimMotive = await Data.GetAsync<ClaimMotive>(x => x.Id == Id);
        if (claimMotive == null)
            throw new NotFound("No existe este motivo de reclamación");
        return ClaimMotiveView.From(claimMotive);
    }

    public async Task<List<StatusItemView>> GetClaimMotives(string? filter = "") =>
        (await Data.GetAll<ClaimMotive>(string.IsNullOrWhiteSpace(filter)
            ? null
            : c => c.Name.ToLower().Contains(filter.ToLower().Trim())))?.Select(s => new StatusItemView(s.Id, s.Name, Item.From((GenericStatus)s.Status)))
        .ToList() ?? throw new NotFound("No se encontro ningun motivo de reclamación registrado");

    #endregion

    #region Legal Instances

    public async Task<int> CreateLegalInstance(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsLegalInstanceWithName(Name))
            throw new AlreadyExists("Ya existe esta instancia legal");
        var result = await Data.Add(new LegalInstance {Name = Name.Trim(), Status = (int) GenericStatus.Activo});
        return result.Id;
    }

    public async Task<int> UpdateLegalInstance(int Id, string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsLegalInstanceWithName(Name))
            throw new AlreadyExists("Ya existe esta instancia legal");
        var legalInstance = await Data.GetByIdAsync<LegalInstance>(Id);
        if (legalInstance == null)
            throw new NotFound("No existe esta instancia legal");
        legalInstance.Name = Name.Trim();
        await Data.Update(legalInstance);
        return legalInstance.Id;
    }

    public async Task<int> UpdateLegalInstanceStatus(int Id, int Status)
    {
        var legalInstance = await Data.GetByIdAsync<LegalInstance>(Id);
        if (legalInstance == null)
            throw new NotFound("No existe esta instancia legal");
        legalInstance.Status = Status;
        await Data.Update(legalInstance);
        return legalInstance.Id;
    }

    public async Task<LegalInstanceView> GetLegalInstance(int Id)
    {
        var legalInstance = await Data.GetAsync<LegalInstance>(x => x.Id == Id);
        if (legalInstance == null)
            throw new NotFound("No existe esta instancia legal");
        return LegalInstanceView.From(legalInstance);
    }

    public async Task<List<StatusItemView>> GetLegalInstances(string? filter = "") =>
        (await Data.GetAll<LegalInstance>(string.IsNullOrWhiteSpace(filter)
            ? null
            : c => c.Name.ToLower().Contains(filter.ToLower().Trim())))?.Select(s => new StatusItemView(s.Id, s.Name, Item.From((GenericStatus)s.Status)))
        .ToList() ?? throw new NotFound("No se encontro ninguna instancia legal registrada");

    #endregion

    #region Payment Terms

    public async Task<int> CreatePaymentTerm(PaymentTermData data)
    {
        Validation.ValidatePaymentTermData(data);
        if (await Data.ExistsPaymentTermWithName(data.Name))
            throw new AlreadyExists("Ya existe este término de pago");
        var result = await Data.Add(new PaymentTerm
        {
            Name = data.Name.Trim(), 
            DueDays = data.DueDays, 
            LateFeeDays = data.LateFeeDays,
            LateFeeRate = data.LateFeeRate,
            Status = (int) GenericStatus.Activo
        });
        return result.Id;
    }

    public async Task<int> UpdatePaymentTerm(int Id, PaymentTermData data)
    {
        Validation.ValidatePaymentTermData(data);
        var paymentTerm = await Data.GetByIdAsync<PaymentTerm>(Id);
        if (paymentTerm == null)
            throw new NotFound("No existe este término de pago");
        if (paymentTerm.Name != data.Name && await Data.ExistsPaymentTermWithName(data.Name))
            throw new AlreadyExists("Ya existe este término de pago");
        paymentTerm.Name = data.Name.Trim();
        paymentTerm.DueDays = data.DueDays;
        paymentTerm.LateFeeDays = data.LateFeeDays;
        paymentTerm.LateFeeRate = data.LateFeeRate;
        await Data.Update(paymentTerm);
        return paymentTerm.Id;
    }
    
    public async Task<int> UpdatePaymentTermStatus(int Id, int Status)
    {
        var paymentTerm = await Data.GetByIdAsync<PaymentTerm>(Id);
        if (paymentTerm == null)
            throw new NotFound("No existe este término de pago");
        paymentTerm.Status = Status;
        await Data.Update(paymentTerm);
        return paymentTerm.Id;
    }

    public async Task<PaymentTermView> GetPaymentTerm(int Id)
    {
        var paymentTerm = await Data.GetAsync<PaymentTerm>(x => x.Id == Id);
        if (paymentTerm == null)
            throw new NotFound("No existe este término de pago");
        return PaymentTermView.From(paymentTerm);
    }

    public async Task<List<PaymentTermView>> GetPaymentTerms(int? status, string? filter = "") =>
        (await Data.GetAll<PaymentTerm>(string.IsNullOrWhiteSpace(filter) ? null : c => c.Name.ToLower().Contains(filter.ToLower().Trim())))?.Select(PaymentTermView.From)
        .Where(c => status == null || c.Status.Id == status).ToList() ?? throw new NotFound("No se encontro ningun término de pago registrado");

    #endregion

    #region Payment Methods

    public async Task<int> CreatePaymentMethod(PaymentMethodData data)
    {
        if (string.IsNullOrWhiteSpace(data.Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsPaymentMethodWithName(data.Name))
            throw new AlreadyExists("Ya existe este método de pago");
        var result = await Data.Add(new PaymentMethod
            {Name = data.Name.Trim(), PaymentMethodTypeId = data.Type, Status = (int) GenericStatus.Activo});
        return result.Id;
    }

    public async Task<int> UpdatePaymentMethod(int Id, PaymentMethodData data)
    {
        if (string.IsNullOrWhiteSpace(data.Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        var paymentMethod = await Data.GetByIdAsync<PaymentMethod>(Id);
        if (paymentMethod == null)
            throw new NotFound("No existe este método de pago");

        paymentMethod.Name = data.Name.Trim();
        paymentMethod.PaymentMethodTypeId = data.Type;

        await Data.Update(paymentMethod);
        return paymentMethod.Id;
    }

    public async Task<int> UpdatePaymentMethodStatus(int Id, int Status)
    {
        var paymentMethod = await Data.GetByIdAsync<PaymentMethod>(Id);
        if (paymentMethod == null)
            throw new NotFound("No existe este método de pago");
        paymentMethod.Status = Status;
        await Data.Update(paymentMethod);
        return paymentMethod.Id;
    }

    public async Task<PaymentMethodView> GetPaymentMethod(int Id)
    {
        var paymentMethod = await Data.GetAsync<PaymentMethod>(x => x.Id == Id);
        if (paymentMethod == null)
            throw new NotFound("No existe este método de pago");
        return PaymentMethodView.From(paymentMethod);
    }

    public async Task<List<PaymentMethodView>> GetPaymentMethods(int? status_id, string? filter = "") =>
        (await Data.GetAll<PaymentMethod>(pm => string.IsNullOrWhiteSpace(filter) || pm.Name.ToLower().Contains(filter.ToLower().Trim()), pm => pm.PaymentMethodType))?
        .Select(pm => PaymentMethodView.From(pm))
        .Where(pm => status_id == null || pm.Status.Id == status_id).ToList() ?? throw new NotFound("No se encontro ningun método de pago registrado");

    public static Task<List<Item>> GetPaymentMethodTypes() => Task.FromResult(GetListFromEnum<PaymentMethodTypes>());

    #endregion

    #region Customer Ticket Types

    public async Task<int> CreateCustomerTicketType(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsCustomerTicketTypeWithName(Name))
            throw new AlreadyExists("Ya existe este tipo de ticket");
        var result = await Data.Add(new CustomerTicketType {Name = Name.Trim(), Status = (int)GenericStatus.Activo});
        return result.Id;
    }

    public async Task<int> UpdateCustomerTicketType(int Id, string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsCustomerTicketTypeWithName(Name))
            throw new AlreadyExists("Ya existe este tipo de ticket");
        var customerTicketType = await Data.GetByIdAsync<CustomerTicketType>(Id);
        if (customerTicketType == null)
            throw new NotFound("No existe este tipo de ticket");
        customerTicketType.Name = Name.Trim();
        await Data.Update(customerTicketType);
        return customerTicketType.Id;
    }
    
    public async Task<int> UpdateCustomerTicketTypeStatus(int Id, int Status)
    {
        var customerTicketType = await Data.GetByIdAsync<CustomerTicketType>(Id);
        if (customerTicketType == null)
            throw new NotFound("No existe este tipo de ticket");
        customerTicketType.Status = Status;
        await Data.Update(customerTicketType);
        return customerTicketType.Id;
    }
    
    public async Task<Item> GetCustomerTicketType(int Id)
    {
        var customerTicketType = await Data.GetAsync<CustomerTicketType>(x => x.Id == Id);
        if (customerTicketType == null)
            throw new NotFound("No existe esta provincia/estado");
        return new Item(customerTicketType.Id, customerTicketType.Name);
    }

    public async Task<List<StatusItemView>> GetCustomerTicketTypes(string? filter = "") =>
        (await Data.GetAll<CustomerTicketType>(string.IsNullOrWhiteSpace(filter)
            ? null
            : c => c.Name.ToLower().Contains(filter.ToLower().Trim())))?.Select(x => new StatusItemView(x.Id, x.Name, Item.From((GenericStatus)x.Status)))
        .ToList() ?? throw new NotFound("No se encontro ningun tipo de ticket registrado");

    #endregion

    #region Receivable Reasons

    public async Task<int> CreateReceivableReason(ReceivableReasonData data)
    {
        Validation.ValidateReceivableReasonData(data);
        if (await Data.ExistsReceivableReasonWithName(data.Name, data.DocumentTypeId))
            throw new AlreadyExists("Ya existe este motivo de cobro");
        var result = await Data.Add(new ReceivableReason
        {
            Name = data.Name.Trim(), DocumentType = data.DocumentTypeId, Description = data.Description,
            Status = (int) GenericStatus.Activo
        });
        return result.Id;
    }

    public async Task<int> UpdateReceivableReason(int Id, ReceivableReasonData data)
    {
        Validation.ValidateReceivableReasonData(data);
        var receivableReason = await Data.GetByIdAsync<ReceivableReason>(Id);
        if (receivableReason == null)
            throw new NotFound("No existe este motivo de cobro");
        if ((receivableReason.Name != data.Name || receivableReason.DocumentType != data.DocumentTypeId) &&
            await Data.ExistsReceivableReasonWithName(data.Name, data.DocumentTypeId))
            throw new AlreadyExists("Ya existe este motivo de cobro");
        receivableReason.Name = data.Name.Trim();
        receivableReason.DocumentType = data.DocumentTypeId;
        receivableReason.Description = data.Description;
        await Data.Update(receivableReason);
        return receivableReason.Id;
    }

    public async Task<int> UpdateReceivableReasonStatus(int Id, int Status)
    {
        var receivableReason = await Data.GetByIdAsync<ReceivableReason>(Id);
        if (receivableReason == null)
            throw new NotFound("No existe este motivo de cobro");
        receivableReason.Status = Status;
        await Data.Update(receivableReason);
        return receivableReason.Id;
    }

    public async Task<ReceivableReasonView> GetReceivableReason(int Id)
    {
        var receivableReason = await Data.GetAsync<ReceivableReason>(x => x.Id == Id);
        if (receivableReason == null)
            throw new NotFound("No existe este motivo de cobro");
        return ReceivableReasonView.From(receivableReason);
    }

    public async Task<List<ReceivableReasonView>> GetReceivableReasons(int? status, string? filter = "") =>
        (await Data.GetAll<ReceivableReason>(x => (status == null || x.Status == status) && (string.IsNullOrWhiteSpace(filter) || x.Name.ToLower().Contains(filter.ToLower().Trim()))))
        ?.Select(s => new ReceivableReasonView(s.Id, Item.From((ReceivableReasonTypes)s.DocumentType), s.Name, s.Description, Item.From((GenericStatus)s.Status)))
        .ToList() ?? throw new NotFound("No se encontro ningun motivo de cobro registrado");

    #endregion

    #region Property Types

    public async Task<int> CreatePropertyType(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsPropertyTypeWithName(Name))
            throw new AlreadyExists("Ya existe este tipo de propiedad");
        var result = await Data.Add(new PropertyType {Name = Name.Trim(), Status = (int) GenericStatus.Activo});
        return result.Id;
    }

    public async Task<int> UpdatePropertyType(int Id, string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsPropertyTypeWithName(Name))
            throw new AlreadyExists("Ya existe este tipo de propiedad");
        var propertyType = await Data.GetByIdAsync<PropertyType>(Id);
        if (propertyType == null)
            throw new NotFound("No existe este tipo de propiedad");
        propertyType.Name = Name.Trim();
        await Data.Update(propertyType);
        return propertyType.Id;
    }

    public async Task<int> UpdatePropertyTypeStatus(int Id, int Status)
    {
        var propertyType = await Data.GetByIdAsync<PropertyType>(Id);
        if (propertyType == null)
            throw new NotFound("No existe este tipo de propiedad");
        propertyType.Status = Status;
        await Data.Update(propertyType);
        return propertyType.Id;
    }

    public async Task<List<StatusItemView>> GetPropertyTypes(int? status, string? filter = "") =>
        (await Data.GetAll<PropertyType>(s => (status == null || s.Status == status) && (string.IsNullOrWhiteSpace(filter) || s.Name.ToLower().Contains(filter.ToLower().Trim()))))
        ?.Select(s => new StatusItemView(s.Id, s.Name, Item.From((GenericStatus)s.Status))).ToList() ??
        throw new NotFound("No se encontro ningun tipo de propiedad registrado");

    #endregion

    #region Tax Schedules

    public async Task<int> CreateTaxSchedule(TaxScheduleData data)
    {
        Validation.ValidateTaxScheduleData(data);
        if (await Data.ExistsTaxScheduleWithName(data.Name))
            throw new AlreadyExists("Ya existe esta escala de impuestos");
        var result = await Data.Add(new TaxSchedule {Name = data.Name.Trim(), TaxRate = data.Rate, Status = (int)GenericStatus.Activo});
        return result.Id;
    }

    public async Task<int> UpdateTaxSchedule(int Id, TaxScheduleData data)
    {
        Validation.ValidateTaxScheduleData(data);
        var taxSchedule = await Data.GetByIdAsync<TaxSchedule>(Id);
        if (taxSchedule == null)
            throw new NotFound("No existe esta escala de impuestos");
        if (taxSchedule.Name != data.Name && await Data.ExistsTaxScheduleWithName(data.Name))
            throw new AlreadyExists("Ya existe esta escala de impuestos");
        taxSchedule.Name = data.Name.Trim();
        taxSchedule.TaxRate = data.Rate;
        await Data.Update(taxSchedule);
        return taxSchedule.Id;
    }

    public async Task<int> UpdateTaxScheduleStatus(int Id, int Status)
    {
        var taxSchedule = await Data.GetByIdAsync<TaxSchedule>(Id);
        if (taxSchedule == null)
            throw new NotFound("No existe esta escala de impuestos");
        taxSchedule.Status = Status;
        await Data.Update(taxSchedule);
        return taxSchedule.Id;
    }

    public async Task<TaxScheduleView> GetTaxSchedule(int Id)
    {
        var taxSchedule = await Data.GetAsync<TaxSchedule>(x => x.Id == Id);
        if (taxSchedule == null)
            throw new NotFound("No existe esta escala de impuestos");
        return TaxScheduleView.From(taxSchedule);
    }

    public async Task<List<TaxScheduleView>> GetTaxSchedules(int? status, string? filter = "") =>
        (await Data.GetAll<TaxSchedule>(s => (status == null || s.Status == status) && (string.IsNullOrWhiteSpace(filter) || s.Name.ToLower().Contains(filter.ToLower().Trim()))))
        .Select(ts => new TaxScheduleView(ts.Id, ts.Name, ts.TaxRate, Item.From((GenericStatus)ts.Status)))
        .ToList() ?? throw new NotFound("No se encontro ninguna escala de impuestos registrada");

    #endregion

    #region NCF Sequences

    public async Task<int> CreateNCFSequence(NCFSequenceData data)
    {
        Validation.ValidateNCFSequenceData(data);
        if (await Data.ExistsNCFSequenceWithNcfType(data.TypeId))
            throw new AlreadyExists("Ya existe una secuencia con este tipo de NCF");
        var result = await Data.Add(new NCFSequenceSetting
        {
            Series = data.Series,
            NCFTypeId = data.TypeId,
            MaxSequence = data.Max,
            LastSequence = data.Min,
            DueDate = data.DueDate
        });
        return result.Id;
    }

    public async Task<int> UpdateNCFSequence(int Id, NCFSequenceData data)
    {
        if (data.Max < 0)
            throw new InvalidParameter("El numero maximo de secuencia no puede ser menor a 0");
        if (data.Min < 0)
            throw new InvalidParameter("El numero minimo de secuencia no puede ser menor a 0");
        if (data.Min >= data.Max)
            throw new InvalidParameter("El numero minimo de secuencia no puede ser mayor o igual al numero maximo");
        var ncfSequence = await Data.GetByIdAsync<NCFSequenceSetting>(Id);
        if (ncfSequence == null)
            throw new NotFound("No existe esta secuencia de NCF");
        ncfSequence.MaxSequence = data.Max;
        ncfSequence.DueDate = data.DueDate;
        ncfSequence.LastSequence = data.Min;
        await Data.Update(ncfSequence);
        return ncfSequence.Id;
    }
    
    public async Task<int> UpdateNCFSequenceStatus(int Id, int Status)
    {
        var ncfSequence = await Data.GetByIdAsync<NCFSequenceSetting>(Id);
        if (ncfSequence == null)
            throw new NotFound("No existe esta secuencia de NCF");
        ncfSequence.Status = Status;
        await Data.Update(ncfSequence);
        return ncfSequence.Id;
    }

    public async Task<NCFSequenceView> GetNCFSequence(int Id)
    {
        var ncfSequence = await Data.GetAsync<NCFSequenceSetting>(x => x.Id == Id);
        if (ncfSequence == null)
            throw new NotFound("No existe esta secuencia de NCF");
        return NCFSequenceView.From(ncfSequence);
    }

    public async Task<List<NCFSequenceView>> GetNCFSequences(int? status, string? filter = "") =>
        (await Data.GetAll<NCFSequenceSetting>(n => (status == null || n.Status == status) && (string.IsNullOrWhiteSpace(filter) || n.NCFType.Name.ToLower().Contains(filter.ToLower().Trim())), n => n.NCFType))
        ?.Select(NCFSequenceView.From).ToList() ?? throw new NotFound("No se encontro ninguna secuencia de NCF registrada");

    #endregion

    #region Branches

    public async Task<int> CreateBranch(BranchData data)
    {
        Validation.ValidateBranchData(data);
        if (await Data.ExistsBranchWithCode(data.Code))
            throw new AlreadyExists("Ya existe esta sucursal");
        var result = await Data.Add(new Branch
        {
            Code = data.Code.Trim(), 
            CityId = data.CityId, 
            BranchTypeId = data.BranchTypeId, 
            Locality = data.Locality,
            Address = data.Address, 
            Phone = data.Phone, 
            Status = (int)GenericStatus.Activo
        });
        return result.Id;
    }

    public async Task<int> UpdateBranch(int Id, BranchData data)
    {
        Validation.ValidateBranchData(data);
        var cam = await Data.GetByIdAsync<Branch>(Id);
        if (cam == null)
            throw new NotFound("No existe esta sucursal");
        if (cam.Code != data.Code && await Data.ExistsBranchWithCode(data.Code))
            throw new AlreadyExists("Ya existe esta sucursal");
        cam.Code = data.Code.Trim();
        cam.CityId = data.CityId;
        cam.BranchTypeId = data.BranchTypeId;
        cam.Locality = data.Locality;
        cam.Address = data.Address;
        cam.Phone = data.Phone;
        await Data.Update(cam);
        return cam.Id;
    }

    public async Task<int> UpdateBranchStatus(int Id, int Status)
    {
        var cam = await Data.GetByIdAsync<Branch>(Id);
        if (cam == null)
            throw new NotFound("No existe esta sucursal");
        cam.Status = Status;
        await Data.Update(cam);
        return cam.Id;
    }
    public async Task<BranchView> GetBranch(int Id)
    {
        var cam = await Data.GetAsync<Branch>(x => x.Id == Id, b => b.City, b => b.BranchType);
        if (cam == null)
            throw new NotFound("No existe esta sucursal");
        return BranchView.From(cam);
    }

    public async Task<List<BranchView>> GetBranches(int? status, int? type_id, string? filter = "") =>
        (await Data.GetAll<Branch>(b => (type_id == null || b.BranchTypeId == type_id) && (status == null || b.Status == status) && (string.IsNullOrWhiteSpace(filter) || b.Code.ToLower().Contains(filter.ToLower().Trim()))
            , b => b.City, b => b.BranchType))
        ?.Select(BranchView.From).ToList() ?? throw new NotFound("No se encontro ninguna sucursal registrada");

    public async Task<List<BranchView>> GetBranchesByType(int type) =>
        (await Data.GetAll<Branch>(b => b.BranchTypeId == type, b => b.City, b => b.BranchType))
        ?.Select(BranchView.From).ToList() ?? throw new NotFound("No se encontro ninguna sucursal registrada");

    public async Task<List<Item>> GetBranchTypes() =>
        (await Data.GetAll<BranchType>())?.Select(x => new Item(x.Id, x.Name)).ToList() ??
        throw new NotFound("No se encontro ningun tipo de sucursal registrado");

    #endregion

    #region Companies

    // public async Task<int> CreateCompany(CompanyData data)
    // {
    //     Validation.ValidateCompanyData(data);
    //     if (await Data.ExistsCompanyWithCode(data.Code))
    //         throw new AlreadyExists("Ya existe esta compañia");
    //     var result = await Data.Add(new Company{ Code = data.Code.Trim(), Name = data.Name.Trim(), Address = data.Address, Phone = data.Phone, Email = data.Email, TaxId = data.TaxId, TaxScheduleId = data.TaxScheduleId, BranchId = data.BranchId});
    //     return result.Id;
    // }
    public async Task<int> UpdateCompany(int Id, CompanyData data)
    {
        Validation.ValidateCompanyData(data);
        var company = await Data.GetByIdAsync<Company>(Id);
        if (company == null)
            throw new NotFound("No existe esta compañia");
        company.Name = data.Name.Trim();
        company.TaxRegistrationNumber = data.TaxRegistrationNumber;
        company.AddressLine1 = data.AddressLine1;
        company.AddressLine2 = data.AddressLine2;
        company.AddressLine3 = data.AddressLine3;
        company.CityId = data.CityId;
        company.PostalCode = data.PostalCode;
        company.Phone = data.Phone;
        company.Email = data.Email;
        await Data.Update(company);
        return company.Id;
    }

    public async Task<CompanyView> GetCompany(int Id)
    {
        var company = await Data.GetAsync<Company>(x => x.Id == Id, x => x.CompanySettings, x => x.CompanySettings.LateFeeProduct, x => x.City, x => x.City.State, x => x.City.State.Country);
        if (company == null)
            throw new NotFound("No existe esta compañia");
        return CompanyView.From(company);
    }

    public async Task<ReportCompanyData> GetCompanyForReport()
    {
        var company = await Data.GetAsync<Company>(x => x.Id == 1, x => x.CompanySettings, x => x.CompanySettings.LateFeeProduct, x => x.City, x => x.City.State, x => x.City.State.Country);
        if (company == null)
            throw new NotFound("No existe esta compañia");
        List<string> address = new () { company.AddressLine1, company.AddressLine2, company.AddressLine3, company.City.Name, company.City.State.Name, company.City.State.Country.Name, company.PostalCode };
        return new ReportCompanyData
        {
            Name = company.Name,
            Code = company.Code,
            Address = address.Where(x => !string.IsNullOrWhiteSpace(x)).Join(", "),
            City = company.City.Name,
            RNC = company.TaxRegistrationNumber,
            Phone = company.Phone
        };
    }
    // public async Task<List<CompanyView>> GetCompanies(string? filter = "") => 
    //     (await Data.GetAll<Company>(s => s.Name.ToLower() == filter.ToLower().Trim()))?.Select(CompanyView.From).ToList() ?? throw new NotFound("No se encontro ninguna compañia registrada");

    public async Task<int> UpdateCompanySettings(int Id, CompanySettingsData data)
    {
        Validation.ValidateCompanySettingsData(data);
        var company = await Data.GetByIdAsync<Company>(Id);
        if (company == null)
            throw new NotFound("No existe esta compañia");

        var tempId = company.CompanySettingsId;
        var settingsId = await Data.Add(new CompanySetting
        {
            TimeZone = data.TimeZone,
            DatePattern = data.DatePattern,
            TimePattern = data.TimePattern,
            LateFeeProductId = data.LateFeeProductId
        });
        company.CompanySettingsId = settingsId.Id;
        await Data.Update(company);

        if (tempId != null)
        {
            var settings = await Data.GetByIdAsync<CompanySetting>(tempId);
            if (settings != null)
                await Data.Delete(settings);
        }

        return company.Id;
    }

    #endregion

    #region Currencies

    public async Task<int> CreateCurrency(CurrencyData data)
    {
        Validation.ValidateCurrencyData(data);
        if (await Data.ExistsCurrencyWithCode(data.Code, data.Symbol))
            throw new AlreadyExists("Ya existe esta moneda");
        var result = await Data.Add(new Currency
        {
            Code = data.Code.Trim(), Name = data.Name.Trim(), Symbol = data.Symbol, Status = (int) GenericStatus.Activo
        });
        return result.Id;
    }

    public async Task<int> CreateCurrencyRate(int id, CurrencyRateData data)
    {
        Validation.ValidateCurrencyRateData(data);
        var currency = await Data.GetByIdAsync<Currency>(id);
        if (currency == null)
            throw new NotFound("No existe esta moneda");
        
        var rates = await Data.GetAll<CurrencyRate>(x => x.CurrencyId == id);
        if (rates.Any(x => x.StartDate <= data.StartDate && x.EndDate >= data.StartDate))
            throw new AlreadyExists("Ya existe una tasa de cambio en este rango de fechas");
        
        var result = await Data.Add(new CurrencyRate
        {
            CurrencyId = currency.Id,
            StartDate = data.StartDate,
            EndDate = data.EndDate,
            ExchangeRate = data.Rate,
        });
        return result.Id;
    }
    public async Task<int> UpdateCurrencyRate(int Id, CurrencyRateData data)
    {
        Validation.ValidateCurrencyRateData(data);
        var currencyRate = await Data.GetByIdAsync<CurrencyRate>(Id);
        if (currencyRate == null)
            throw new NotFound("No existe esta tasa de cambio");
        
        var rates = await Data.GetAll<CurrencyRate>(x => x.CurrencyId == currencyRate.CurrencyId && x.Id != currencyRate.Id);
        if (rates.Any(x => x.StartDate <= data.StartDate && x.EndDate >= data.StartDate))
            throw new AlreadyExists("Ya existe una tasa de cambio en este rango de fechas");
        
        currencyRate.StartDate = data.StartDate;
        currencyRate.EndDate = data.EndDate;
        currencyRate.ExchangeRate = data.Rate;
        await Data.Update(currencyRate);
        return currencyRate.Id;
    }

    public async Task<int> UpdateCurrency(int Id, CurrencyData data)
    {
        Validation.ValidateCurrencyData(data);
        var currency = await Data.GetByIdAsync<Currency>(Id);
        if (currency == null)
            throw new NotFound("No existe esta moneda");
        if ((currency.Code != data.Code || currency.Symbol != data.Symbol) &&
            await Data.ExistsCurrencyWithCode(data.Code, data.Symbol))
            throw new AlreadyExists("Ya existe esta moneda");
        currency.Code = data.Code.Trim();
        currency.Name = data.Name.Trim();
        currency.Symbol = data.Symbol;
        await Data.Update(currency);
        return currency.Id;
    }

    public async Task<int> UpdateCurrencyStatus(int Id, int Status)
    {
        var currency = await Data.GetByIdAsync<Currency>(Id);
        if (currency == null)
            throw new NotFound("No existe esta moneda");
        currency.Status = Status;
        await Data.Update(currency);
        return currency.Id;
    }

    public async Task<CurrencyDetailedView> GetCurrency(int Id)
    {
        var currency = await Data.GetAsync<Currency>(x => x.Id == Id, x => x.Rates);
        if (currency == null)
            throw new NotFound("No existe esta moneda");
        return CurrencyDetailedView.From(currency);
    }

    public async Task<List<CurrencyListView>> GetCurrencies(string? filter = "") =>
        (await Data.GetAll<Currency>(
            string.IsNullOrWhiteSpace(filter)
                ? null
                : s => s.Code.ToLower().Contains(filter.ToLower().Trim()), c => c.Rates))
        ?.Select(CurrencyListView.From).ToList() ?? throw new NotFound("No se encontro ninguna moneda registrada");

    public async Task<List<CurrencyListView>> GetCurrenciesWithRates(string? filter = "") =>
    (await Data.GetAll<Currency>(
        string.IsNullOrWhiteSpace(filter)
            ? null
            : s => s.Code.ToLower().Contains(filter.ToLower().Trim()), c => c.Rates))
    ?.Select(CurrencyListView.From).ToList() ?? throw new NotFound("No se encontro ninguna moneda registrada");

    #endregion

    #region Electrical Equipment

    public async Task<int> CreateElectricalEquipment(ElectricalEquipmentData data)
    {
        Validation.ValidateElectricalEquipmentData(data);
        if (await Data.ExistsElectricalEquipmentWithCode(data.Code))
            throw new AlreadyExists("Ya existe este equipo electrico");
        var result = await Data.Add(new ElectricalEquipment
        {
            Code = data.Code.Trim(),
            Name = data.Name.Trim(),
            Power = data.Power,
            DailyUsage = data.DailyUsage,
            MonthlyUsage = data.MonthlyUsage,
            UseHours = data.UsedHours,
            Status = (int) GenericStatus.Activo
        });
        return result.Id;
    }

    public async Task<int> UpdateElectricalEquipment(int Id, ElectricalEquipmentData data)
    {
        Validation.ValidateElectricalEquipmentData(data);
        var electricalEquipment = await Data.GetByIdAsync<ElectricalEquipment>(Id);
        if (electricalEquipment == null)
            throw new NotFound("No existe este equipo electrico");
        if (electricalEquipment.Code != data.Code && await Data.ExistsElectricalEquipmentWithCode(data.Code))
            throw new AlreadyExists("Ya existe este equipo electrico");
        electricalEquipment.Code = data.Code.Trim();
        electricalEquipment.Name = data.Name.Trim();
        electricalEquipment.Power = data.Power;
        electricalEquipment.DailyUsage = data.DailyUsage;
        electricalEquipment.MonthlyUsage = data.MonthlyUsage;
        electricalEquipment.UseHours = data.UsedHours;
        await Data.Update(electricalEquipment);
        return electricalEquipment.Id;
    }

    public async Task<int> UpdateElectricalEquipmentStatus(int Id, int Status)
    {
        var electricalEquipment = await Data.GetByIdAsync<ElectricalEquipment>(Id);
        if (electricalEquipment == null)
            throw new NotFound("No existe este equipo electrico");
        electricalEquipment.Status = Status;
        await Data.Update(electricalEquipment);
        return electricalEquipment.Id;
    }

    public async Task<ElectricalEquipmentView> GetElectricalEquipment(int Id)
    {
        var electricalEquipment = await Data.GetByIdAsync<ElectricalEquipment>(Id);
        if (electricalEquipment == null)
            throw new NotFound("No existe este equipo electrico");
        return ElectricalEquipmentView.From(electricalEquipment);
    }

    public async Task<List<ElectricalEquipmentView>> GetElectricalEquipments(string? filter = "") =>
        (await Data.GetAll<ElectricalEquipment>(string.IsNullOrWhiteSpace(filter)
            ? null
            : s => s.Code.ToLower().Contains(filter.ToLower().Trim())))?.Select(ElectricalEquipmentView.From)
        .ToList() ?? throw new NotFound("No se encontro ningun equipo electrico registrado");

    #endregion

    #region Routes

    public async Task<int> CreateRoute(RouteData data)
    {
        Validation.ValidateRouteData(data);
        if (await Data.ExistsRouteWithName(data.Name))
            throw new AlreadyExists("Ya existe esta ruta");
        var result = await Data.Add(new Route
        {
            Name = data.Name.Trim(),
            RouteTypeId = data.TypeId,
            BillingCycleId = data.BillingCycleId,
            Status = (int) GenericStatus.Activo
        });
        return result.Id;
    }

    public async Task<int> UpdateRoute(int Id, RouteData data)
    {
        Validation.ValidateRouteData(data);
        var route = await Data.GetByIdAsync<Route>(Id);
        if (route == null)
            throw new NotFound("No existe esta ruta");
        if (route.Name != data.Name && await Data.ExistsRouteWithName(data.Name))
            throw new AlreadyExists("Ya existe esta ruta");
        route.Name = data.Name.Trim();
        route.RouteTypeId = data.TypeId;
        route.BillingCycleId = data.BillingCycleId;
        await Data.Update(route);
        return route.Id;
    }

    public async Task<int> UpdateRouteStatus(int Id, int Status)
    {
        var route = await Data.GetByIdAsync<Route>(Id);
        if (route == null)
            throw new NotFound("No existe esta ruta");
        if (route.Status == Status)
            throw new InvalidParameter("Ya esta ruta ya tiene este estado");
        route.Status = Status;
        await Data.Update(route);
        return route.Id;
    }

    public async Task<RouteDetailedView> GetRoute(int id)
    {
        var route = await Data.GetByIdAsync<Route>(id);
        if (route == null)
            throw new NotFound("No existe esta ruta");
        return await Data.GetRouteView(id);
    }
    
    
    public async Task<List<int>> AddAddressesToRoute(int routeId, List<int> addressIds)
    {
        var addresses = await Data.GetAll<CustomerAddress>(c => addressIds.Contains(c.Id));
        if (addresses.Count != addressIds.Count)
            throw new NotFound("No se encontraron todas las direcciones");
        var route = await Data.GetByIdAsync<Route>(routeId);
        if (route == null)
            throw new NotFound("No existe esta ruta");
        
        var addedAddresses = new List<int>();
        foreach (var address in addresses)
        {
            address.RouteId = routeId;
            await Data.Update(address);
            addedAddresses.Add(address.Id);
        }
        return addedAddresses;
    }

    public async Task<List<RouteListView>> GetRoutes(int? status_id, int start = 0, int count = 20, string? filter = "") =>
        await Data.GetRoutes(status_id, start, count, filter);

    public async Task<int> GetRouteCount(int? status_id, string? filter = "") =>
        await Data.Count<Route>(r => 
            (string.IsNullOrWhiteSpace(filter) || r.Name.ToLower().Contains(filter.ToLower().Trim())) &&
            (!status_id.HasValue || r.Status == status_id.Value));

    #endregion

    #region Route Types

    public async Task<int> CreateRouteType(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        if (await Data.ExistsRouteTypeWithName(Name))
            throw new AlreadyExists("Ya existe este tipo de ruta");
        var result = await Data.Add(new RouteType {Name = Name.Trim(), Status = (int) GenericStatus.Activo});
        return result.Id;
    }

    public async Task<int> UpdateRouteType(int Id, string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidParameter("El nombre no puede estar vacio");
        var routeType = await Data.GetByIdAsync<RouteType>(Id);
        if (routeType == null)
            throw new NotFound("No existe este tipo de ruta");
        if (routeType.Name != Name && await Data.ExistsRouteTypeWithName(Name))
            throw new AlreadyExists("Ya existe este tipo de ruta");
        routeType.Name = Name.Trim();
        await Data.Update(routeType);
        return routeType.Id;
    }

    public async Task<int> UpdateRouteTypeStatus(int Id, int Status)
    {
        var routeType = await Data.GetByIdAsync<RouteType>(Id);
        if (routeType == null)
            throw new NotFound("No existe este tipo de ruta");
        routeType.Status = Status;
        await Data.Update(routeType);
        return routeType.Id;
    }

    public async Task<RouteTypeView> GetRouteType(int Id)
    {
        var routeType = await Data.GetByIdAsync<RouteType>(Id);
        if (routeType == null)
            throw new NotFound("No existe este tipo de ruta");
        return RouteTypeView.From(routeType);
    }

    public async Task<List<RouteTypeView>> GetRouteTypes(int start = 0, int count = 20, string? filter = "") =>
        (await Data.GetAll<RouteType>(string.IsNullOrWhiteSpace(filter)
            ? null
            : s => s.Name.ToLower().Contains(filter.ToLower().Trim())))
        .Skip(start)
        .Take(count)
        .Any()
            ? (await Data.GetAll<RouteType>(string.IsNullOrWhiteSpace(filter)
                ? null
                : s => s.Name.ToLower().Contains(filter.ToLower().Trim())))
            .Skip(start)
            .Take(count)
            .Select(RouteTypeView.From)
            .ToList()
            : new List<RouteTypeView>();

    public async Task<int> GetRouteTypeCount(string? filter = "") =>
        await Data.Count<RouteType>(string.IsNullOrWhiteSpace(filter)
            ? null
            : s => s.Name.ToLower().Contains(filter.ToLower().Trim()));
    #endregion
    
    public static Task<List<Item>> GetAreaTypes() => Task.FromResult(GetListFromEnum<AreaTypes>());

    public static Task<List<Item>> GetNcfTypes() => Task.FromResult(GetListFromEnum<NCFTypes>());

    public static Task<List<FrequencyView>> GetFrequencies()
    {
        var result = new List<FrequencyView>();
        foreach (var frequency in GetListFromEnum<Frequencies>())
        {
            result.Add(new FrequencyView
            {
                Id = frequency.Id,
                Description = frequency.Description,
                Divide = frequency.Id switch
                {
                    (int) Frequencies.Semanal => 52,
                    (int) Frequencies.Mensual => 12,
                    (int) Frequencies.Bimestral => 6,
                    (int) Frequencies.Trimestral => 4,
                    (int) Frequencies.Semestral => 2,
                    (int) Frequencies.Anual => 1,
                    _ => 12
                }
            });
        }
        return Task.FromResult(result);
    }

    public static Task<List<Item>> GetDeliveryMethods() => Task.FromResult(GetListFromEnum<DeliveryMethods>());

    public async Task<List<Item>> GetContactTypes() =>
        (await Data.GetAll<ContactType>()).Select(ct => new Item(ct.Id, ct.Name)).ToList();


    public static Task<List<Item>> GetInvoiceStatus() => Task.FromResult(GetListFromEnum<InvoiceStatuses>());

    public static Task<List<Item>> GetPaymentStatus() => Task.FromResult(GetListFromEnum<PaymentStatuses>());

    public async Task<List<Item>> GetPriceTypes() => 
        (await Data.GetAll<PriceType>()).Select(pt => new Item(pt.Id, pt.Name)).ToList();

    public async Task<List<string>> GetCashiers() =>
        await Data.GetCashiers();

    public static Task<List<Item>> GetReceivableReasonTypes() => Task.FromResult(GetListFromEnum<ReceivableReasonTypes>());
    
    public static Task<List<Item>> GetReadingTypes() => Task.FromResult(GetListFromEnum<ReadingTypes>());
    public static Task<List<Item>> GetMeasuringTypes() => Task.FromResult(GetListFromEnum<MeasuringTypes>());
    
    public static Task<List<Item>> GetPermissionTypes() => Task.FromResult(GetListFromEnum<PermissionTypes>());
    public static Task<List<Item>> GetPermissionAreas() => Task.FromResult(GetListFromEnum<PermissionAreas>()); 

    private static List<Item> GetListFromEnum<T>() where T : Enum
    {
        var list = new List<Item>();
        foreach (var value in Enum.GetValues(typeof(T)))
            list.Add(new Item((int)(object)value, value.ToString().PascalCaseWithInitialsToTitleCase()));
        return list;
    }

}