namespace Housing.Services
{
    public interface IUserService
    {
        Task<int> LoginAsync(string userId, string password);
        Task<int> RegisterAsync(string userId, string name, string password);
    }
}
