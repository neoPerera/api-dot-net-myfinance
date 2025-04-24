using APPLICATION.DTOs;
using CORE.Entities;
using CORE.Interfaces;
using INFRASTRUCTURE.Persistence;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;
        public DashboardRepository(AppDbContext context) 
        {
            _context = context;
        }
        public async Task<List<DashboardChart1>> GetDashboardChart1DataAsync(string userId)
        {
            // Fetch data and return as DTOs
            var result = await _context.Transactions
                .Where(a => a.User.Trim() == userId)
                .Join(
                    _context.Expenses,
                    a => a.TrnCat,
                    b => b.Id,
                    (a, b) => new { Transaction = a, Expense = b }
                )
                .GroupBy(x => x.Expense.Name)
                .Select(g => new DashboardChart1
                {
                    Type = g.Key,
                    Value = (float?)g.Sum(x => x.Transaction.Amount)
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<DashboardChart2>> GetDashboardChart2DataAsync(string userId)
        {
            // First part: Income → Account
            var incomePart = _context.Transactions
                .Where(a =>
                    a.TrnType == "INC" &&
                    a.User.Trim() == userId.Trim()
                )
                .Join(
                    _context.Incomes,
                    a => a.TrnCat,
                    b => b.Id,
                    (a, b) => new { Transaction = a, Income = b }
                )
                .Join(
                    _context.Accounts.Where(c => c.IsMain == 'Y'),
                    ab => ab.Transaction.Account,
                    c => c.Id,
                    (ab, account) => new DashboardChart2
                    {
                        Source1 = ab.Income.Name,
                        Target = account.Name,
                        Value = (float?)(ab.Transaction.Amount ?? 0)
                    }
                );

            // Second part: Account → Expense
            var expensePart = _context.Transactions
                .Where(a =>
                    a.TrnType == "EXP" &&
                    a.User.Trim() == userId.Trim()
                )
                .Join(
                    _context.Expenses,
                    a => a.TrnCat,
                    b => b.Id,
                    (a, b) => new { Transaction = a, Expense = b }
                )
                .Join(
                    _context.Accounts.Where(c => c.IsMain == 'Y'),
                    ab => ab.Transaction.Account,
                    c => c.Id,
                    (ab, account) => new DashboardChart2
                    {
                        Source1 = account.Name,
                        Target = ab.Expense.Name,
                        Value = (float?)(ab.Transaction.Amount ?? 0)
                    }
                );

            // Combine both with UnionAll
            var result = await incomePart
                .Concat(expensePart)
                .ToListAsync();

            return result;
        }



        public async Task<List<DashboardChart3>> GetDashboardChart3DataAsync(string userId)
        {
            var result = await _context.Transactions
                .Where(a =>
                    a.User.Trim() == userId.Trim() &&
                    a.Account != null
                )
                .Join(
                    _context.Accounts.Where(b => b.IsMain == 'Y'),
                    a => a.Account,
                    b => b.Id,
                    (a, b) => new { Transaction = a, Account = b }
                )
                .GroupBy(x => x.Transaction.TrnType)
                .Select(g => new DashboardChart3
                {
                    Type = g.Key,
                    Value = (float?)g.Sum(x => x.Transaction.Amount)
                })
                .OrderBy(x => x.Type) // Ensure it's ordered by transaction type
                .ToListAsync();

            return result;
        }

        public async Task<List<DashboardChart4>> GetDashboardChart4DataAsync(string userId)
        {
            var result = await _context.Transactions
                .Where(t => t.User.Trim() == userId.Trim())
                .Join(
                    _context.Accounts.Where(a => a.Active == 'Y'),
                    t => t.Account,
                    a => a.Id,
                    (t, a) => new { Transaction = t, Account = a }
                )
                .OrderByDescending(x => x.Transaction.Date)
                .Select(x => new DashboardChart4
                {
                    Key = x.Transaction.Id,
                    Name = x.Transaction.Reason,
                    Account = x.Account.Name,
                    IntAmount = (float?)(x.Transaction.TrnType == "EXP" ? -x.Transaction.Amount : x.Transaction.Amount),
                    Int_amount_char = (x.Transaction.TrnType == "EXP" ? -x.Transaction.Amount : x.Transaction.Amount)
                        .GetValueOrDefault()
                        .ToString("N2")
                })
                .ToListAsync();

            return result;
        }
    }
}
