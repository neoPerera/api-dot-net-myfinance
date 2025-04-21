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
    public class TransactionService(IHttpContextAccessor _httpContextAccessor, ICommonRepository _commonRepository) : ITransactionService
    {
        public async Task<GetTransactionSequenceResponse> GetExpenseSequenceAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity;
            var sequence = await _commonRepository.GetSequenceAsync("transaction_sequence",4);
            var username = user?.Name ?? "ERROR";
            var accounts = await _commonRepository.GetListAsync<Account>(filter: a => a.Active == 'Y' && a.User == username);
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
            {
                try
                {
                    var user = _httpContextAccessor.HttpContext?.User.Identity;
                    var username = user?.Name ?? "ERROR";

                    // Create the primary transaction
                    var transaction = new Transaction
                    {
                        Id = request.StrId,
                        TrnType = request.StrTransType,
                        TrnCat = request.StrTransCat,
                        Amount = request.FloatAmount,
                        Date = DateTime.Now,
                        Reason = request.StrName,
                        User = username,
                        Account = request.StrAccount
                    };

                    // Handle double-entry logic
                    if (request.IsDoubleEntry)
                    {
                        if (request.StrAccount == request.StrAccount2)
                            throw new Exception("Source and destination accounts cannot be the same.");

                        var oppositeType = request.StrTransType == "INC" ? "EXP" : "INC";
                        var mirrorId = await _commonRepository.GetSequenceAsync("transaction_sequence", 4);

                        var mirrorTransaction = new Transaction
                        {
                            Id = mirrorId,
                            TrnType = oppositeType,
                            TrnCat = request.StrTransCat,
                            Amount = request.FloatAmount,
                            Date = DateTime.Now,
                            Reason = $"DOUBLE ENTRY FOR => {request.StrId}",
                            User = username,
                            Account = request.StrAccount2
                        };

                        await _commonRepository.SaveAsync<Transaction>(transaction);
                        await _commonRepository.SaveAsync<Transaction>(mirrorTransaction);
                    }
                    else
                    {
                        await _commonRepository.SaveAsync<Transaction>(transaction);
                    }

                    return new ResponseService<AddRefResponse>().Response;
                }
                catch (Exception ex)
                {
                    return new ResponseService<AddRefResponse>(ex).Response;
                }
            }

        }
    }
}
