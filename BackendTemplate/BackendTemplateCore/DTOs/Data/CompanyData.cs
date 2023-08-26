namespace BackendTemplateCore.DTOs.Data;

public record struct CompanyData(
    string Name,
    string Code,
    string TaxRegistrationNumber,
    string AddressLine1,
    string AddressLine2,
    string AddressLine3,
    string Region,
    int CityId,
    string Prefix,
    string PostalCode,
    string Phone,
    string Email);