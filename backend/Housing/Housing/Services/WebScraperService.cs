using Housing.Data;
using StandardTest;
using Microsoft.EntityFrameworkCore;
using NewsTools.Models;
using NewsTools.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Housing.Services
{
    public class WebScraperService : IWebScraperService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;

        public WebScraperService(HttpClient httpClient, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task ScrapeAndSaveHousingDynamicsAsync()
        {
            var scraper = new NewsTools.Services.WebScraperService(_httpClient);
            List<NewsTools.Models.NewsItem> newsToolsItems = await scraper.ScrapeHousingDynamicsAsync();

            // Convert NewsTools.Models.NewsItem to Housing.Models.NewsItem
            List<StandardTest.NewsItem> housingNewsItems = newsToolsItems.Select(item => new StandardTest.NewsItem
            {
                NewsName = item.NewsName,
                NewsTime = item.NewsTime,
                NewsLink = item.NewsLink
            }).ToList();

            foreach (var newsItem in housingNewsItems)
            {
                if (!await NewsExistsAsync(newsItem))
                {
                    _context.NewsItems.Add(newsItem);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task<bool> NewsExistsAsync(StandardTest.NewsItem newsItem)
        {
            return await _context.NewsItems.AnyAsync(n =>
                n.NewsName == newsItem.NewsName &&
                n.NewsTime == newsItem.NewsTime);
        }
    }
}
