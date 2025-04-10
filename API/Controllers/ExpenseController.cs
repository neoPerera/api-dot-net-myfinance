using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/reference/ref-expense")]
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
            return Ok(result);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddExpense([FromBody] AddRefRequest request)
        {
            var result = await _expenseService.AddExpenseAsync(request);
            return StatusCode(result.StatusCode, result.Data);
        }
    }
}
