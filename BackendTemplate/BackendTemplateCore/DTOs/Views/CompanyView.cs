using Core.Models;

namespace Core.DTOs.Queries;

public record struct CompanyView(
    int Id,
    string Name,
    string Code,
    string Document,
    string AddressLine1,
    string AddressLine2,
    string AddressLine3,
    CityView City,
    string PostalCode,
    string Phone,
    string Email,
    CompanySettingsView? Settings)
{
    public static CompanyView From (Company company) => new(
        company.Id,
        company.Name,
        company.Code,
        company.TaxRegistrationNumber,
        company.AddressLine1,
        company.AddressLine2,
        company.AddressLine3,
        CityView.From(company.City), 
        company.PostalCode,
        company.Phone,
        company.Email,
        company.CompanySettingsId is null ? null : CompanySettingsView.From(company.CompanySettings));
}

public record struct CompanySettingsView(
    int Id,
    string TimeZone,
    string DatePattern,
    string TimePattern
)
{
    public static CompanySettingsView From(CompanySetting setting) => new(
        setting.Id,
        setting.TimeZone,
        setting.DatePattern,
        setting.TimePattern
    );
}