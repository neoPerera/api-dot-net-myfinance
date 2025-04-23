using APPLICATION.DTOs;

namespace APPLICATION.Interfaces
{
    public interface IIncomeService
    {
        Task<RefResponse> GetIncomeListAsync();
        Task<RefResponse> GetIncomeSequenceAsync();
        Task<RefResponse> AddIncomeAsync(AddRefRequest request);
    }
}
