using FleetTechCore.Models.Fleet;
using FleetTechCore.Models.Fuel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.fuel
{
    public class FuelGestion: AuditableEntity
    {
        public          int        Status        { get; set; }
        public required decimal    Mileage       { get; set; }
        public required decimal    FuelCapacity  { get; set; }
        public required int        VehicleId     { get; set; }
        public required int        DriveId       { get; set; }
        public required int        FuelStationId { get; set; }
        public virtual Vehicle     Vehicle       { get; set; }
        public virtual Driver      Driver        { get; set; }
        public virtual FuelStation FuelStation   { get; set; }
    }
}
