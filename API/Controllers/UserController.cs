using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("validatejwt")]
        public IActionResult ValidateJwt()
        {
            bool isValid = true; // Assume the JWT is valid for demonstration

            // Return response with isValid property
            return Ok(new { isValid = isValid });
        }
    }
}
