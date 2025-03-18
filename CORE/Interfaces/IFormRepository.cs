using CORE.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APPLICATION.Interfaces
{
    public interface IFormRepository
    {
        Task<List<Form>> GetActiveFormsAsync();
    }
}