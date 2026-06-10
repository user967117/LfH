namespace LawFirmsHelper.DTO;

public class DTO
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}