using APPLICATION.DTOs;

namespace APPLICATION.Interfaces
{
    public interface IExpenseService
    {
        Task<List<GetRefListResponse>> GetExpenseListAsync();
        Task<GetRefSequenceResponse> GetExpenseSequenceAsync();
        Task<AddRefResponse> AddExpenseAsync(AddRefRequest request);
    }
}
