using FleetTechCore.Models.Fleet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.WorkShop
{
    public class MaintenanceScheduling:AuditableEntity
    {
        public decimal Mileage      { get; set; }
        public decimal Description  { get; set; }
        public int     Status       { get; set; }
        public int     VehicleId    { get; set; } 

        public virtual Vehicle Vehicle { get; set; }
    }
}
