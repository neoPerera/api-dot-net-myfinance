using APPLICATION.DTOs;
using CORE.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APPLICATION.Interfaces
{
    public interface IFormService
    {
        Task<List<FormResponse>> GetActiveFormsAsync(string userId);
    }
}