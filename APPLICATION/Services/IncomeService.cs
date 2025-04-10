using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Entities;
using CORE.Interfaces;

namespace APPLICATION.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;
        public IncomeService(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }
        public async Task<List<GetRefListResponse>> GetIncomeListAsync()
        {
            var incomes = await _incomeRepository.GetIncomeListAsync("userId");

            var mappedIncomes = incomes.Select(x => new GetRefListResponse
            {
                Key = x.Id,
                Str_name = x.Name,
                Dtm_date = x.Date.ToString("yyyy-MM-dd")

            })
            .ToList();
            return mappedIncomes;

        }
        public async Task<GetRefSequenceResponse> GetIncomeSequenceAsync()
        {
            var sequence = await _incomeRepository.GetIncomeSequenceAsync();
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            var response = new GetRefSequenceResponse
            {
                Output_value = $"INC{currentDate}{sequence}"
            };

            return response;
        }
        public async Task<AddRefResponse> AddIncomeAsync(AddRefRequest request)
        { 
            // Add the income entity to the context
            var Entity = new Income
            {
                Id = request.StrId,
                Name = request.StrName,
                Date = DateTime.Now,
                Active = '1'
            };
            try
            {
                await _incomeRepository.AddIncomeAsync(Entity);
                return new ResponseService<AddRefResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<AddRefResponse>(e).Response;
            }
        }
    }
}
