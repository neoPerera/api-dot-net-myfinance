using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Entities;
using CORE.Interfaces;
using Microsoft.AspNetCore.Http;
using Npgsql;

namespace APPLICATION.Services
{
    public class AccountService(IHttpContextAccessor _httpContextAccessor, ICommonRepository _commonRepository) : IAccountService
    {
        public async Task<AddRefResponse> AddAccountAsync(AddRefRequest request)
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity;

            var Entity = new Account
            {
                Id = request.StrId,
                Name = request.StrName,
                User = user?.Name ?? "ERROR",
                Date = DateTime.Now,
                Active = 'Y'
            };
            try
            {
                await _commonRepository.SaveAsync<Account>(Entity);
                return new ResponseService<AddRefResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<AddRefResponse>(e).Response;
            }
        }

        public async Task<List<GetRefListResponse>> GetAccountListAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User.Identity ;
            var username = user?.Name ?? "ERROR";
            var accounts = await _commonRepository.GetListAsync<Account>(filter: a => a.Active == 'Y' && a.User == username );
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
            var sequence = await _commonRepository.GetSequenceAsync("accounts_sequence", 3);
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            var response = new GetRefSequenceResponse
            {
                Output_value = $"ACC{currentDate}{sequence}"
            };
            return response;
        }
    }
}
