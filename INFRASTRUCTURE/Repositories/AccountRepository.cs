using CORE.Entities;
using CORE.Interfaces;
using INFRASTRUCTURE.Persistence;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAccountAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

        }

        public async Task<List<Account>> GetAccountListAsync(string userId)
        {
            var result = await _context.Accounts
                .Where(a => a.Active == 'Y')
                .ToListAsync();
            return result;
        }

        public async Task<string> GetAccountSequenceAsync()
        {
            var nextSequenceValue = await _context.Database
                .SqlQueryRaw<string>("SELECT LPAD(nextval('accounts_sequence')::TEXT, 3, '0') as \"Value\"")
                .FirstOrDefaultAsync();
            return nextSequenceValue.ToString();
        }
    }
}
