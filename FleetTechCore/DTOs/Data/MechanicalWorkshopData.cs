namespace FleetTechCore.DTOs.Data
{
    public record struct MechanicalWorkshopData
    (   int Id,
        string Code, 
        string CompanyName,
        string RNC,
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
