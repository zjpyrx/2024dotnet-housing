using Housing.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using StandardTest;

namespace Housing.Controllers
{
    [ApiController]
    [Route("api/v1/communities")]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityService _communityService;

        public CommunityController(ICommunityService communityService)
        {
            _communityService = communityService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCommunities(string keyword)
        {
            var communities = await _communityService.SearchCommunitiesAsync(keyword);
            return Ok(communities);
        }

        [HttpGet("nearest")]
        public async Task<IActionResult> GetNearestCommunities(double latitude, double longitude, int count)
        {
            var communities = await _communityService.GetNearestCommunitiesAsync(latitude, longitude, count);
            return Ok(communities);
        }
    }
}
