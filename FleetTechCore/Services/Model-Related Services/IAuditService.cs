using FleetTechCore.Models.User;

namespace FleetTechCore.Services.Model_Related_Services;

public interface IAuditService
{
    void SetCurrentUser(User UserId);
    User? GetCurrentUser();
}