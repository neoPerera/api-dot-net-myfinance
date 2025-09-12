using Microsoft.AspNetCore.Http;
using MyFinanceService.APPLICATION.DTOs;
using MyFinanceService.APPLICATION.Interfaces;
using MyFinanceService.CORE.Entities;
using MyFinanceService.CORE.Interfaces;
using Newtonsoft.Json;
using Npgsql;

namespace MyFinanceService.APPLICATION.Services
{
    public class AccountService(IHttpContextAccessor _httpContextAccessor, ICommonRepository _commonRepository, IActivityLogService _logService) : IAccountService
    {
        public async Task<CommonResponse> AddAccountAsync(AddRefRequest request)
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
                await _logService.Info("Added New Account", variable:Entity);
                return new ResponseService<CommonResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<CommonResponse>(e).Response;
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

        public async Task<CommonResponse> GetAccountSequenceAsync()
        {
            var sequence = await _commonRepository.GetSequenceAsync("accounts_sequence", 3);
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            var response = new { Output_value = $"ACC{currentDate}{sequence}" };
            return new ResponseService<CommonResponse>(response).Response;
        }
        public async Task<CommonResponse> UpdateAccountAsync(UpdateRefRequest request)
        {
            try
            {
                // Fetch the existing entity by ID
                var Entity = await _commonRepository.GetByIdAsync<Account>(request.Str_id);
                if (Entity == null)
                {
                    return new ResponseService<CommonResponse>("Income record not found.").Response;
                }
                _logService.ChangeLog(Entity.Name);
                Entity.Name = request.Updates.Str_name;

                _logService.ChangeLog(Entity.Name);
                // Save changes
                await _commonRepository.UpdateAsync<Account>(Entity);


                await _logService.FlushAsync("Account Updated");
                return new ResponseService<CommonResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<CommonResponse>(e).Response;
            }
        }
    }
}
