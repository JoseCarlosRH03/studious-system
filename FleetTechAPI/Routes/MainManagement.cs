using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Shared;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using static FleetTechAPI.Extensions;

namespace FleetTechAPI.Routes;

public static class MainManagement
{
    public static void MapMainManagement(this WebApplication app)
    {
        Tagged("Consultas generales", new[]
        {
            app.MapGet("/license/type",(Context ctx) => ctx.Execute(
                (logic) => logic.GetAllLicenseType())).Produces<List<Item>>(),
            app.MapGet("/cities", (int start, int count, string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetCities(start, count, filter))).Produces<List<CityView>>(),

        }); 
    }
}