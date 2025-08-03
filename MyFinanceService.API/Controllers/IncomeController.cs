using MyFinanceService.APPLICATION.DTOs;
using MyFinanceService.APPLICATION.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyFinanceService.API.Controllers
{
    [Authorize]
    [Route("reference/ref-income")]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }
        [HttpGet("getincome")]
        public async Task<IActionResult> GetIncomeAsync()
        {
            var result = await _incomeService.GetIncomeListAsync();
            return StatusCode(result.StatusCode, result.Data);
        }
        [HttpGet("getSequence")]
        public async Task<IActionResult> GetSequenceAsync()
        {
            var result = await _incomeService.GetIncomeSequenceAsync();
            return StatusCode(result.StatusCode, result.Data);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddIncome([FromBody] AddRefRequest request)
        {
            var result = await _incomeService.AddIncomeAsync(request);
            return StatusCode(result.StatusCode, result.Data);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateIncome([FromBody] UpdateRefRequest request)
        {
            var result = await _incomeService.UpdateIncomeAsync(request);
            return StatusCode(result.StatusCode, result.Data);
        }
    }
}
