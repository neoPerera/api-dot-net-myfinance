using MainService.APPLICATION.DTOs;
using MainService.APPLICATION.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MainService.API.Controllers
{
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _loginService;
        private readonly IActivityLogService _logService;
        public LoginController(ILogin loginService, IActivityLogService logService)
        {
            _loginService = loginService;
            _logService = logService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            await _logService.Debug("User tries to log in", userId: loginRequest.Username, action: "Login");
            var result = await _loginService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
            if (result.Success)
            {
                await _logService.Debug("Login successful", userId: loginRequest.Username, action: "Login");
                return Ok(result);
            }
            else
            {
                await _logService.Error("Login failed", userId: loginRequest.Username, action: "Login");
                return Unauthorized(new { message = result.Message });
            }
        }
    }
}
