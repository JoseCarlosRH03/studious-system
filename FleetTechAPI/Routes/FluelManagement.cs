using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Shared;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Enums;
using static FleetTechAPI.Extensions;

namespace FleetTechAPI.Routes;

public static class FluelManagement
{
    public static void MapFluelManagement(this WebApplication app)
    {
        Tagged("Manejo de Combustible", new[]
        {
            app.MapGet("/fuelType",(Context ctx) => ctx.Execute(
                (logic) => logic.GetAllFuelType())).Produces<List<Item>>(),
        }); ;
    }
}