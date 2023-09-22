﻿using FleetTechCore.Models.Fleet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.Shared
{
    public class StorageFile: AuditableEntity
    {
        public string       Name                 { get; set; }
        public string       FileName             { get; set; }
        public int          ContentTypeId        { get; set; }
        public long         FileSize             { get; set; }
        public string       File                 { get; set; }
    }
}
