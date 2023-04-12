namespace WebApplication.Contracts.Responses
{
    
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public string Message { get; set; }

        public bool Success { get; set; } = false;
        public LoginUserResponse User { get; set; }


    }

    public class LoginUserResponse 
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }


    }
}
