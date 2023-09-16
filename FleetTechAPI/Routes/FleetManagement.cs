using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Shared;
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
            app.MapGet("/drives", (Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetAllPermissions()))
                .Produces<List<PermissionView>>(),

            app.MapGet("/vehicle/state", (Context ctx) => ctx.Execute(
                (logic) => logic.GetAllVehicleState()))
                .Produces<List<Item>>(),

            app.MapGet("/vehicle/type", (Context ctx) => ctx.Execute(
                (logic) => logic.GetAllVehicleType()))
                .Produces<List<Item>>(),

            app.MapPost("/vehicle", (VehicleData data,Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.CreateVehicle(data)))
                .Produces<int>(),

            app.MapGet("/license/type",(Context ctx) => ctx.Execute(
                (logic) => logic.GetAllLicenseType())).Produces<List<Item>>(),
        }); ;
    }
}