using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("validatejwt")]
        public IActionResult ValidateJwt()
        {
            bool isValid = true;
            return Ok(new { isValid = isValid });
        }
    }
}
