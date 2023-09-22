using FleetTechCore.Models.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models
{
    public class ServicePlace:AuditableEntity
    {
        public          string          Code        { get; set; }
        public required string          CompanyName { get; set; }
        public required string          RNC         { get; set; }
        public required string          Phone       { get; set; }
        public          string          Email       { get; set; }
        public          int             AddressId   { get; set; }
        
        
        public virtual Address.Address Address  { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
