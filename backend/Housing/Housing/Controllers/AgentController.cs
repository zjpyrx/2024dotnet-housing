using StandardTest;
using Housing.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Housing.Controllers
{
    [ApiController]
    [Route("api/v1/agents")]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;

        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Agent>>> SearchAgents(string keyword)
        {
            var agents = await _agentService.SearchAgentsAsync(keyword);
            return Ok(agents);
        }

        [HttpGet("nearest")]
        public async Task<ActionResult<List<Agent>>> GetNearestAgents(double latitude, double longitude, int count = 5)
        {
            var agents = await _agentService.GetNearestAgentsAsync(latitude, longitude, count);
            return Ok(agents);
        }
    }
}
