using MyFinanceService.APPLICATION.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MyFinanceService.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IActivityLogService _logService;

        public DashboardController(IDashboardService dashboardService, IActivityLogService logService)
        {
            _dashboardService = dashboardService;
            _logService = logService;
        }


        [HttpGet]
        [Route("getdashboardaccountbalances")]
        public async Task<IActionResult> GetDashboardAccountBalances()
        {
            await _logService.Debug("Execution started");
            var result = await _dashboardService.GetDashboardAccountBalancesAsync();
            await _logService.Debug("Execution ended",result);

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet]
        [Route("getdashboardaccountincomes")]
        public async Task<IActionResult> GetDashboardAccountIncomes()
        {
            await _logService.Debug("Execution started");
            var result = await _dashboardService.GetDashboardAccountIncomesAsync();
            await _logService.Debug("Execution ended", result);
            return StatusCode(result.StatusCode, result.Data);

        }

        [HttpGet]
        [Route("getdashboardaccountexpenses")]
        public async Task<IActionResult> GetDashboardAccountExpenses()
        {
            await _logService.Debug("Execution started");
            var result = await _dashboardService.GetDashboardAccountExpensesAsync();
            await _logService.Debug("Execution ended", result);
            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet]
        [Route("getdashboardtransactions")]
        public async Task<IActionResult> GetDashboardTransactions()
        {
            await _logService.Debug("Execution started");
            var result = await _dashboardService.GetDashboardTransactionsAsync();
            int x = 123;
            await _logService.Debug("Execution ended", x);
            return StatusCode(result.StatusCode, result.Data);
        }
    }
}
