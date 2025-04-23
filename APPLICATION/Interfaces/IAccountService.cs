using APPLICATION.DTOs;
namespace APPLICATION.Interfaces
{
    public interface IAccountService
    {
        Task<List<GetRefListResponse>> GetAccountListAsync();
        Task<RefResponse> GetAccountSequenceAsync();
        Task<RefResponse> AddAccountAsync(AddRefRequest request);
    }
}
