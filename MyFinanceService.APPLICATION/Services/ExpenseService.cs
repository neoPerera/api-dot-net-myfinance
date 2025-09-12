using MyFinanceService.APPLICATION.DTOs;
using MyFinanceService.APPLICATION.Interfaces;
using MyFinanceService.CORE.Entities;
using MyFinanceService.CORE.Interfaces;

namespace MyFinanceService.APPLICATION.Services
{
    public class ExpenseService(ICommonRepository _commonRepository, IActivityLogService _logService) : IExpenseService
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
                await _logService.Info("Record added to expense master", Entity);
                return new ResponseService<CommonResponse>().Response;
            }
            catch (Exception e)
            {
                await _logService.Error(e);
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
                var Entity = await _commonRepository.GetByIdAsync<Expense>(request.Str_id);
                if (Entity == null)
                {
                    string message = "Expense record not found";
                    await _logService.Error(message:message);
                    return new ResponseService<CommonResponse>(message).Response;
                }
                _logService.ChangeLog(Entity.Name);
                // Update the column(s)
                Entity.Name = request.Updates.Str_name;
                _logService.ChangeLog(Entity.Name);

                // Save changes
                await _commonRepository.UpdateAsync<Expense>(Entity);
                await _logService.FlushAsync("Expense Updated");
                return new ResponseService<CommonResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<CommonResponse>(e).Response;
            }
        }
    }
}
