
public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddApplication(builder.Configuration)
            .AddInfrastructure(builder.Configuration);

        builder.Services.AddProblemDetails();
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var app = builder.Build();
        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(o => o.SwaggerEndpoint("/openapi/v1.json", "CustomerManagement v1"));
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}