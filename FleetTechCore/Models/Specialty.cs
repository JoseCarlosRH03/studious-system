using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models
{
    public class Specialty
    {
        public int Id { get; set; }
        public int? MechanicalWorkshopId { get; set; }
        public int? MechanicId { get; set; }

        public required string Description { get; set; }
    }
}
