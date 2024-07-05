namespace Housing.Services
{
    public interface IWebScraperService
    {
        Task ScrapeAndSaveHousingDynamicsAsync();
    }
}
