using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.DTOs.Data;

    public record VehicleData(
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
        int       FuelType,
        decimal   FuelCapacity,
        decimal   FuelPerMonth,
        decimal   Mileage,
        string    Chassis,
        string    Engine
    );
    

