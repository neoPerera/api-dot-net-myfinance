using APPLICATION.DTOs;

namespace APPLICATION.Interfaces
{
    public interface IIncomeService
    {
        Task<List<GetRefListResponse>> GetIncomeListAsync();
        Task<GetRefSequenceResponse> GetIncomeSequenceAsync();
        Task<AddRefResponse> AddIncomeAsync(AddRefRequest request);
    }
}
