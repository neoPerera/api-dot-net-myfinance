using MyFinanceService.APPLICATION.DTOs;
using MyFinanceService.APPLICATION.Interfaces;
using MyFinanceService.CORE.Entities;
using MyFinanceService.CORE.Interfaces;

namespace MyFinanceService.APPLICATION.Services
{
    public class IncomeService(ICommonRepository _commonRepository, IActivityLogService _logService) : IIncomeService
    {
        public async Task<CommonResponse> GetIncomeListAsync()
        {
            var incomes = await _commonRepository.GetListAsync<Income>(filter: x => x.Active == '1');
            var mappedIncomes = incomes.Select(x => new GetRefListResponse
            {
                Key = x.Id,
                Str_name = x.Name,
                Dtm_date = x.Date.ToString("yyyy-MM-dd")
            }).ToList();
            return new ResponseService<CommonResponse>(mappedIncomes).Response;
           // return mappedIncomes;
        }
        public async Task<CommonResponse> GetIncomeSequenceAsync()
        {
            try
            {

                var sequence = await _commonRepository.GetSequenceAsync("income_sequence",3);
                string currentDate = DateTime.Now.ToString("yyyyMMdd");
                var response = new { Output_value = $"INC{currentDate}{sequence}" };
                return new ResponseService<CommonResponse>(response).Response;
            }
            catch (Exception e)
            {
                return new ResponseService<CommonResponse>(e).Response;
            }
        }
        public async Task<CommonResponse> AddIncomeAsync(AddRefRequest request)
        { 
            var Entity = new Income
            {
                Id = request.StrId,
                Name = request.StrName,
                Date = DateTime.Now,
                Active = '1'
            };
            try
            {
                await _commonRepository.SaveAsync<Income>(Entity);
                await _logService.Info("Record Added to Income master", Entity);
                return new ResponseService<CommonResponse>().Response;
            }
            catch (Exception e)
            {
                await _logService.Error(e);
                return new ResponseService<CommonResponse>(e).Response;
            }
        }

        public async Task<CommonResponse> UpdateIncomeAsync(UpdateRefRequest request)
        {
            try
            {
                // Fetch the existing entity by ID
                var Entity = await _commonRepository.GetByIdAsync<Income>(request.Str_id);
                if (Entity == null)
                {
                    string message = "Income record not found";
                    await _logService.Error(message:message);
                    return new ResponseService<CommonResponse>(message).Response;
                }

                _logService.ChangeLog(Entity.Name);
                Entity.Name = request.Updates.Str_name;
                _logService.ChangeLog(Entity.Name);
                // Save changes
                await _commonRepository.UpdateAsync<Income>(Entity);
                await _logService.FlushAsync("Income Updated");
                return new ResponseService<CommonResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<CommonResponse>(e).Response;
            }
        }
    }
}
