using LawFirmsHelper;
using LawFirmsHelper.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace LawFirmsHelper.Extentions;

public static class EndpointExtentions
{
    public static WebApplication UseEndpointExtensions(this WebApplication app)
    {
        app.MapPost("/api/auth/register", async (RegisterRequest model, UserManager<IdentityUser> userManager) =>
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return Results.BadRequest();
            }
            
            var userExist = await userManager.FindByEmailAsync(model.Email);
            if (userExist != null)
            {
                return Results.BadRequest();
            }

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Results.Ok();
            }
            
            return Results.BadRequest();
        });

        app.MapPost("/api/auth/login", async (LoginRequest model, UserManager<IdentityUser> userManager, IJwtService jwtService) =>
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return Results.BadRequest();
            }
            
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Results.BadRequest();
            }
            
            var isPasswordValid = await userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                return Results.BadRequest();
            }
            
            var tokenString = jwtService.GenerateJwt(user);

            return Results.Ok(new { token = tokenString });
        });
        
        
        return app;
    }
}