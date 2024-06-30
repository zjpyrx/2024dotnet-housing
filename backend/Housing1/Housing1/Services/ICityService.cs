namespace Housing1.Services
{
    public interface ICityService
    {
        Task<int> LoginAsync(string userId, string password);
    }
}
