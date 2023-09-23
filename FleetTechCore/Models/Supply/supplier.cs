using FleetTechCore.Model;

namespace FleetTechCore.Models.Supply
{
    public class Supplier : ServicePlace
    {
        public required string          Name            { get; set; }

        public required string          Position        { get; set; }

        public virtual ICollection<Material> Matirials  { get; set; }
    }
}
