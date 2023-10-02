using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.DTOs.Data
{
    public record struct VoucherData
    (
        int Id,
        int FuelStationId,
        int VehicleId,
        int DriverId,
        decimal Mileage,
        decimal FuelCapacity
    );
}
