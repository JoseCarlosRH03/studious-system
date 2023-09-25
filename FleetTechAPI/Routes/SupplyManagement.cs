using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Shared;
using FleetTechCore.DTOs.Views;
using static FleetTechAPI.Extensions;

namespace FleetTechAPI.Routes;

public static class SupplyManagement
{
    public static void MapSupplyManagement(this WebApplication app)
    {
        Tagged("Manejo de Suministros", new[]
        {
            app.MapPost("/supply",(SupplyData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.CreateSupply(data,user)))
            .Produces<int>(),
             app.MapPut("/supply",(SupplyData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.UpdateSupply(data,user)))
            .Produces<int>(),
            app.MapGet("/supply",( Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.GetAllSupply()))
            .Produces<List<SupplyView>>(),
            app.MapGet("/supply/{id:int}",(int Id, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.GetSupplyById(Id)))
            .Produces<SupplyView>(),
           app.MapDelete("/supply/{id:int}",(int Id, Context ctx) => ctx.ExecuteAuthenticated(
                (user ,logic) => logic.DeleteSupply(Id, user))).
                Produces<int>(),
           
        }); ;
    }
}