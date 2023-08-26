using BackendTemplateCore.Models.User;

namespace BackendTemplateCore.Services.Model_Related_Services;

public interface IAuditService
{
    void SetCurrentUser(User UserId);
    User? GetCurrentUser();
}