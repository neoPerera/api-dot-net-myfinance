using MainService.APPLICATION.DTOs;
using MainService.APPLICATION.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MainService.API.Controllers
{
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _loginService;

        public LoginController(ILogin loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _loginService.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return Unauthorized(new { message = result.Message });
            }
        }
    }
}
