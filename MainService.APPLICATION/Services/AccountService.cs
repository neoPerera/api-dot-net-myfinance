using MainService.APPLICATION.DTOs;
using MainService.APPLICATION.Interfaces;
using MainService.CORE.Entities;
using MainService.CORE.Interfaces;
using Microsoft.AspNetCore.Http;
using Npgsql;

namespace MainService.APPLICATION.Services
{
    public class AccountService(IHttpContextAccessor _httpContextAccessor, ICommonRepository _commonRepository) : IAccountService
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
                var existingEntity = await _commonRepository.GetByIdAsync<Account>(request.Str_id);
                if (existingEntity == null)
                {
                    return new ResponseService<CommonResponse>("Income record not found.").Response;
                }

                // Update the column(s)
                existingEntity.Name = request.Updates.Str_name;

                // Save changes
                await _commonRepository.UpdateAsync<Account>(existingEntity);

                return new ResponseService<CommonResponse>().Response;
            }
            catch (Exception e)
            {
                return new ResponseService<CommonResponse>(e).Response;
            }
        }
    }
}
