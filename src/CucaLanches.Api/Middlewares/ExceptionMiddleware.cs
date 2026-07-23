using CucaLanches.Application.Exceptions;

namespace CucaLanches.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {

            switch (ex)
            {
                case NotFoundException notFoundException: 
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                   await context.Response.WriteAsJsonAsync(new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = notFoundException.Message
                    });
                    break;
                
                case ValidationException validationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = validationException.Message,
                        Error = validationException.Errors

                    });
                    break;
                
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = "Internal server error",
                    });
                    break;
            }
        }
       
    }
    
}