using FleetTechCore.DTOs.Shared;
using FleetTechCore.Enums;
using FleetTechCore.Models.Fleet;

namespace FleetTechCore.DTOs.Views;

public record struct VehicleView
    (
        int      Id, 
        string   Code,
        string   PolicyDescription,
        string   PolicyNumber,
        string   PolicyReference,
        DateTime PolicyExpiration,
        Item     Status,
        Item     Type,
        string   Brand,
        string   Model,
        string   Year,
        string   LicensePlate,
        string   Color,
        Item     FuelType,
        decimal  FuelCapacity,
        decimal  FuelPerMonth,
        decimal  Mileage,
        string   Chassis,
        string   Engine
    ){
        public static VehicleView From(Vehicle data) => new()
        {   
            Id = data.Id,
            Code = data.Code,
            PolicyDescription = data.PolicyDescription,
            PolicyNumber = data.PolicyNumber,
            PolicyReference = data.PolicyReference,
            PolicyExpiration = data.PolicyExpiration,
            Status = new Item { Id = data.Status, Description = ((VehicleState)data.Status).ToString() },
            Type = new Item { Id = data.Type, Description = ((VehicleType)data.Type).ToString() },
            Brand = data.Brand,
            Model = data.Model,
            Year = data.Year,
            LicensePlate = data.LicensePlate,
            Color = data.Color,
            FuelType = data.FuelType is null? new Item() : new Item { Id = data.FuelType.Id, Description = data.FuelType.Name },
            FuelCapacity = data.FuelCapacity,
            FuelPerMonth = data.FuelPerMonth,
            Mileage = data.Mileage,
            Chassis = data.Chassis,
            Engine = data.Engine,
        };
    }

