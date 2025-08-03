using MyFinanceService.APPLICATION.DTOs;

namespace MyFinanceService.APPLICATION.Interfaces
{
    public interface IIncomeService
    {
        Task<CommonResponse> GetIncomeListAsync();
        Task<CommonResponse> GetIncomeSequenceAsync();
        Task<CommonResponse> AddIncomeAsync(AddRefRequest request);
        Task<CommonResponse> UpdateIncomeAsync(UpdateRefRequest request);
    }
}
