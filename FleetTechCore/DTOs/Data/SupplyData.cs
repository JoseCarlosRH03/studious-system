namespace FleetTechCore.DTOs.Data
{
    public record struct SupplyData
    (   int Id,
        string Code, 
        string CompanyName,
        string RNC,
        string Name,
        string Position,
        string Phone, 
        string Email,
        string AddressLine1, 
        string AddressLine2, 
        string AddressLine3,
        int    CityId,
        int    StatusId,
        List<ContactData> Contacts
    );
}
