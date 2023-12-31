﻿using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Models.Fleet;
using FleetTechCore.Models.Fuel;
using FleetTechCore.Models.Supply;
using FleetTechCore.Models.User;
using FleetTechCore.Models.WorkShop;
using Microsoft.EntityFrameworkCore;

namespace FleetTechAPI.Services.Data;

public partial class DataService
{
    
    #region User Management
    public async Task RegisterLogin(Guid userId)
    {
        // await Audits.AddAsync(new Audit
        // {
        //     UserId = userId,
        //     Type = (int) AuditTypes.Login,
        //     TableName = "Users",
        //     DateTime = DateTime.Now,
        //     PrimaryKey = JsonSerializer.Serialize(new Dictionary<string, Guid> {{"Id", userId}})
        // });
        await SaveChangesAsync();
    }

    public async Task<Guid> LockoutUser(User user, int days = 0, int hours = 0)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        if (days == 0 && hours == 0) user.LockoutEnd = DateTime.Now.AddYears(100);
        else user.LockoutEnd = DateTime.Now.AddDays(days).AddHours(hours);

        Users.Update(user);

        // await Audits.AddAsync(new Audit
        // {
        //     UserId = user.Id,
        //     Type = (int) AuditTypes.Lockout,
        //     TableName = "Users",
        //     DateTime = DateTime.Now,
        //     OldValues = JsonSerializer.Serialize(new Dictionary<string, object?> {{"LockoutEnd", null}}),
        //     NewValues = JsonSerializer.Serialize(new Dictionary<string, object?> {{"LockoutEnd", user.LockoutEnd}}),
        //     AffectedColumns = JsonSerializer.Serialize(new List<string> {"LockoutEnd"}),
        //     PrimaryKey = JsonSerializer.Serialize(new Dictionary<string, Guid> {{"Id", user.Id}})
        // });
        await SaveChangesAsync();
        return user.Id;
    }

    public async Task<Guid> UnlockoutUser(User user)
    {
        // await Audits.AddAsync(new Audit
        // {
        //     UserId = user.Id,
        //     Type = (int) AuditTypes.Unlockout,
        //     TableName = "Users",
        //     DateTime = DateTime.Now,
        //     OldValues = JsonSerializer.Serialize(new Dictionary<string, object?> {{"LockoutEnd", user.LockoutEnd}}),
        //     NewValues = JsonSerializer.Serialize(new Dictionary<string, object?> {{"LockoutEnd", DateTime.Now}}),
        //     AffectedColumns = JsonSerializer.Serialize(new List<string> {"LockoutEnd"}),
        //     PrimaryKey = JsonSerializer.Serialize(new Dictionary<string, Guid> {{"Id", user.Id}})
        // });
        user.LockoutEnd = DateTime.Now;
        Users.Update(user);
        await SaveChangesAsync();
        return user.Id;
    }
    #endregion

    #region Fleet Management
        public async Task<Vehicle> GetVehicleById(int Id) =>
        await Vehicles.Include(v => v.FuelType).FirstOrDefaultAsync(v => v.Id == Id);

    public async Task<Driver> GetDriverById(int Id) =>
    await Drivers.Include(d => d.LicenseCategory).FirstOrDefaultAsync(d => d.Id == Id);
    #endregion

    #region  Fuel Managemen
    public async Task<List<ServicePlaseView>> GetAllFuelStation() =>
        await FuelStations
        .Include(f => f.Contacts)
        .Include(f => f.Address)
            .ThenInclude(f => f.City)
                .ThenInclude(f => f.State)
        .Select(f => ServicePlaseView.From(f))
        .ToListAsync();
    public async Task<FuelStation> GetFuelStationById(int Id) =>
       await FuelStations
       .Include(f => f.Contacts)
       .Include(f => f.Address)
           .ThenInclude(f => f.City)
               .ThenInclude(f => f.State)
       .FirstOrDefaultAsync( f => f.Id == Id);

    public async Task<List<PriceView>> GetAllFuelPrice() => 
        await FuelPrices
        .Include(p => p.FuelType)
        .Select(p => PriceView.From(p))
        .ToListAsync();

    public async Task<FuelPrice> GetFuelPriceById(int id) =>
        await FuelPrices
        .Include(p => p.FuelType)
        .FirstOrDefaultAsync(p => p.Id == id);
    #endregion

    #region  Supply Managemen
    public async Task<List<SupplyView>> GetAllSupply() =>
        await Suppliers
        .Include(s => s.Contacts)
        .Include(s => s.Address)
            .ThenInclude(s => s.City)
                .ThenInclude(s => s.State)
        .Select(f => SupplyView.From(f))
        .ToListAsync();
    public async Task<Supplier> GetSupplyById(int Id) =>
       await Suppliers
       .Include(s => s.Contacts)
       .Include(s => s.Address)
           .ThenInclude(s => s.City)
               .ThenInclude(s => s.State)
       .FirstOrDefaultAsync(s => s.Id == Id);
    #endregion

    #region  Mechanical Managemen
    public async Task<List<MechanicalWorkshopView>> GetAllMechanicalWorkshop() =>
        await MechanicalWorkshop
        .Include(m => m.Contacts)
        .Include(m => m.Specialties)
        .Include(m => m.Address)
            .ThenInclude(m => m.City)
                .ThenInclude(m => m.State)
        .Select(f => MechanicalWorkshopView.From(f))
        .ToListAsync();
    public async Task<MechanicalWorkshop> GetMechanicalWorkshopById(int Id) =>
       await MechanicalWorkshop
       .Include(m => m.Contacts)
       .Include(m => m.Specialties)
       .Include(m => m.Address)
           .ThenInclude(m => m.City)
               .ThenInclude(m => m.State)
       .FirstOrDefaultAsync(m => m.Id == Id);

    public async Task<List<MechanicView>> GetAllMechanic() =>
        await Mechanics
        .Include(m => m.Specialties)
        .Select(f => MechanicView.From(f))
        .ToListAsync();
    public async Task<Mechanic> GetMechanicById(int Id) =>
       await Mechanics
       .Include(m => m.Specialties)
       .FirstOrDefaultAsync(m => m.Id == Id);
    #endregion
}