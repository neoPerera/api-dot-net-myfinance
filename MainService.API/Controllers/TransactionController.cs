using MainService.APPLICATION.DTOs;
using MainService.APPLICATION.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainService.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
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
