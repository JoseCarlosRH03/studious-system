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
            app.MapPost("/station",(ServicePlaseData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.CreateStation(data,user)))
            .Produces<int>(),
             app.MapPut("/station",(ServicePlaseData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.UpdateStation(data,user)))
            .Produces<int>(),
            app.MapGet("/fuel/station",( Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.GetAllFuelSation()))
            .Produces<List<ServicePlaseView>>(),
            app.MapGet("/fuel/station/{id:int}",(int Id, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.GetFuelSationById(Id)))
            .Produces<ServicePlaseView>(),
            app.MapGet("/fuel/price", (Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetAllPrice()))
            .Produces<List<PriceView>>(),
            app.MapGet("fuel/price/{id: int}", (int Id, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetFuelPriceById(Id)))
            .Produces<PriceView>(),
            app.MapPost("/fuel/price", (FuelPriceData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.CreateFuelPrice(data, user)))
            .Produces<int>(),
            app.MapPut("fuel/price/{id:int}", (int id, FuelPriceData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.UpdateFuelPrice(id, data, user)))
            .Produces<int>(),
            app.MapDelete("fuel/price/{id:int}", (int id, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.InactiveFuelPrice(id, user)))
            .Produces<int>(),
        }); ;
    }
}