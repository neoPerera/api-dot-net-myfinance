using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFinanceService.APPLICATION.DTOs;

namespace MyFinanceService.APPLICATION.Interfaces
{
    public interface ITransactionService
    { 
        Task<CommonResponse> AddTransaction(AddTransactionRequest request);
        public Task<GetTransactionSequenceResponse> GetExpenseSequenceAsync();
    }
}
