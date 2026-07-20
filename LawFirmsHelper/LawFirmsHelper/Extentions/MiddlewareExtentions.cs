using LawFirmsHelper;
namespace LawFirmsHelper.Extentions;

public static class MiddlewareExtentions
{
    public static IApplicationBuilder UseMiddlewares(this WebApplication app)
    {
        
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        return app;
    }
    
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
        
        await seeder.SeedAsync();
    }
}