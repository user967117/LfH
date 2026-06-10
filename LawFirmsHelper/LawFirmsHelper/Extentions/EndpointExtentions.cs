using LawFirmsHelper;
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
            
            var result = await userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                return Results.Ok();
            }
            
            return Results.BadRequest();
        });

        app.MapPost("/api/auth/login", async (LoginRequest model, SignInManager<IdentityUser> signInManager) =>
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return Results.BadRequest();
            }
            
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                return Results.Ok();
            }

            if (result.IsLockedOut)
            {
                return Results.BadRequest();
            }
            
            return Results.BadRequest();
        });
        return app;
    }
}