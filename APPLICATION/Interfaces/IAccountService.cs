using APPLICATION.DTOs;
namespace APPLICATION.Interfaces
{
    public interface IAccountService
    {
        Task<List<GetRefListResponse>> GetAccountListAsync();
        Task<GetRefSequenceResponse> GetAccountSequenceAsync();
        Task<AddRefResponse> AddAccountAsync(AddRefRequest request);
    }
}
