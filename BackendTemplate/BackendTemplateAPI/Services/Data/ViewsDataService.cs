using BackendTemplateCore.DTOs.Shared;
using BackendTemplateCore.DTOs.Views;
using BackendTemplateCore.Enums;
using BackendTemplateCore.Errors;
using BackendTemplateCore.Models.Address;
using BackendTemplateCore.Models.User;
using Microsoft.EntityFrameworkCore;

namespace BackendTemplateAPI.Services.Data;

public partial class DataService
{
    public Task<Country?> GetCurrentCountry() =>
        Countries.FirstOrDefaultAsync(c =>
            Companies.FirstOrDefault() != null && c.Id == Companies.FirstOrDefault()!.Id);

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
}