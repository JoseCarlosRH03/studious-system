using FleetTechCore.DTOs.Shared;
using FleetTechCore.Enums;
using FleetTechCore.Models.Fuel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.DTOs.Views
{
    public record struct PriceView
    (
        int         Id,
        Item        FuelType,
        DateTime    DateStart,
        DateTime    DateEnd,
        decimal     Price
    )
    {
        public static PriceView From(FuelPrice data) => new()
        {
            Id = data.Id,
            FuelType = data.FuelType is null ? new Item() : new Item { Id = data.FuelType.Id, Description = data.FuelType.Name },
            DateStart = data.DateFrom,
            DateEnd = data.DateTo,
            Price = data.Price
        };
    }
}
