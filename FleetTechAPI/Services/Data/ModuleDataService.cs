using System.Text.Json;
using FleetTechCore.Enums;
using FleetTechCore.Errors;
using FleetTechCore.Models.User;
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
        
    #endregion
}