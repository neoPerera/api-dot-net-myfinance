using APPLICATION.DTOs;

namespace APPLICATION.Interfaces
{
    public interface IIncomeService
    {
        Task<CommonResponse> GetIncomeListAsync();
        Task<CommonResponse> GetIncomeSequenceAsync();
        Task<CommonResponse> AddIncomeAsync(AddRefRequest request);
        Task<CommonResponse> UpdateIncomeAsync(UpdateRefRequest request);
    }
}
