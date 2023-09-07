using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.Supply
{
    public class Inventory: AuditableEntity
    {
        public required int     Supplier        { get; set; }
        public required int     Quantity        { get; set; }
        public required int     UnitCost        { get; set; }
        public          int     Material        { get; set; }
    }
}
