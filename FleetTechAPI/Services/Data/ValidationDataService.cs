using Microsoft.EntityFrameworkCore;

namespace FleetTechAPI.Services.Data;

public partial class DataService
{
    public Task<bool> ExistsUserWithEmail(string email) =>
        Users.AnyAsync(u => u.Email.ToLower() == email.ToLower().Trim());
    public Task<bool> ExistsCityWithName(string name, int stateId) =>
        Cities.AnyAsync(u => u.Name!.ToLower() == name.ToLower() && u.StateId == stateId);
    public Task<bool> ExistsStateWithName(string name, int countryId) =>
        States.AnyAsync(u => u.Name!.ToLower() == name.ToLower() && u.CountryId == countryId);
    public Task<bool> ExistsBranchWithCode(string Code) =>
        Branches.AnyAsync(u => u.Code!.ToLower() == Code.ToLower());
    public Task<bool> ExistsRoleWithName(string name) =>
        Roles.AnyAsync(u => u.Name!.ToLower() == name.ToLower());
    public Task<bool> ExistsUserWithPhone(string phone) =>
        Users.AnyAsync(u => u.Phone == phone.Trim() && !string.IsNullOrWhiteSpace(u.Phone));
    public Task<bool> ExistsCompanyWithCode(string code) =>
        Companies.AnyAsync(u => u.Code.ToLower() == code.Trim().ToLower());
    public Task<bool> ExistsStationWithRnc(string rnc) =>
        FuelStations.AnyAsync(u => u.RNC.ToLower() == rnc.Trim().ToLower());
    public Task<bool> ExistsSuplyWithRnc(string rnc) =>
    Suppliers.AnyAsync(u => u.RNC.ToLower() == rnc.Trim().ToLower());

}