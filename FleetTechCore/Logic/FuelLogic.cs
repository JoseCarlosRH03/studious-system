using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Shared;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Errors;
using FleetTechCore.Models.Company;
using FleetTechCore.Models.Fleet;
using FleetTechCore.Models.Fuel;
using Microsoft.VisualBasic.FileIO;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace FleetTechCore.Logic;

public partial class Logic
{

    public async Task<List<Item>> GetAllFuelType () => (await Data.GetAll<FuelType>())
        .Select(f => new Item(f.Id,f.Name)).ToList() ?? throw new NotFound("No se encontro ningun tipo de combustible");
   
}