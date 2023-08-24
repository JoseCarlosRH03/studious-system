using Microsoft.AspNetCore.Mvc;
using static BackendTemplateAPI.Extensions;
namespace BackendTemplateAPI.Routes;

public static class WorkOrderManagement
{
    public static void MapWorkOrderManagement(this WebApplication app)
    {
        Tagged("Ordenes de trabajo", new[]
        {
            app.MapGet("/work_order", (int start, int count, string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetWorkOrders(start, count, filter), PermissionAreas.WorkOrders, PermissionTypes.Read))
                .Produces<List<WorkOrderView>>(),
            app.MapGet("/work_order" + "/count", (string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetWorkOrderCount(filter), PermissionAreas.WorkOrders, PermissionTypes.Read))
                .Produces<int>(),
            app.MapGet("/work_order" + "/{id:int}", (int id, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetWorkOrder(id), PermissionAreas.WorkOrders, PermissionTypes.Read))
                .Produces<WorkOrderView>(),
            app.MapPost("/work_order", (WorkOrderData workOrder, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.CreateWorkOrder(workOrder), PermissionAreas.WorkOrders, PermissionTypes.Create))
                .Produces<int>(),
            app.MapDelete("/work_order/{id:int}", (int id, [FromBody]DescriptionData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.UpdateWorkOrderStatus(user, id, (int)WorkOrderStatuses.Rechazada, data.Description), PermissionAreas.WorkOrders, PermissionTypes.Delete))
                .Produces<int>(),
            app.MapPut("/work_order/{id:int}", (int id, WorkOrderData workOrder, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.UpdateWorkOrder(id, workOrder), PermissionAreas.WorkOrders, PermissionTypes.Update))
                .Produces<int>(),
            
            app.MapGet("/work_order_type", (int start, int count, string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetWorkOrderTypes(start, count, filter), PermissionAreas.WorkOrderTypes, PermissionTypes.Read))
                .Produces<List<WorkOrderTypeView>>(),
            app.MapGet("/work_order_type" + "/count", (string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetWorkOrderTypeCount(filter), PermissionAreas.WorkOrderTypes, PermissionTypes.Read))
                .Produces<int>(),
            app.MapPost("/work_order_type", (WorkOrderTypeData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.CreateWorkOrderType(data), PermissionAreas.WorkOrderTypes, PermissionTypes.Create))
                .Produces<int>(),
            app.MapPut("/work_order_type/{id:int}", (int id, WorkOrderTypeData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.UpdateWorkOrderType(id, data), PermissionAreas.WorkOrderTypes, PermissionTypes.Update))
                .Produces<int>(),
            app.MapDelete("/work_order_type/{id:int}", (int id, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.UpdateWorkOrderTypeStatus(id, (int)GenericStatus.Inactivo), PermissionAreas.WorkOrderTypes, PermissionTypes.Delete))
                .Produces<int>(),
            
            app.MapGet("/meter", (int start, int count, string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetMeters(start, count, filter), PermissionAreas.Meters, PermissionTypes.Read))
                .Produces<List<MeterView>>(),
            app.MapGet("/meter" + "/count", (string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetMeterCount(filter), PermissionAreas.Meters, PermissionTypes.Read))
                .Produces<int>(),
            app.MapGet("/meter" + "/{id:int}", (int id, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetMeter(id), PermissionAreas.Meters, PermissionTypes.Read))
                .Produces<MeterView>(),
            app.MapPost("/meter", (MeterData meter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.CreateMeter(meter), PermissionAreas.Meters, PermissionTypes.Create))
                .Produces<int>(),
            app.MapDelete("/meter/{id:int}", (int id, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.DeleteMeter(id), PermissionAreas.Meters, PermissionTypes.Delete))
                .Produces<int>(),
            
            app.MapGet("/meter-query", (string? number, string? subscription, string? client_id, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetMeterQuery(number, subscription, client_id.GetInt()), PermissionAreas.Meters, PermissionTypes.Read))
                .Produces<List<MeterQueryView>>(),
            
            app.MapGet("/meter_model", (string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetMeterModels(filter), PermissionAreas.MeterModels, PermissionTypes.Read))
                .Produces<List<MeterModelView>>(),
            app.MapGet("/meter_model" + "/count", (string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetMeterModelCount(filter), PermissionAreas.MeterModels, PermissionTypes.Read))
                .Produces<int>(),
            app.MapPost("/meter_model", (MeterModelData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.CreateMeterModel(data), PermissionAreas.MeterModels, PermissionTypes.Create))
                .Produces<int>(),
            app.MapPut("/meter_model/{id:int}", (int id, MeterModelData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.UpdateMeterModel(id, data), PermissionAreas.MeterModels, PermissionTypes.Update))
                .Produces<int>(),
            app.MapDelete("/meter_model/{id:int}", (int id, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.UpdateMeterModelStatus(id, (int)GenericStatus.Inactivo), PermissionAreas.MeterModels, PermissionTypes.Delete))
                .Produces<int>(),
        });
    }
}