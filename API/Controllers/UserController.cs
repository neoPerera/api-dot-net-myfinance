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
            bool isValid = true;
            return Ok(new { isValid = isValid });
        }
    }
}
