using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.fuel
{
    public class FuelGestion: AuditableEntity
    {
        public required int     Vehicle       { get; set; }
        public required int     Driver        { get; set; }
        public required int     FuelStation   { get; set; }
        public          int     Status        { get; set; }
        public required decimal Mileage       { get; set; }
        public required decimal FuelCapacity  { get; set; }
    }
}
