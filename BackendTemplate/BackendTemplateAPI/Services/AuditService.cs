using BackendTemplateCore.Models.User;
using BackendTemplateCore.Services.Model_Related_Services;

namespace BackendTemplateAPI.Services;

public class AuditService : IAuditService
{
    User? user { get; set; }

    public void SetCurrentUser(User User) => user = User;
    public User? GetCurrentUser() => user;
}