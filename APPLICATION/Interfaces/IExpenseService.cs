using APPLICATION.DTOs;

namespace APPLICATION.Interfaces
{
    public interface IExpenseService
    {
        Task<List<GetRefListResponse>> GetExpenseListAsync();
        Task<RefResponse> GetExpenseSequenceAsync();
        Task<RefResponse> AddExpenseAsync(AddRefRequest request);
    }
}
