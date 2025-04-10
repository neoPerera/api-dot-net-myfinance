using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/reference/ref-accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _iAccountService;
        public AccountController(IAccountService iAccountService)
        {
            _iAccountService = iAccountService;
        }
        [HttpGet("getaccounts")]
        public async Task<IActionResult> GetAccountAsync()
        {
            var result = await _iAccountService.GetAccountListAsync();
            return Ok(result);
        }
        [HttpGet("getsequence")]
        public async Task<IActionResult> GetSequenceAsync()
        {
            var result = await _iAccountService.GetAccountSequenceAsync();
            return Ok(result);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddAccount([FromBody] AddRefRequest request)
        {
            var result = await _iAccountService.AddAccountAsync(request);
            return StatusCode(result.StatusCode, result.Data);
        }

    }
}
