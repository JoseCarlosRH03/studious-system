using FleetTechCore.Models.Shared;
using static FleetTechAPI.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FleetTechAPI.Routes;

public static class Reports
{
    public static void MapReports(this WebApplication app)
    {
        Tagged("Reportes", new[]
        {

             app.MapGet("/document/{id:int}", (int id, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
             async (user, logic) =>
             {
                     var stream =  await logic.GetStorageFile(id);

                     res.Headers.Add("Content-Disposition", $"inline; filename=\"{stream.name}{stream.mimetype}\"");
                     return Results.File(stream.data, $"application/pdf");
             })).Produces(200, null, "application/pdf"),
        });


    }
}