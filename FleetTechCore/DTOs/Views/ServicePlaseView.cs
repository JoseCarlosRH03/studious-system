using FleetTechCore.Model;

namespace FleetTechCore.DTOs.Views;

public record struct ServicePlaseView
    (int Id,
        string Code,
        string CompanyName,
        string RNC,
        string Phone,
        string Email,
        string AddressLine1,
        string AddressLine2,
        string AddressLine3,
        int CityId,
        string City,
        List<ContactView> Contacts
    ){
        public static ServicePlaseView From(ServicePlace data) => new()
        {
            Id = data.Id,
            Code = data.Code,
            CompanyName = data.CompanyName,
            RNC = data.RNC,
            Phone = data.Phone,
            Email = data.Email,
            AddressLine1 = data.Address.AddressLine1,
            AddressLine2 = data.Address.AddressLine2,
            AddressLine3 = data.Address.AddressLine3,
            CityId = data.Address.CityId,
            City = $"{data.Address.City.Name} | {data.Address.City.State.Name}",
            Contacts = data.Contacts.Select(c => ContactView.From(c)).ToList(),

        };     

     };
