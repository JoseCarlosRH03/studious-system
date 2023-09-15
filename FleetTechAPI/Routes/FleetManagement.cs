using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Enums;
using static FleetTechAPI.Extensions;

namespace FleetTechAPI.Routes;

public static class FleetManagement
{
    public static void MapFleetManagement(this WebApplication app)
    {
        Tagged("Manejo de Conductores y Vehículos", new[]
        {
            app.MapGet("/conductores", (Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetAllPermissions()))
                .Produces<List<PermissionView>>(),

            app.MapPost("/Vehicle", (VehicleData data,Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.CreateVehicle(data)))
                .Produces<int>()
        }); ;
    }
}