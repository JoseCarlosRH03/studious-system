﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.DTOs.Data
{
    public record struct FuelPriceData
    (
        int         Id,
        int         FuelTypeId,
        DateTime    DateStart,
        DateTime    DateEnd,
        decimal     Price
    );
}
