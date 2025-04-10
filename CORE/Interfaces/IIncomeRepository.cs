using CORE.Entities;

namespace CORE.Interfaces
{
    public interface IIncomeRepository
    {
        Task<List<Income>> GetIncomeListAsync(string userId);
        Task<string> GetIncomeSequenceAsync();
        Task AddIncomeAsync(Income income);
    }
}
