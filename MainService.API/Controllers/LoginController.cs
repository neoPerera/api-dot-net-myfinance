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
            await _logService.Debug("Started Execution");
            var result = await _loginService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
            if (result.Success)
            {
                await _logService.Debug("Execution Ended");
                return Ok(result);
            }
            else
            {
                //await _logService.Error(message:"Login failed");
                return Unauthorized(new { message = result.Message });
            }
        }

        [HttpPost("mobile")]
        public async Task<IActionResult> LoginMobile([FromBody] LoginRequest loginRequest)
        {
            await _logService.Debug("Started Mobile Login Execution");
            var result = await _loginService.AuthenticateMobileAsync(loginRequest.Username, loginRequest.Password);
            if (result.Success)
            {
                await _logService.Debug("Mobile Login Execution Ended");
                return Ok(result);
            }
            else
            {
                return Unauthorized(new { message = result.Message });
            }
        }
    }
}
