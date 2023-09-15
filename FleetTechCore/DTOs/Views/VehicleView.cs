using FleetTechCore.Models.Fleet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.DTOs.Views;

    public record struct VehicleView
    (
        string   Code,
        string   PolicyDescription,
        string   PolicyNumber,
        string   PolicyReference,
        DateTime PolicyExpiration,
        int      Status,
        int      Type,
        string   Brand,
        string   Model,
        string   Year,
        string   LicensePlate,
        string   Color,
        int      FuelType,
        decimal  FuelCapacity,
        decimal  FuelPerMonth,
        decimal  Mileage,
        string   Chassis,
        string   Engine
    ){
        public static VehicleView From(Vehicle data) => new()
        {
            Code = data.Code,
            PolicyDescription = data.PolicyDescription,
            PolicyNumber = data.PolicyNumber,
            PolicyReference = data.PolicyReference,
            PolicyExpiration = data.PolicyExpiration,
            Status = data.Status,
            Type = data.Type,
            Brand = data.Brand,
            Model = data.Model,
            Year = data.Year,
            LicensePlate = data.LicensePlate,
            Color = data.Color,
            FuelType = data.FuelType,
            FuelCapacity = data.FuelCapacity,
            FuelPerMonth = data.FuelPerMonth,
            Mileage = data.Mileage,
            Chassis = data.Chassis,
            Engine = data.Engine,
        };
    }

