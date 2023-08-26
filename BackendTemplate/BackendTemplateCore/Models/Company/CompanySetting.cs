

namespace BackendTemplateCore.Models.Company;

public class CompanySetting : AuditableEntity
{
    public string TimeZone { get; set; }
    public string DatePattern { get; set; }
    public string TimePattern { get; set; }
}