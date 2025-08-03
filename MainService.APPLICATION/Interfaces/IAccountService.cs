using MainService.APPLICATION.DTOs;
namespace MainService.APPLICATION.Interfaces
{
    public interface IAccountService
    {
        Task<List<GetRefListResponse>> GetAccountListAsync();
        Task<CommonResponse> GetAccountSequenceAsync();
        Task<CommonResponse> AddAccountAsync(AddRefRequest request);
        Task<CommonResponse> UpdateAccountAsync(UpdateRefRequest request);
    }
}
