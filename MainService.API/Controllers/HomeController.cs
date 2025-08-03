using MainService.APPLICATION.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainService.API.Controllers
{
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IFormService _formService;
        private readonly IConfiguration _configuration;

        public HomeController(IFormService formService, IConfiguration configuration)
        {
            _formService = formService;
            _configuration = configuration;
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
            var forms = await _formService.GetActiveFormsAsync(username);
            return Ok(forms);
        }
    }
}
