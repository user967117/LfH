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
}