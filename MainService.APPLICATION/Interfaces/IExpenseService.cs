using MainService.APPLICATION.DTOs;

namespace MainService.APPLICATION.Interfaces
{
    public interface IExpenseService
    {
        Task<List<GetRefListResponse>> GetExpenseListAsync();
        Task<CommonResponse> GetExpenseSequenceAsync();
        Task<CommonResponse> AddExpenseAsync(AddRefRequest request);
        Task<CommonResponse> UpdateExpenseAsync(UpdateRefRequest request);
    }
}
