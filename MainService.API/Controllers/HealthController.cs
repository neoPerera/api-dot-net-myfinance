using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MainService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public HealthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var healthInfo = new
            {
                Status = "Healthy",
                Service = _configuration["SERVICE_NAME"] ?? "MainService",
                Version = _configuration["SERVICE_VERSION"] ?? "1.0.0",
                Timestamp = DateTime.UtcNow,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                ProcessId = Process.GetCurrentProcess().Id,
                MemoryUsage = GC.GetTotalMemory(false),
                Uptime = Process.GetCurrentProcess().StartTime
            };

            return Ok(healthInfo);
        }

        [HttpGet("ready")]
        public IActionResult Ready()
        {
            // Add any readiness checks here (database connectivity, etc.)
            return Ok(new { Status = "Ready" });
        }

        [HttpGet("live")]
        public IActionResult Live()
        {
            // Simple liveness check
            return Ok(new { Status = "Alive" });
        }
    }
} 