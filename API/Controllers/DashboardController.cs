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

        // GET: /Dashboard
        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var username = User.Identity?.Name;
            var result = await _dashboardService.GetDashboardDataAsync(username);

            // Return the result wrapped in an OK response
            return Ok(result);
        }
    }
}
