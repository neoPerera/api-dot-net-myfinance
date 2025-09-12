using MainService.APPLICATION.Interfaces;
using MainService.APPLICATION.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainService.API.Controllers
{
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IFormService _formService;
        private readonly IConfiguration _configuration;
        private readonly IActivityLogService _logService;

        public HomeController(IFormService formService, IConfiguration configuration, IActivityLogService logService)
        {
            _formService = formService;
            _configuration = configuration;
            _logService = logService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var serviceInfo = new
            {
                Service = _configuration["SERVICE_NAME"] ?? "MainService",
                Version = _configuration["SERVICE_VERSION"] ?? "1.0.0",
                Status = "Running",
                Timestamp = DateTime.UtcNow,
                Endpoints = new
                {
                    Health = "/health",
                    Swagger = "/swagger",
                    Api = "/api"
                }
            };

            return Ok(serviceInfo);
        }

        [HttpGet("getForms")]
        [Authorize]
        public async Task<IActionResult> GetActiveForms()
        {
            var username = User.Identity?.Name;
            await _logService.Debug("Started Execution",variable:username);
            var forms = await _formService.GetActiveFormsAsync(username);
            await _logService.Debug("Started Ended", variable: forms);

            return Ok(forms);
        }
    }
}
