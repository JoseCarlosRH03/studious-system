using BackendTemplateCore.Models;

namespace BackendTemplateCore.Services.Model_Related_Services;

public interface IAuditService
{
    void SetCurrentUser(User UserId);
    User? GetCurrentUser();
}