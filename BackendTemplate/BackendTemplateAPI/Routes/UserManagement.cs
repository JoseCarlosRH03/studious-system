using BackendTemplateCore.DTOs.Data;
using BackendTemplateCore.DTOs.Views;
using BackendTemplateCore.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static BackendTemplateAPI.Extensions;

namespace BackendTemplateAPI.Routes;

public static class UserManagement
{
    public static void MapUserManagement(this WebApplication app)
    {
        Tagged("Manejo de usuarios", new []
        {
            app.MapPost("/login", [SwaggerOperation("Obtener token")]
                ([FromBody]LoginParameters login, Context ctx, HttpResponse res) => ctx.Execute(async logic => {
                var (User, Token) = await logic.Login(login.email, login.password);
                res.Cookies.Append("Auth", Token, new CookieOptions {  MaxAge = TimeSpan.FromDays(7) });
                return User;
            })),
            app.MapPost("/forgot-password", [SwaggerOperation("Reestablecer contraseña")]
                (Guid userId, string password, Context ctx, HttpRequest req) => ctx.Execute(
                logic => logic.ChangePassword(userId, password))),
            
            app.MapGet("/permission", (Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetAllPermissions(), PermissionAreas.Permissions, PermissionTypes.Read))
                .Produces<List<PermissionView>>(),
            
            app.MapGet("/role", (int start, int count, string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetRoles(start, count, filter), PermissionAreas.Roles, PermissionTypes.Read))
                .Produces<List<RoleView>>(),
            app.MapGet("/role" + "/count", (string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetRoleCount(filter), PermissionAreas.Roles, PermissionTypes.Read))
                .Produces<int>(),
            app.MapPost("/role", (RoleData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.CreateRole(data), PermissionAreas.Roles, PermissionTypes.Create))
                .Produces<int>(),
            app.MapPut("/role/{id:int}", (int id, RoleData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.UpdateRole(id, data), PermissionAreas.Roles, PermissionTypes.Update))
                .Produces<int>(),
            app.MapDelete("/role/{id:int}", (int id, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.DeleteRole(id), PermissionAreas.Roles, PermissionTypes.Delete))
                .Produces<int>(),
            
            app.MapGet("/user", (int start, int count, string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetUsers(start, count, filter), PermissionAreas.Users, PermissionTypes.Read))
                .Produces<List<UserView>>(),
            app.MapGet("/user" + "/count", (string? filter, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.GetUserCount(filter), PermissionAreas.Users, PermissionTypes.Read))
                .Produces<int>(),
            app.MapPost("/user", (UserData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.CreateUser(data), PermissionAreas.Users, PermissionTypes.Create))
                .Produces<Guid>(),
            app.MapPut("/user/{id:guid}", (Guid id, UserUpdateData data, Context ctx, HttpRequest req) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.UpdateUser(id, data, req.Headers["Origin"]), PermissionAreas.Users, PermissionTypes.Update))
                .Produces<Guid>(),
            app.MapPut("/user/{id:guid}/picture", (Guid id, UserUpdatePictureData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.UpdateUserProfilePicture(id, data), PermissionAreas.Users, PermissionTypes.Update))
                .Produces<Guid>(),
            app.MapPut("/user/{id:guid}/role", (Guid id, UserRoleData data, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.UpdateUserRoles(id, data), PermissionAreas.Users, PermissionTypes.Update))
                .Produces<Guid>(),
            app.MapPut("/user/{id:guid}/status", (Guid id, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.ActivateUser(id), PermissionAreas.Users, PermissionTypes.Update))
                .Produces<Guid>(),
            app.MapDelete("/user/{id:guid}", (Guid id, Context ctx) => ctx.ExecuteAuthenticated(
                (user, logic) => logic.DeactivateUser(id), PermissionAreas.Users, PermissionTypes.Delete))
                .Produces<Guid>(),
        });
    }
}