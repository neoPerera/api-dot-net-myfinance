using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
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
            // Check if the result contains a success property
            if (result.Success)
            {
                // If authentication was successful, return Ok with the result
                return Ok(result); // { message: "Login successful", token: "your-jwt-token" }
            }
            else
            {
                // If authentication failed, return Unauthorized with the message
                return Unauthorized(new { message = result.Message });
            }
        }
    }
}
