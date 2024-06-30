using StandardTest;
using System.Threading.Tasks;

namespace Housing.Services
{
    public interface ICityService
    {
        Task<DistInfo> GetDistInfoAsync(double lon, double lat);
        Task<List<NewsItem>> GetLatestNewsAsync();
        Task<StreetView> GetStreetViewInfoAsync(double lng, double lat);
        Task<int> AddFavoriteAsync(int userId, double lat, double lon, string note);
        Task<UserFavorite> GetUserFavoriteAsync(int userId);
        Task<int> DeleteUserFavoriteAsync(int favoriteId);
    }
}
