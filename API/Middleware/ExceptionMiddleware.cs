using API.Errors;
using System.Net;
using System.Text.Json;

namespace API.Middleware;

public class ExceptionMiddleware (IHostEnvironment env, RequestDelegate next)
{
    private static readonly JsonSerializerOptions m_jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, env);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = env.IsDevelopment()
            ? new ApiErrorResponds(context.Response.StatusCode, ex.Message, ex.StackTrace)
            : new ApiErrorResponds(context.Response.StatusCode, "Internal Server Error");

        var json = JsonSerializer.Serialize(response, m_jsonOptions);
        await context.Response.WriteAsync(json);
    }
}
