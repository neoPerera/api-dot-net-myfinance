using CORE.Entities;
using CORE.Interfaces;
using INFRASTRUCTURE.Persistence;
using Microsoft.EntityFrameworkCore;

 
namespace INFRASTRUCTURE.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
             
    }
}
 