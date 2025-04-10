using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpGet("getsequence")]
        public async Task<IActionResult> GetSequence([FromQuery] string type)
        {
            var results = await _transactionService.GetExpenseSequenceAsync();
            return Ok(results);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddTransactionAsync([FromBody] AddTransactionRequest request)
        {
            var result = await _transactionService.AddTransaction(request);
            return StatusCode(result.StatusCode, result.Data);
        }
    }
}
