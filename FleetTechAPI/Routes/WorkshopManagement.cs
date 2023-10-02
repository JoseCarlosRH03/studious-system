using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Shared;
using FleetTechCore.DTOs.Views;
using static FleetTechAPI.Extensions;

namespace FleetTechAPI.Routes;

public static class WorkshopManagement
{
    public static void MapWorkshopManagement(this WebApplication app)
    {
        Tagged("Manejo de Taller", new[]
        {
            app.MapPost("/mechanical/workshop",(MechanicalWorkshopData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.CreateMechanicalWorkshop(data,user)))
            .Produces<int>(),
             app.MapPut("/mechanical/workshop",(MechanicalWorkshopData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.UpdateMechanicalWorkshop(data,user)))
            .Produces<int>(),
            app.MapGet("/mechanical/workshop",( Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.GetAllMechanicalWorkshop()))
            .Produces<List<MechanicData>>(),
            app.MapGet("/mechanical/workshop/{id:int}",(int Id, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.GetMechanicalWorkshopById(Id)))
            .Produces<MechanicData>(),
           app.MapDelete("/mechanical/workshop/{id:int}",(int Id, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.DeleteMechanicalWorkshop(Id, user))).
                Produces<int>(),

           app.MapPost("/mechanic",(MechanicData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.CreateMechanic(data,user)))
            .Produces<int>(),
             app.MapPut("/mechanic",(MechanicData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.UpdateMechanic(data,user)))
            .Produces<int>(),
            app.MapGet("/mechanic",( Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.GetAllMechanic()))
            .Produces<List<MechanicData>>(),
            app.MapGet("/mechanic/{id:int}",(int Id, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.GetMechanicById(Id)))
            .Produces<MechanicData>(),
           app.MapDelete("/mechanic/{id:int}",(int Id, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.DeleteMechanic(Id, user))).
                Produces<int>(),
        }); 
    }
}