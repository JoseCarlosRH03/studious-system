using System.ComponentModel.DataAnnotations;

namespace FleetTechCore.DTOs.Data;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}