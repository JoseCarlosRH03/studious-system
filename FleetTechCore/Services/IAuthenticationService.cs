using FleetTechCore.Models.User;

namespace FleetTechCore.Services;

public interface IAuthenticationService {
   bool VerifyPassword(User user, string password);
   string GenerateToken(User user, DateTime? issued = null);
   (bool success, Guid? user_id, DateTime? date_issued) Authenticate(string token);
   string HashPassword(User? user, string password);
}