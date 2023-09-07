﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.WorkShop
{
    public class MechanicSpecialty
    {
        public required int     Id          { get; set; }
        public required int     Code        { get; set; }
        public required string  Description { get; set; }
    }
}
