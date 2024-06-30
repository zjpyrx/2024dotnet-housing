using Housing.Data;
using StandardTest;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Housing.Services
{
    public class AgentService : IAgentService
    {
        private readonly ApplicationDbContext _context;

        public AgentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Agent>> SearchAgentsAsync(string keyword)
        {
            return await _context.Agents
                .Where(a => a.AgentName.Contains(keyword) || a.Address.Contains(keyword))
                .ToListAsync();
        }

        public async Task<List<Agent>> GetNearestAgentsAsync(double latitude, double longitude, int count)
        {
            string query = @"
                SELECT *
                FROM Agents
                ORDER BY 6371 * ACOS(
                    COS(RADIANS({0})) * COS(RADIANS(Latitude)) * COS(RADIANS(Longitude) - RADIANS({1})) +
                    SIN(RADIANS({0})) * SIN(RADIANS(Latitude))
                )
                LIMIT {2}";

            var agents = await _context.Agents
                .FromSqlRaw(query, latitude, longitude, count)
                .ToListAsync();

            return agents;
        }
    }
}
