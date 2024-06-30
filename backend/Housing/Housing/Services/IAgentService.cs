using StandardTest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Housing.Services
{
    public interface IAgentService
    {
        Task<List<Agent>> SearchAgentsAsync(string keyword);
        Task<List<Agent>> GetNearestAgentsAsync(double latitude, double longitude, int count);
    }
}
