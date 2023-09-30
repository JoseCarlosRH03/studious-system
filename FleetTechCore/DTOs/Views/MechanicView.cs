using FleetTechCore.DTOs.Shared;
using FleetTechCore.Enums;
using FleetTechCore.Models.WorkShop;

namespace FleetTechCore.DTOs.Views;

public record struct MechanicView
    (   
        int Id,
        string Code,
        string Phone,
        string Email,
        string Name,
        Item Status,
        string JobTitle,
        List<Item> Specialties
    ){
        public static MechanicView From(Mechanic data) => new()
        {
            Id = data.Id,
            Phone = data.Phone,
            Name = data.Name,
            Code = data.Code,
            Email = data.Email,
            JobTitle = data.JobTitle,
            Specialties = data.Specialties.Select(s => new Item { Description = s.Description, Id = s.Id}).ToList(),
            Status = new Item { Id = data.Status, Description = ((GenericStatus)data.Status).ToString() }

        };     

     };
