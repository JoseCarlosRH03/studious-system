using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Shared;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using static FleetTechAPI.Extensions;

namespace FleetTechAPI.Routes;

public static class FleetManagement
{
    public static void MapFleetManagement(this WebApplication app)
    {
        Tagged("Manejo de Conductores y Vehículos", new[]
        {
            app.MapGet("/driver", (int start, int count, string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetAllDrivers(start, count, filter)))
                .Produces<List<DriverView>>(),
            app.MapPost("/driver", (DriverData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.CreateDriver(data)))
                .Produces<int>(),
            app.MapPut("/driver/{id:int}", (int id, DriverData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.UpdateDriver(id, data)))
                .Produces<int>(),
            app.MapDelete("/driver/{id:int}", (int id, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.InactiveDriver(id)))
                .Produces<int>(),
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

            app.MapGet("/vehicles", (Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetAllVehicle()))
                .Produces<List<VehicleView>>(),

            app.MapGet("/vehicle/{id:int}",( int Id ,Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetVehicleById(Id)))
                .Produces<List<VehicleView>>(),
        }); 
    }
}