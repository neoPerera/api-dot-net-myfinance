using APPLICATION.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IFormService _formService;

        public HomeController(IFormService formService)
        {
            _formService = formService;
        }

        [HttpGet("getForms")]
        public async Task<IActionResult> GetActiveForms(string userId)
        {
            var forms = await _formService.GetActiveFormsAsync(userId);
            return Ok(forms);
        }

    }
}
