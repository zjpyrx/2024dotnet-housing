using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NewsTools.Models;

namespace NewsTools.Services
{
    public class WebScraperService
    {
        private readonly HttpClient _httpClient;

        public WebScraperService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NewsItem>> ScrapeHousingDynamicsAsync()
        {
            var url = "https://fgj.sh.gov.cn/ywdt/index.html";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var pageContents = await response.Content.ReadAsStringAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(pageContents);

            var nodes = doc.DocumentNode.SelectNodes("//ul[@class='uli14 nowrapli list-date lisn lisn01 lisn01-qu']/li");

            var newsItems = new List<NewsItem>();

            foreach (var node in nodes)
            {
                var titleNode = node.SelectSingleNode(".//a");
                var dateNode = node.SelectSingleNode(".//span");

                var newsItem = new NewsItem
                {
                    NewsName = titleNode.GetAttributeValue("title", ""),
                    NewsTime = DateTime.ParseExact(dateNode.InnerText.Trim(), "yyyy.MM.dd", null),
                    NewsLink = "https://fgj.sh.gov.cn" + titleNode.GetAttributeValue("href", "")
                };

                newsItems.Add(newsItem);
            }

            return newsItems;
        }
    }
}
