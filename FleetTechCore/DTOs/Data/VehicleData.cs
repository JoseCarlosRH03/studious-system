using FleetTechCore.Models.Fleet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.DTOs.Data;

    public record struct VehicleData(
        string?   Code,
        string    PolicyDescription,
        string    PolicyNumber,
        string    PolicyReference,
        DateTime  PolicyExpiration,
        int       Status,
        int       Type,
        string    Brand,
        string    Model,
        string    Year,
        string    LicensePlate,
        string    Color,
        int       FuelTypeId,
        int   FuelCapacity,
        int FuelPerMonth,
        int Mileage,
        string    Chassis,
        string    Engine
    );