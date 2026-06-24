using Microsoft.AspNetCore.Identity;

namespace LawFirmsHelper.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IJwtService _jwtService;
    public AuthenticationService(UserManager<IdentityUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }
    
    public async Task<IdentityResult> RegisterAsync(string email, string password)
    {
        var user = new IdentityUser {UserName = email, Email = email};
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
        {
            return null;
        }
        return _jwtService.GenerateJwt(user);
    }
}