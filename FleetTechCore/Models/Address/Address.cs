using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.Address
{
    public class Address:AuditableEntity
    {
        // Geolocation
        public double   Latitude    { get; set; }
        public double   Longitude   { get; set; }

        // Address
        public required     string      PlainAddress   { get; set; }
        public required     string      AddressLine1   { get; set; }
        public              string      AddressLine2   { get; set; }
        public              string?     AddressLine3   { get; set; }

        public int CityId { get; set; }

        public virtual City City { get; set; }

    }
}
