using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainService.APPLICATION.DTOs;

namespace MainService.APPLICATION.Interfaces
{
    public interface ITransactionService
    { 
        Task<CommonResponse> AddTransaction(AddTransactionRequest request);
        public Task<GetTransactionSequenceResponse> GetExpenseSequenceAsync();
    }
}
