using APPLICATION.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IFormService _formService;

        public HomeController(IFormService formService)
        {
            _formService = formService;
        }

        [HttpGet("getForms")]
        public async Task<IActionResult> GetActiveForms()
        {
            var username = User.Identity?.Name;
            var forms = await _formService.GetActiveFormsAsync(username);
            return Ok(forms);
        }

    }
}
