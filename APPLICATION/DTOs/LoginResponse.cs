namespace Application.DTOs
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }

        public LoginResponse(bool success, string message, string token = null)
        {
            Success = success;
            Message = message;
            Token = token;
        }
    }
}