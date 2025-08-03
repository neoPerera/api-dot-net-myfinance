using MainService.APPLICATION.DTOs;

namespace MainService.APPLICATION.Interfaces
{
    public interface ILogin
    {
        Task<LoginResponse> AuthenticateAsync(string username, string password);
    }
}