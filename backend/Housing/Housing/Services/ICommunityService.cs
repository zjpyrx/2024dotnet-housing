using StandardTest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Housing.Services
{
    public interface ICommunityService
    {
        Task<List<Community>> SearchCommunitiesAsync(string keyword);
        Task<List<Community>> GetNearestCommunitiesAsync(double latitude, double longitude, int count);
    }
}
