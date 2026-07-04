namespace LawFirmsHelper.Services;

public class UserContext
{
    public string UserId { get; set; }

    public UserContext(string userId)
    {
        UserId = userId;
    }
}