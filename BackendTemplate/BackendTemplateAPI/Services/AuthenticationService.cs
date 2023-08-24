using BackendTemplateCore;
using BackendTemplateCore.Models;
using BackendTemplateCore.Services;
using Microsoft.AspNetCore.Identity;

namespace BackendTemplateAPI.Services;

public class AuthenticationService : IAuthenticationService {
   public AuthenticationService(IEncryptionService encryption) {
      Encryption = encryption;
   }

   readonly IEncryptionService Encryption;
   readonly PasswordHasher<User> Hasher = new();

   internal record struct AuthenticationToken(Guid user_id, DateTime date_issued);

   public bool VerifyPassword(User user, string password) =>
      Hasher.VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Failed;

   public string GenerateToken(User user, DateTime? issued = null) =>
      Encryption.Encrypt(new AuthenticationToken(user.Id, issued ?? DateTime.Now));

   public (bool success, Guid? user_id, DateTime? date_issued) Authenticate(string token) {
      try {
         var data = Encryption.Decrypt<AuthenticationToken>(token);
         return (true, data.user_id, data.date_issued);
      } catch (Exception ex) {
         LogService.LogException(ex);
         return (false, null, null);
      }
   }

   public string HashPassword(User? user, string password) =>
      Hasher.HashPassword(user, password);
}