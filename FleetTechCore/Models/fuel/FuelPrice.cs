using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.Fuel
{
    public class FuelPrice: AuditableEntity
    {
        public required int          FuelTypeId    { get; set; }
        public required DateTime     DateFrom      { get; set; }
        public required DateTime     DateTo        { get; set; }
        public required decimal      Price         { get; set; }
        public required int         Status         { get; set; }

        public virtual FuelType      FuelType      { get; set; }

    }
}
