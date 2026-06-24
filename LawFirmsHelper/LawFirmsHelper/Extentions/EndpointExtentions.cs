using LawFirmsHelper;
using LawFirmsHelper.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace LawFirmsHelper.Extentions;

public static class EndpointExtentions
{
    public static WebApplication UseEndpointExtensions(this WebApplication app)
    {
        app.MapPost("/api/auth/register", async (RegisterRequest model, IAuthenticationService authService) =>
        {
            var result = await authService.RegisterAsync(model.Email, model.Password);
            return result.Succeeded ? Results.Ok() : Results.BadRequest();
        });

        app.MapPost("/api/auth/login", async (LoginRequest model, IAuthenticationService authService, CancellationToken cancellationToken) =>
        {
            var token = await authService.LoginAsync(model.Email, model.Password, cancellationToken);
            return token != null ? Results.Ok(new {token}) : Results.BadRequest();
        });
        
        
        return app;
    }
}