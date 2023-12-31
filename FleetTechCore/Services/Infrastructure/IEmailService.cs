﻿using FleetTechCore.Models.User;

namespace FleetTechCore.Services.Infrastructure;

public interface IEmailService {
   Task SendVerificationEmail(User user, string token, string origin);
   Task SendWelcomeEmail(User user, string origin);
   Task SendPasswordResetEmail(User user, string token, string origin);
   Task SendPasswordResetEmailNotFound(string email, string origin);
}
