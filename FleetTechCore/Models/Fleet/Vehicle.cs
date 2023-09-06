using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FleetTechCore.Models.Fleet
{
    public class Vehicle: AuditableEntity
    {
       public required string   Code                    { get; set; }
       public required string   PolicyDescription       { get; set; }
       public required string   PolicyNumber            { get; set; }
       public required string   PolicyReference         { get; set; }
       public required DateTime PolicyExpiration        { get; set; }
       public required int      Status                  { get; set; }
       public required int      Type                    { get; set; }
       public required string   Brand                   { get; set; }
       public required string   Model                   { get; set; }
       public required string   Year                    { get; set; }
       public required string   LicensePlate            { get; set; }
       public required string   Color                   { get; set; }
       public required int      FuelType                { get; set; }
       public required decimal  FuelCapacity            { get; set; }
       public required decimal  FuelPerMonth            { get; set; }
       public required decimal  Mileage                 { get; set; }
       public required string   Chassis                 { get; set; }
       public required string   Engine                  { get; set; }

    }
}
