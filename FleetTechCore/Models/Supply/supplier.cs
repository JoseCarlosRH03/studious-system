using FleetTechCore.Models.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.Supply
{
    public class Supplier : ServicePlace
    {
        public required string          Name            { get; set; }

        public required string          Position        { get; set; }

        public virtual ICollection<Material> Matirials  { get; set; }
    }
}
