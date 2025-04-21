using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Entities;
using CORE.Interfaces;

namespace APPLICATION.Services
{
    public class ExpenseService(ICommonRepository _commonRepository) : IExpenseService
    {
 
        public async Task<AddRefResponse> AddExpenseAsync(AddRefRequest request)
        {
            var Entity = new Expense
            {
                Id = request.StrId,
                Name = request.StrName,
                Date = DateTime.Now,
                Active = '1'
            };
            try
            {
                await _commonRepository.SaveAsync<Expense>(Entity);
                return new ResponseService<AddRefResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<AddRefResponse>(e).Response;
            }
        }

        public async Task<List<GetRefListResponse>> GetExpenseListAsync()
        {
            var expenses = await _commonRepository.GetListAsync<Expense>();

            var mappedExpenses = expenses.Select(x => new GetRefListResponse
            {
                Key = x.Id,
                Str_name = x.Name,
                Dtm_date = x.Date.ToString("yyyy-MM-dd")

            })
            .ToList();
            return mappedExpenses;
        }

        public async Task<GetRefSequenceResponse> GetExpenseSequenceAsync()
        {
            var sequence = await _commonRepository.GetSequenceAsync("expense_sequence", 3);
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            var response = new GetRefSequenceResponse
            {
                Output_value = $"EXP{currentDate}{sequence}"
            };

            return response;
        }
    }
}
