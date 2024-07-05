using Housing.Data;
using Housing.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StandardTest;

namespace Housing.Controllers
{
    [ApiController]
    [Route("api/v1/cityInfo")]
    public class NewsController : ControllerBase
    {
        private readonly IWebScraperService _webScraperService;
        private readonly ApplicationDbContext _context;

        public NewsController(IWebScraperService webScraperService, ApplicationDbContext context)
        {
            _webScraperService = webScraperService;
            _context = context;
        }

        [HttpGet("scrape")]
        public async Task<IActionResult> ScrapeHousingDynamics()
        {
            await _webScraperService.ScrapeAndSaveHousingDynamicsAsync();
            return Ok(new { Message = "Scraping completed", resultCode=1 });
        }

        [HttpGet("dynamics")]
        public async Task<IActionResult> GetHousingDynamics()
        {
            var dynamics = await _context.NewsItems.ToListAsync();
            return Ok(dynamics);
        }
    }
}
