using Microsoft.AspNetCore.Identity;

namespace LawFirmsHelper.Services;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterAsync(string email, string password);
    Task<string?> LoginAsync(string email, string password);
}