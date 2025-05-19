using APPLICATION.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }


        [HttpGet]
        [Route("getdashboardaccountbalances")]
        public async Task<IActionResult> GetDashboardAccountBalances()
        {
            var result = await _dashboardService.GetDashboardAccountBalancesAsync();
            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet]
        [Route("getdashboardaccountincomes")]
        public async Task<IActionResult> GetDashboardAccountIncomes()
        {
            var result = await _dashboardService.GetDashboardAccountIncomesAsync();
            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet]
        [Route("getdashboardaccountexpenses")]
        public async Task<IActionResult> GetDashboardAccountExpenses()
        {
            var result = await _dashboardService.GetDashboardAccountExpensesAsync();
            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet]
        [Route("getdashboardtransactions")]
        public async Task<IActionResult> GetDashboardTransactions()
        {
            var result = await _dashboardService.GetDashboardTransactionsAsync();
            return StatusCode(result.StatusCode, result.Data);
        }
    }
}
