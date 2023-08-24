using BackendTemplateCore;
using BackendTemplateCore.Errors;
using BackendTemplateCore.Models;
using BackendTemplateCore.Services.Model_Related_Services;

namespace BackendTemplateAPI;

public class Context {
   public Context(Logic logic, IHttpContextAccessor context, IAuditService auditService) {
      Logic = logic;
      ContextAccessor = context;
      AuditService = auditService;
   }

   readonly Logic Logic;
   readonly IHttpContextAccessor ContextAccessor;
   readonly IAuditService AuditService;
   
   public HttpRequest  Request  => ContextAccessor.HttpContext!.Request;
   HttpResponse Response => ContextAccessor.HttpContext!.Response;

   void SetStatusCode(int status_code) => Response.StatusCode = status_code;
   T ReturnWithStatusCode<T>(T data, int status_code) {
      SetStatusCode(status_code);
      return data;
   }

   public record struct ErrorView(string code, object? message);
   T Ok<T>(T data)            => ReturnWithStatusCode(data, StatusCodes.Status200OK);
   ErrorView BadRequest<T>(T data)    => ReturnWithStatusCode(new ErrorView("BAD_REQUEST", data), StatusCodes.Status400BadRequest);
   ErrorView NotFound<T>(T data)      => ReturnWithStatusCode(new ErrorView("NOT_FOUND", data), StatusCodes.Status404NotFound);
   ErrorView Unauthorized<T>(T data)  => ReturnWithStatusCode(new ErrorView("UNAUTHORIZED", data), StatusCodes.Status401Unauthorized);
   ErrorView InternalError<T>(T data) => ReturnWithStatusCode(new ErrorView("INTERNAL_ERROR", data), StatusCodes.Status500InternalServerError);

   public async Task<object?> Execute<T>(Func<Logic, Task<T>> procedure) => await WrapResponse(() => procedure(Logic));
   public Task<object?>      Execute(Func<Logic, Task> procedure)       => WrapResponse<object?>(async () => {
      await procedure(Logic);
      return null;
   });

   public async Task<object> ExecuteAuthenticated<T>(Func<User, Logic, Task<T>> procedure, PermissionAreas? area = null, PermissionTypes? type = null) => await WrapResponse(async () => {
      var token = Request.Cookies["Auth"];
      var user = await Logic.Authenticate(token?.Split(' ')?.Last());
      AuditService.SetCurrentUser(user);
      
      if (area != null && type != null && !user.HasPermission(area.Value, type.Value))
            throw new NotAuthorized("El usuario no tiene permisos para realizar esta acción \n" 
                                    + $"Permiso requerido es {area.ToString().PascalCaseWithInitialsToTitleCase()} - {type.ToString().PascalCaseWithInitialsToTitleCase()}");

      return await procedure(user, Logic);
   });
   public Task<object> ExecuteAuthenticated(Func<User, Logic, Task> procedure, PermissionAreas? area = null, PermissionTypes? type = null) => ExecuteAuthenticated<object?>(async (user, logic) => {
      await procedure(user, logic);
      return null;
   });
   
   public async Task<object> ExecuteAPI<T>(Func<User, Logic, Task<T>> procedure, PermissionAreas? area = null, PermissionTypes? type = null) => await WrapResponse(async () => {
      //Authenticate with bearer token
      var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
      var user = await Logic.Authenticate(token);
      AuditService.SetCurrentUser(user);
      
      if (area != null && type != null && !user.HasPermission(area.Value, type.Value))
         throw new NotAuthorized("El usuario no tiene permisos para realizar esta acción \n" 
                                 + $"Permiso requerido es {area.ToString().PascalCaseWithInitialsToTitleCase()} - {type.ToString().PascalCaseWithInitialsToTitleCase()}");
      
      return await procedure(user, Logic);
   });
   
   public Task<object> ExecuteAPI(Func<User, Logic, Task> procedure, PermissionAreas? area = null, PermissionTypes? type = null) => ExecuteAPI<object?>(async (user, logic) => {
      await procedure(user, logic);
      return null;
   });

   async Task<object?> WrapResponse<T>(Func<Task<T>> function) {
      try {
         var data = await function();
         if (data is Task)
            throw new Exception("Endpoint returned Task not awaited.  \nCheck endpoint body in API controller; await the task.");
         return Ok(data);
      } catch (Exception ex) {
         var uex = ex.Unwrap();
         if (uex is Error error) {
            if (error is NotFound)
               return NotFound(error.Message);
            if (error is NotAuthenticated or NotAuthorized)
               return Unauthorized(error.Message);
            return BadRequest(error.Message);
         } else {
            LogService.LogException(ex);
            return InternalError(uex.Message);
         }
      }

   }
}
