using APPLICATION.DTOs;
namespace APPLICATION.Interfaces
{
    public interface IAccountService
    {
        Task<List<GetRefListResponse>> GetAccountListAsync();
        Task<CommonResponse> GetAccountSequenceAsync();
        Task<CommonResponse> AddAccountAsync(AddRefRequest request);
    }
}
