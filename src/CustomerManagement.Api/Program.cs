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
        app.MapControllers();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(o => o.SwaggerEndpoint("/openapi/v1.json", "CustomerManagement v1"));
            app.UseCors("AngularDev");
        }

        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.MapControllers();

        app.Run();
    }
}