﻿using FleetTechCore.Models.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.fuel
{
    public class FuelStation: AuditableEntity
    {
        public          string Code          { get; set; }
        public required string CompanyName   { get; set; }
        public required string RNC           { get; set; }
        public required string Phone         { get; set; }
        public          string Email         { get; set; }
        public          string Address       { get; set; }

        public virtual List<Contact> Contacts { get; set; }
    }
}