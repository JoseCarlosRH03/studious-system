using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace BackendTemplateAPI.Services.Infrastructure;

public class EmailService : IEmailService {
   public class MailSettings {
      public string EmailFrom   { get; set; }
      public string SmtpHost    { get; set; }
      public int    SmtpPort    { get; set; }
      public string SmtpUser    { get; set; }
      public string SmtpPass    { get; set; }
      public string DisplayName { get; set; }
   }

   public EmailService(IOptions<MailSettings> settings, IWebHostEnvironment env) {
      Settings   = settings.Value;
      MailClient = new SmtpClient(Settings.SmtpHost, Settings.SmtpPort) {
         UseDefaultCredentials = false,
         Credentials = new NetworkCredential(Settings.SmtpUser, Settings.SmtpPass),
         EnableSsl   = true
      };
      Environment = env;
   }

   readonly MailSettings Settings;
   readonly SmtpClient MailClient;
   readonly IWebHostEnvironment Environment;

   static readonly Regex TemplateVariable = new(@"\{\{(.+?)\}\}");

   MailMessage Email(string email_address, string name, string subject, string body) {
      var unprocessed_variables = TemplateVariable.Matches(body);
      if (unprocessed_variables.Any()) {
         LogService.LogException(new Exception($"Plantilla de correo no procesada correctamente.\n{string.Join('\n', unprocessed_variables.Select(v => " - " + v.Groups[1].Value))}"));
         throw new Exception("Error en servicio de mensajeria por correos.");
      }

      var email = new MailMessage {
         From       = new MailAddress(Settings.EmailFrom, Settings.DisplayName),
         Subject    = subject,
         Body       = body,
         IsBodyHtml = true
      };
      email.To.Add(new MailAddress(email_address, name));
      return email;
   }

   async Task<string> ReadTemplate(string template_name) {
      using var file_reader = new StreamReader(Path.Combine(Environment.ContentRootPath, "Templates", template_name + ".html"));
      return await file_reader.ReadToEndAsync();
   }

   public Task SendPasswordResetEmail(User user, string token, string origin) {
      throw new NotImplementedException();
   }

   public Task SendPasswordResetEmailNotFound(string email, string origin) {
      throw new NotImplementedException();
   }

   public async Task SendVerificationEmail(User user, string token, string origin) {
      var template = await ReadTemplate("email-verification");
      var email = Email(user.Email, user.FirstName, "Verificación de correo", template
         .Replace("{{origin}}", origin)
         .Replace("{{name}}",   user.FirstName)
         .Replace("{{token}}",  token));
      await MailClient.SendMailAsync(email);
   }

   public Task SendWelcomeEmail(User user, string origin) {
      throw new NotImplementedException();
   }
}