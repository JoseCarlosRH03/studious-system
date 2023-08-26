using System.ComponentModel.DataAnnotations;

namespace BackendTemplateCore.DTOs.Data;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}