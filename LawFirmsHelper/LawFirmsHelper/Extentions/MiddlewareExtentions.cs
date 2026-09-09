using LawFirmsHelper;
using Microsoft.EntityFrameworkCore;

namespace LawFirmsHelper.Extentions;

public static class MiddlewareExtentions
{
    public static IApplicationBuilder UseMiddlewares(this WebApplication app)
    {
        
        app.UseSwagger();
        app.UseSwaggerUI();
            
        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        return app;
    }
    
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
        
        await context.Database.MigrateAsync(); 
        
        await seeder.SeedAsync();
    }
}