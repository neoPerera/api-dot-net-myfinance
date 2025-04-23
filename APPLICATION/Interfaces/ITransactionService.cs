using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APPLICATION.DTOs;

namespace APPLICATION.Interfaces
{
    public interface ITransactionService
    { 
        Task<RefResponse> AddTransaction(AddTransactionRequest request);
        public Task<GetTransactionSequenceResponse> GetExpenseSequenceAsync();
    }
}
