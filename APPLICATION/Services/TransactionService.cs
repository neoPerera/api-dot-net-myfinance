using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Entities;
using CORE.Interfaces;
using Microsoft.AspNetCore.Http;

namespace APPLICATION.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;


        public TransactionService(IHttpContextAccessor httpContextAccessor,ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<GetTransactionSequenceResponse> GetExpenseSequenceAsync()
        {
            var sequence = await _transactionRepository.GetTransactionSequence();
            var accounts = await _accountRepository.GetAccountListAsync("userId");
            var MappedAccounts = accounts.Select(c => new GetTransactionSequenceTypes
            {
                Value = c.Id ?? "No Id",    // Handle null if any
                Label = c.Name ??"No Label"
            }).ToList();

            var AccountsObject = new List<object>
            {
                MappedAccounts
            };

            var MappedTransTypes = new List<GetTransactionSequenceTypes>
            {
                new GetTransactionSequenceTypes
                {
                    Value = "INC",
                    Label = "Income"
                },
                new GetTransactionSequenceTypes
                {
                    Value = "EXP",
                    Label = "Expense"
                }
            }.ToList();

            var TransTypesObject = new List<object>
            {
                MappedTransTypes
            };
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            var response = new GetTransactionSequenceResponse
            {
                Accounts = AccountsObject,
                Sequence_id = $"TRN{currentDate}{sequence}",
                Trans_types = TransTypesObject
            };

            return response;
        }

        public async Task<AddRefResponse> AddTransaction(AddTransactionRequest request)
        {
            try
            {

                var user = _httpContextAccessor.HttpContext?.User.Identity;
                // Map AddTransactionRequest to Transaction entity
                var Entity = new Transaction
                {
                    Id = request.StrId,
                    TrnType = request.StrTransType,
                    TrnCat = request.StrTransCat,
                    Amount = request.FloatAmount,
                    Date = DateTime.Now,  // Assuming current time for Date
                    Reason = request.StrName, // Assuming "Name" as reason for now
                    User = user?.Name ?? "ERROR", // Assuming "Account" as user for now
                    Account = request.StrAccount // Account is set as Account
                };
                await _transactionRepository.AddTransaction(Entity);
                return new ResponseService<AddRefResponse>().Response;
            }catch(Exception e)
            {
                return new ResponseService<AddRefResponse>(e).Response;
            }

        }
    }
}
