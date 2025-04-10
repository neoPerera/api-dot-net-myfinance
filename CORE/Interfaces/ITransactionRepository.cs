using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Entities;

namespace CORE.Interfaces
{
    public interface ITransactionRepository
    {
        public Task<string> GetTransactionSequence();
        Task AddTransaction(Transaction transaction);
    }
}
