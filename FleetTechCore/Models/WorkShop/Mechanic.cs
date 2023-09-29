using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.WorkShop
{
    public class Mechanic: AuditableEntity
    {
        public              string    Code      { get; set; }
        public required     string    Name      { get; set; }
        public required     string    JobTitle  { get; set; }
        public              string    Email     { get; set; }
        public              string    Phone     { get; set; }
        public              int       Status    { get; set; }

        public virtual List<Specialty> Specialties { get; set; }

    }
}
