using System.Net;
using System.Text.Json;
using FluentValidation;

namespace Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {    
            await _next(context);
        }
        catch (Exception ex)
        {
            
            _logger.LogError(ex, "Unhandled exception occurred during request processing");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        
        HttpStatusCode code = HttpStatusCode.InternalServerError;
        string result = JsonSerializer.Serialize(new { error = "Internal Server Error" });


        switch (exception)
        {
            case ValidationException validationException:

                code = HttpStatusCode.BadRequest;
                var errors = validationException.Errors.Select(err => new { field = err.PropertyName, message = err.ErrorMessage }).ToList();
                result = JsonSerializer.Serialize(new { errors = errors, message = "Validation Failed" });
                break;

            case UnauthorizedAccessException:
                code = HttpStatusCode.Unauthorized;
                result = JsonSerializer.Serialize(new { message = exception.Message });
                break;

          
            default :
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new { message = exception.Message });
                break;
            
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}