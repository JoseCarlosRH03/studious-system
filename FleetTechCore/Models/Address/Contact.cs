﻿using FleetTechCore.Models.Fuel;

namespace FleetTechCore.Models.Address
{
    public class Contact
    {
        public          int     Id                    { get; set; }
        public required string  Name                  { get; set; }
        public required string  Telephone             { get; set; }
        public          string? Email                { get; set; }
        public          int?   FuelStationId         { get; set; }
        public          int?   MechanicalWorkshopId  { get; set; }
        public          int?   SupplierId            { get; set; }

    }
}
