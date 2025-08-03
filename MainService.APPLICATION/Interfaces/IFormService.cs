using MainService.APPLICATION.DTOs;
using MainService.CORE.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainService.APPLICATION.Interfaces
{
    public interface IFormService
    {
        Task<List<FormResponse>> GetActiveFormsAsync(string userId);
    }
}