using Application.DTOs;

namespace APPLICATION.Interfaces
{
    public interface ILogin
    {
        Task<LoginResponse> AuthenticateAsync(string username, string password);
    }
}