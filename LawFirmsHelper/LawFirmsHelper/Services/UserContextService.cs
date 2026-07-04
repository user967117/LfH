using System.Security.Claims;

namespace LawFirmsHelper.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _accessor;

    public UserContextService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public UserContext GetContext()
    {
        var user = _accessor.HttpContext.User;
        var userId = user.FindFirstValue(ClaimConstants.Id);
        
        return new UserContext(userId);
    }
}