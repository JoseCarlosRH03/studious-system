using FleetTechCore.DTOs.Shared;
using FleetTechCore.Models.fuel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.DTOs.Views
{
    public record struct VoucherView
    (
        int Id,
        Item FuelStation,
        Item Vehicle,
        Item Driver,
        decimal Mileage,
        decimal FuelCapacity
    )
    {
        public static VoucherView From(FuelGestion data) => new()
        {
            Id = data.Id,
            FuelStation = new Item { Id = data.FuelStation.Id, Description = data.FuelStation.CompanyName },
            Vehicle = new Item { Id = data.Vehicle.Id, Description = $"{data.Vehicle.Brand} {data.Vehicle.Model} - {data.Vehicle.LicensePlate}" },
            Driver = new Item { Id = data.Driver.Id, Description = data.Driver.FirstName },
            Mileage = data.Mileage,
            FuelCapacity = data.FuelCapacity
        };

    }
}
