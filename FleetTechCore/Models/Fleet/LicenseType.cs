using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.Fleet
{
    public class LicenseType
    {
        public           int    Id { get; set; }
        public required  string Description { get; set; }
    }
}
