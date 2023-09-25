using FleetTechCore.Models;
using FleetTechCore.Models.Address;

namespace FleetTechCore.Model;

    public class ServicePlace:AuditableEntity
    {
        public          string          Code        { get; set; }
        public required string          CompanyName { get; set; }
        public required string          RNC         { get; set; }
        public required string          Phone       { get; set; }
        public          string          Email       { get; set; }
        public          int             AddressId   { get; set; }
        public          int             Status      {  get; set; } = 1;


    public virtual Address Address  { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }

