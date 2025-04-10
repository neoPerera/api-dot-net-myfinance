using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Entities;

namespace CORE.Interfaces
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> GetExpenseListAsync(string userId);
        Task<string> GetExpenseSequenceAsync();
        Task AddExpenseAsync(Expense expense);
    }
}
