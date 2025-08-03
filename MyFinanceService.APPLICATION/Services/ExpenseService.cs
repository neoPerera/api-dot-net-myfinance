using MyFinanceService.APPLICATION.DTOs;
using MyFinanceService.APPLICATION.Interfaces;
using MyFinanceService.CORE.Entities;
using MyFinanceService.CORE.Interfaces;

namespace MyFinanceService.APPLICATION.Services
{
    public class ExpenseService(ICommonRepository _commonRepository) : IExpenseService
    {
 
        public async Task<CommonResponse> AddExpenseAsync(AddRefRequest request)
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
                return new ResponseService<CommonResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<CommonResponse>(e).Response;
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

        public async Task<CommonResponse> GetExpenseSequenceAsync()
        {
            var sequence = await _commonRepository.GetSequenceAsync("expense_sequence", 3);
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            var response = new { Output_value = $"EXM{currentDate}{sequence}" };
            return new ResponseService<CommonResponse>(response).Response;
        }

        public async Task<CommonResponse> UpdateExpenseAsync(UpdateRefRequest request)
        {
            try
            {
                // Fetch the existing entity by ID
                var existingEntity = await _commonRepository.GetByIdAsync<Expense>(request.Str_id);
                if (existingEntity == null)
                {
                    return new ResponseService<CommonResponse>("Income record not found.").Response;
                }

                // Update the column(s)
                existingEntity.Name = request.Updates.Str_name;

                // Save changes
                await _commonRepository.UpdateAsync<Expense>(existingEntity);

                return new ResponseService<CommonResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<CommonResponse>(e).Response;
            }
        }
    }
}
