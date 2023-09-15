using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Report;
using FleetTechCore.DTOs.Shared;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Enums;
using FleetTechCore.Errors;
using FleetTechCore.Models.Address;
using FleetTechCore.Models.Company;
using FleetTechCore.Services;
using FleetTechCore.Services.Infrastructure;
using FleetTechCore.Services.Model_Related_Services;

namespace FleetTechCore.Logic;

//Naming conventions for regions: Plural names
public partial class Logic
{
    public Logic(IAuditService audit, IDataService data, IAuthenticationService authentication, 
        IEmailService emails, IResourceService resources,/* ICedulaService cedulaService,*/
        IClientService clientService)
    {
        Audit = audit;
        Data = data;
        Authentication = authentication;
        Email = emails;
        Resources = resources;
        //Cedula = cedulaService;
        Client = clientService;
    }

    readonly IAuditService Audit;
    readonly IDataService Data;
    readonly IAuthenticationService Authentication;
    readonly IEmailService Email;
    readonly IResourceService Resources;
    //readonly ICedulaService Cedula;
    readonly IClientService Client;

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

    public async Task<int> CreateCompany(CompanyData data)
    {
        Validation.ValidateCompanyData(data);
        if (await Data.ExistsCompanyWithCode(data.Code))
            throw new AlreadyExists("Ya existe esta compañia");
        var result = await Data.Add(
            new Company
            {
                Code = data.Code.Trim(),
                TaxRegistrationNumber = data.TaxRegistrationNumber.Trim(),
                AddressLine1 = data.AddressLine1.Trim(),
                AddressLine2 = data.AddressLine2?.Trim(),
                AddressLine3 = data.AddressLine3?.Trim(),
                Region = data.Region.Trim(),
                CityId = data.CityId,
                PostalCode = data.PostalCode.Trim(),
                Name = data.Name.Trim(),
                Phone = data.Phone,
                Email = data.Email,
                UsernamePrefix = data.Prefix.Trim(),
                CompanySettingsId = null,
            });
        return result.Id;
    }
    public async Task<int> UpdateCompany(int Id, CompanyData data)
    {
        Validation.ValidateCompanyData(data);
        var company = await Data.GetByIdAsync<Company>(Id);
        if (company == null)
            throw new NotFound("No existe esta compañia");
        if (company.Code != data.Code && await Data.ExistsCompanyWithCode(data.Code))
            throw new AlreadyExists("Ya existe esta compañia");
        company.Code = data.Code.Trim();
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
        var company = await Data.GetAsync<Company>(x => x.Id == Id, x => x.CompanySettings, x => x.City, x => x.City.State, x => x.City.State.Country);
        if (company == null)
            throw new NotFound("No existe esta compañia");
        return CompanyView.From(company);
    }

    public async Task<ReportCompanyData> GetCompanyForReport()
    {
        var company = await Data.GetAsync<Company>(x => x.Id == 1, x => x.CompanySettings, x => x.City, x => x.City.State, x => x.City.State.Country);
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

        
    public static Task<List<Item>> GetPermissionTypes() => Task.FromResult(GetListFromEnum<PermissionTypes>());
    public static Task<List<Item>> GetPermissionAreas() => Task.FromResult(GetListFromEnum<PermissionAreas>()); 
    public static Task<List<Item>> GetVehicleState() => Task.FromResult(GetListFromEnum<VehicleState>());

    private static List<Item> GetListFromEnum<T>() where T : Enum
    {
        var list = new List<Item>();
        foreach (var value in Enum.GetValues(typeof(T)))
            list.Add(new Item((int)(object)value, value.ToString().PascalCaseWithInitialsToTitleCase()));
        return list;
    }

}