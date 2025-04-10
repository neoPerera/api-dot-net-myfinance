using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Entities;
using CORE.Interfaces;
using Microsoft.AspNetCore.Http;
using Npgsql;

namespace APPLICATION.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountService(IHttpContextAccessor httpContextAccessor,IAccountRepository accountRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _accountRepository = accountRepository;
        }
        public async Task<AddRefResponse> AddAccountAsync(AddRefRequest request)
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity;

            var Entity = new Account
            {
                Id = request.StrId,
                Name = request.StrName,
                User = user?.Name ?? "test",
                Date = DateTime.Now,
                Active = 'Y'
            };
            try
            {
                await _accountRepository.AddAccountAsync(Entity);
                return new ResponseService<AddRefResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<AddRefResponse>(e).Response;
            }
        }

        public async Task<List<GetRefListResponse>> GetAccountListAsync()
        {
            var accounts = await _accountRepository.GetAccountListAsync("userId");
            var mappedAccounts = accounts.Select(x => new GetRefListResponse
            {
                Key = x.Id,
                Str_name = x.Name,
                Dtm_date = x.Date.ToString("yyyy-MM-dd")
            }).ToList();

            return mappedAccounts;
        }

        public async Task<GetRefSequenceResponse> GetAccountSequenceAsync()
        {
            var sequence = await _accountRepository.GetAccountSequenceAsync();
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            var response = new GetRefSequenceResponse
            {
                Output_value = $"ACC{currentDate}{sequence}"
            };
            return response;
        }
    }
}
