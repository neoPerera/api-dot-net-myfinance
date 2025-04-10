using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Entities;
using CORE.Interfaces;
using INFRASTRUCTURE.Persistence;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;
        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddTransaction(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetTransactionSequence()
        {
            // Use FromSqlRaw to execute the SQL query and get the next sequence value
            var nextSequenceValue = await _context.Database.SqlQueryRaw<string>("SELECT LPAD(nextval('transaction_sequence')::TEXT, 4, '0') as \"Value\"").FirstOrDefaultAsync();


            // Return the next sequence value as a string
            return nextSequenceValue.ToString();
        }
    }
}
