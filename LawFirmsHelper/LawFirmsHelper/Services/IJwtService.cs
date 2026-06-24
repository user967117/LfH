using Microsoft.AspNetCore.Identity;

namespace LawFirmsHelper.Services;

public interface IJwtService
{
    string GenerateJwt(IdentityUser user);
}