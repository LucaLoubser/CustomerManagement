using CustomerManagement.Api.Middleware;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddApi(builder.Configuration)
            .AddApplication(builder.Configuration)
            .AddInfrastructure(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerManagement v1"));
            app.UseCors("AngularDev");
        }

        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseMiddleware<ApiKeyAuthenticationMiddleware>();
        app.MapControllers();

        app.Run();
    }
}