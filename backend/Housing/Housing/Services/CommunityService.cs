using Housing.Data;
using StandardTest;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Housing.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly ApplicationDbContext _context;

        public CommunityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Community>> SearchCommunitiesAsync(string keyword)
        {
            return await _context.Communities
                .Where(a => a.CommunityName.Contains(keyword) || a.Address.Contains(keyword))
                .ToListAsync();
        }

        public async Task<List<Community>> GetNearestCommunitiesAsync(double latitude, double longitude, int count)
        {
            string query = @"
                SELECT *
                FROM Community
                ORDER BY 6371 * ACOS(
                    COS(RADIANS({0})) * COS(RADIANS(Latitude)) * COS(RADIANS(Longitude) - RADIANS({1})) +
                    SIN(RADIANS({0})) * SIN(RADIANS(Latitude))
                )
                LIMIT {2}";

            var communities = await _context.Communities
                .FromSqlRaw(query, latitude, longitude, count)
                .ToListAsync();

            return communities;
        }
    }
}
