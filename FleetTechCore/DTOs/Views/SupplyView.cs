using FleetTechCore.DTOs.Shared;
using FleetTechCore.Enums;
using FleetTechCore.Model;
using FleetTechCore.Models.Supply;

namespace FleetTechCore.DTOs.Views;

public record struct SupplyView
    (int Id,
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
        int CityId,
        string City,
        Item Status,
        List<ContactView> Contacts
    ){
        public static SupplyView From(Supplier data) => new()
        {
            Id = data.Id,
            Code = data.Code,
            CompanyName = data.CompanyName,
            RNC = data.RNC,
            Name = data.Name,
            Position = data.Position,
            Phone = data.Phone,
            Email = data.Email,
            AddressLine1 = data.Address.AddressLine1,
            AddressLine2 = data.Address.AddressLine2,
            AddressLine3 = data.Address.AddressLine3,
            CityId = data.Address.CityId,
            City = $"{data.Address.City.Name} | {data.Address.City.State.Name}",
            Contacts = data.Contacts.Select(c => ContactView.From(c)).ToList(),
            Status = new Item { Id = data.Status, Description = ((GenericStatus)data.Status).ToString() }

        };     

     };
