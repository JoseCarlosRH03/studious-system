using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.Fuel
{
    public class FuelPrice: AuditableEntity
    {
        public required int     FuelType    { get; set; }
        public required int     DateFrom    { get; set; }
        public required int     DateTo      { get; set; }
        public required decimal Price       { get; set; }
    }
}
