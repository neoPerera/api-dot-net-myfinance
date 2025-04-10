using APPLICATION.DTOs;
using CORE.Entities;
using CORE.Interfaces;
using INFRASTRUCTURE.Persistence;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Repositories
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly AppDbContext _context;
        public IncomeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Income>> GetIncomeListAsync(string userId)
        {
            // Fetch data and return as DTOs
            var result = await _context.Incomes
                .Where(a => a.Active == '1')
                .ToListAsync();
            return result;
        }
    
        public async Task<string> GetIncomeSequenceAsync()
        {
            // Use FromSqlRaw to execute the SQL query and get the next sequence value
            var nextSequenceValue = await _context.Database.SqlQueryRaw<string>("SELECT LPAD(nextval('income_sequence')::TEXT, 3, '0') as \"Value\"").FirstOrDefaultAsync();


            // Return the next sequence value as a string
            return nextSequenceValue.ToString();
        }
        public async Task AddIncomeAsync(Income income)
        {
            
            await _context.Incomes.AddAsync(income);
            // Save changes to the database
            await _context.SaveChangesAsync();
        }
    }
    
}
