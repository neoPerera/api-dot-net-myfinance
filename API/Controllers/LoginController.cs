using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using APPLICATION.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class LoginController(ILogin _loginService, EmailService _emailService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            await _emailService.SendEmailAsync();
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
