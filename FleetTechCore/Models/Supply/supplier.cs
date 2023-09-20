using FleetTechCore.Models.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.Supply
{
    public class Supplier: AuditableEntity
    {
        public string                   code            { get; set; }
        public required string          name            { get; set; }
        public required string          CompanyName     { get; set; }
        public required string          RNC             { get; set; }
        public required string          Position        { get; set; }
        public required string          Phone           { get; set; }
        public string                   Email           { get; set; }
        public virtual List<Contact>    Contacts        { get; set; }
    }
}
