namespace CustomerManagement.Api.Middleware;

public class ApiKeyAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationMiddleware(
        RequestDelegate next,
        IConfiguration configuration
    ){
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }

        var configApiKey = _configuration["ApiKey"]
            ?? throw new InvalidOperationException("ApiKey not set");

        if(!context.Request.Headers.TryGetValue("X-API-Key", out var headerApiKey)
            || headerApiKey != configApiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }
}