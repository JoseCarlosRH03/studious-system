using BackendTemplateCore;

namespace BackendTemplateAPI.Middlewares;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    
    public LoggerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try {
            await _next(context);
        } catch (Exception ex) {
            LogService.LogException(ex);
            throw;
        }
    }
}