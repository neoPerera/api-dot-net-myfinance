using MainService.APPLICATION.DTOs;
using MainService.APPLICATION.Interfaces;
using MainService.APPLICATION.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainService.API.Controllers
{
    [Authorize]
    [Route("reference/ref-expense")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }
        [HttpGet("getexpense")]
        public async Task<IActionResult> GetExpenseAsync()
        {
            var result = await _expenseService.GetExpenseListAsync();
            return Ok(result);
        }
        [HttpGet("getSequence")]
        public async Task<IActionResult> GetSequenceAsync()
        {
            var result = await _expenseService.GetExpenseSequenceAsync();
            return StatusCode(result.StatusCode, result.Data);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddExpense([FromBody] AddRefRequest request)
        {
            var result = await _expenseService.AddExpenseAsync(request);
            return StatusCode(result.StatusCode, result.Data);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateIncome([FromBody] UpdateRefRequest request)
        {
            var result = await _expenseService.UpdateExpenseAsync(request);
            return StatusCode(result.StatusCode, result.Data);
        }
    }
}
