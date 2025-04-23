using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Entities;
using CORE.Interfaces;

namespace APPLICATION.Services
{
    public class IncomeService(ICommonRepository _commonRepository) : IIncomeService
    {
        public async Task<RefResponse> GetIncomeListAsync()
        {
            var incomes = await _commonRepository.GetListAsync<Income>(filter: x => x.Active == '1');
            var mappedIncomes = incomes.Select(x => new GetRefListResponse
            {
                Key = x.Id,
                Str_name = x.Name,
                Dtm_date = x.Date.ToString("yyyy-MM-dd")
            }).ToList();
            return new ResponseService<RefResponse>(mappedIncomes).Response;
           // return mappedIncomes;
        }
        public async Task<RefResponse> GetIncomeSequenceAsync()
        {
            try
            {

                var sequence = await _commonRepository.GetSequenceAsync("income_sequence",3);
                string currentDate = DateTime.Now.ToString("yyyyMMdd");
                var response = new { Output_value = $"INC{currentDate}{sequence}" };
                return new ResponseService<RefResponse>(response).Response;
            }
            catch (Exception e)
            {
                return new ResponseService<RefResponse>(e).Response;
            }
        }
        public async Task<RefResponse> AddIncomeAsync(AddRefRequest request)
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
                return new ResponseService<RefResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<RefResponse>(e).Response;
            }
        }
    }
}
