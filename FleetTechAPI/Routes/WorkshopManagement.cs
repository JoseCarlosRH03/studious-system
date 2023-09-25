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
            .Produces<List<MechanicalWorkshopView>>(),
            app.MapGet("/mechanical/workshop/{id:int}",(int Id, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.GetMechanicalWorkshopById(Id)))
            .Produces<MechanicalWorkshopView>(),
           app.MapDelete("/mechanical/workshop/{id:int}",(int Id, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.DeleteMechanicalWorkshop(Id, user))).
                Produces<int>(),
           
        }); ;
    }
}