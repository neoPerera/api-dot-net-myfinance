using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Entities;
using CORE.Interfaces;

namespace APPLICATION.Services
{
    public class IncomeService(ICommonRepository _commonRepository) : IIncomeService
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
                return new ResponseService<CommonResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<CommonResponse>(e).Response;
            }
        }

        public async Task<CommonResponse> UpdateIncomeAsync(UpdateRefRequest request)
        {
            try
            {
                // Fetch the existing entity by ID
                var existingIncome = await _commonRepository.GetByIdAsync<Income>(request.Str_id);
                if (existingIncome == null)
                {
                    return new ResponseService<CommonResponse>("Income record not found.").Response;
                }

                // Update the column(s)
                existingIncome.Name = request.Updates.Str_name;

                // Save changes
                await _commonRepository.UpdateAsync<Income>(existingIncome);

                return new ResponseService<CommonResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<CommonResponse>(e).Response;
            }
        }
    }
}
