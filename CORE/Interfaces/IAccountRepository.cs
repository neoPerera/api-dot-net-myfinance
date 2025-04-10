using CORE.Entities;

namespace CORE.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAccountListAsync(string userId);
        Task<string> GetAccountSequenceAsync();
        Task AddAccountAsync(Account account);
    }
}
