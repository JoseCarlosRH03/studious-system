using FleetTechCore.Models.Address;

namespace FleetTechCore.DTOs.Views;

public record struct ContactView
    (
        int Id,
        string Name,
        string Phone,
        string Email
    ){
        public static ContactView From(Contact data) => new() { 
            Email = data.Email, 
            Name = data.Name,
            Id = data.Id,
            Phone = data.Telephone,
        };
    };

