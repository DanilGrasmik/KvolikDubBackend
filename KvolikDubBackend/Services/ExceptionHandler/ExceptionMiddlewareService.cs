using KvolikDubBackend.Exceptions;

namespace KvolikDubBackend.Services.ExceptionHandler;

public class ExceptionMiddlewareService
{
    private readonly RequestDelegate _next;

    public ExceptionMiddlewareService(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException exception)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (BadRequestException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (ConflictException exception)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (NotAcceptableException exception)
        {
            context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (ForbiddenException exception)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (NotAuthorizedException exception)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        /*catch (Exception exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }*/
    }
}
public static class MyMiddlewareExtensions {
    public static void UseMyMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionMiddlewareService> ();
    }
}