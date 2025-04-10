using CORE.Entities;
using CORE.Interfaces;
using INFRASTRUCTURE.Persistence;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _context;
        public ExpenseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddExpenseAsync(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);
            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<List<Expense>> GetExpenseListAsync(string userId)
        {
            var result = await _context.Expenses
                .Where(a => a.Active == '1')
                .ToListAsync();
            return result;
        }

        public async Task<string> GetExpenseSequenceAsync()
        {
            // Use FromSqlRaw to execute the SQL query and get the next sequence value
            var nextSequenceValue = await _context.Database.SqlQueryRaw<string>("SELECT LPAD(nextval('expense_sequence')::TEXT, 3, '0') as \"Value\"").FirstOrDefaultAsync();


            // Return the next sequence value as a string
            return nextSequenceValue.ToString();
        }
    }
}
