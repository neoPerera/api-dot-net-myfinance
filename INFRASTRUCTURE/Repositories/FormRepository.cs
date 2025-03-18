using APPLICATION.Interfaces;
using CORE.Entities;
using INFRASTRUCTURE.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Repositories
{
    public class FormRepository : IFormRepository
    {
        private readonly AppDbContext _context;

        public FormRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Form>> GetActiveFormsAsync()
        {
            // Query to get active forms from the tbl_forms table
            return await _context.Forms
                .Where(f => f.Active == 'Y') // Only active forms
                .ToListAsync(); // Execute the query asynchronously
        }
    }
}
