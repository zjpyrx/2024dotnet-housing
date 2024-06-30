using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Housing1.Data;
using Housing1.Models;

namespace Housing1.Services
{
    public class CityService : ICityService
    {
        private readonly ApplicationDbContext _context;

        public CityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> LoginAsync(string userId, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId && u.Password == password);

            if (user == null)
            {
                return -101; // Illegal username or password
            }

            return 1; // Login successful
        }
    }
}
