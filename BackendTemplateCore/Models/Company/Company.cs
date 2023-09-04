using FleetTechCore.Models.Address;

namespace FleetTechCore.Models.Company;

public class Company : AuditableEntity
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string TaxRegistrationNumber { get; set; }

    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string Region { get; set; }
    public int CityId { get; set; }
    public string PostalCode { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    
    public string? UsernamePrefix { get; set; }
    public int? CompanySettingsId { get; set; }
    public virtual City City { get; set; }
    public virtual CompanySetting CompanySettings { get; set; }
}