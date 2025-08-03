using System.Collections.Generic;
using System.Threading.Tasks;
using MyFinanceService.APPLICATION.DTOs;

namespace MyFinanceService.APPLICATION.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        Task<UserResponse> GetUserByIdAsync(int id);
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request);
        Task<bool> DeleteUserAsync(int id);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
} 