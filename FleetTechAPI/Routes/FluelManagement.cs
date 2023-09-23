using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Shared;
using FleetTechCore.DTOs.Views;
using static FleetTechAPI.Extensions;

namespace FleetTechAPI.Routes;

public static class FluelManagement
{
    public static void MapFluelManagement(this WebApplication app)
    {
        Tagged("Manejo de Combustible", new[]
        {
            app.MapGet("/fuel/type",(Context ctx) => ctx.Execute(
                (logic) => logic.GetAllFuelType())).Produces<List<Item>>(),
            app.MapPost("station",(ServicePlaseData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.CreateStation(data,user))),
            app.MapGet("station",( Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.GetAllFuelSation()))
        }); ;
    }
}