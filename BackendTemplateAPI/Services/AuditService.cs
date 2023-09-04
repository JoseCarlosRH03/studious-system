using FleetTechCore.Models.User;
using FleetTechCore.Services.Model_Related_Services;

namespace FleetTechAPI.Services;

public class AuditService : IAuditService
{
    User? user { get; set; }

    public void SetCurrentUser(User User) => user = User;
    public User? GetCurrentUser() => user;
}