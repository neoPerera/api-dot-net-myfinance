using APPLICATION.DTOs;
using APPLICATION.Interfaces;
using CORE.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APPLICATION.Services
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepository;

        public FormService(IFormRepository formRepository)
        {
            _formRepository = formRepository;
        }

        public async Task<List<FormResponse>> GetActiveFormsAsync(string userId)
        {
            // Get active forms from repository
            var forms =  await _formRepository.GetActiveFormsAsync();
            var mappedforms = forms.Select(form => new FormResponse
            {
                str_form_id = form.FormId,
                str_form_name = form.FormName,
                str_menu_id = form.MenuId ?? "",  // Default to empty string if null
                str_icon = form.Icon ?? "",      // Default to empty string if null
                str_link = form.Link ?? "",      // Default to empty string if null
                str_component = form.Component ?? "",  // Default to empty string if null
                str_isMenu = form.IsMenu,
                str_active = form.Active
            }).ToList();

            return mappedforms; 

        }
    }
}
