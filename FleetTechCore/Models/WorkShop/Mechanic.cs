using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.WorkShop
{
    public class Mechanic: AuditableEntity
    {
        public int Code { get; set; }
        public required string Name { get; set; }
        public required string JobTitle { get; set; }
        public string Email { get; set; }

        public virtual List<MechanicSpecialty> Specialty { get; set; }

    }
}
