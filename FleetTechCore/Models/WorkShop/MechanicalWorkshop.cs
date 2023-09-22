using FleetTechCore.Models.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.WorkShop
{
    public class MechanicalWorkshop:ServicePlace
    {
        public virtual ICollection<WorksopSpecialty> Specialties { get; set; }
    }
}
