using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Entities;
using CORE.Interfaces;

namespace APPLICATION.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

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
                await _expenseRepository.AddExpenseAsync(Entity);
                return new ResponseService<AddRefResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<AddRefResponse>(e).Response;
            }
        }

        public async Task<List<GetRefListResponse>> GetExpenseListAsync()
        {
            var expenses = await _expenseRepository.GetExpenseListAsync("userId");

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
            var sequence = await _expenseRepository.GetExpenseSequenceAsync();
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            var response = new GetRefSequenceResponse
            {
                Output_value = $"EXP{currentDate}{sequence}"
            };

            return response;
        }
    }
}
