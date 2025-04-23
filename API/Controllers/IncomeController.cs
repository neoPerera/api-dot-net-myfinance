using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/reference/ref-income")]
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
    }
}
