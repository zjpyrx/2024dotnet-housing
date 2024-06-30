using Housing.Data;
using StandardTest;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Runtime;
using GeoLibCppCli;


namespace Housing.Services
{
    public class CityService : ICityService
    {
        private readonly ApplicationDbContext _context;
        private const double EarthRadiusKm = 6371;

        public CityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DistInfo> GetDistInfoAsync(double Lng, double Lat)
        {
            var distInfo = new DistInfo();

            try
            {
                distInfo.SchoolDist = await GetNearDist(Lat, Lng, 502);
                distInfo.ParkDist = await GetNearDist(Lat, Lng, 505);
                distInfo.HospitalDist = await GetNearDist(Lat, Lng, 503);
                distInfo.FactoryDist = await GetNearDist(Lat, Lng, 301);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                distInfo.ResultCode = -1;
                return distInfo;
            }

            distInfo.ResultCode = 1;
            return distInfo;
        }

        private async Task<double> GetNearDist(double lat, double lon, int code)
        {
            var landUse = await _context.LandUses
                .FromSqlRaw("SELECT UUID, Lon, Lat, F_AREA, Level1, Level2 " +
                            "FROM LandUse " +
                            "WHERE Level2 = {0} " +
                            "ORDER BY 6371 * ACOS(" +
                            "    COS(RADIANS({1})) * COS(RADIANS(Lat)) * COS(RADIANS(Lon) - RADIANS({2})) + " +
                            "    SIN(RADIANS({1})) * SIN(RADIANS(Lat))" +
                            ") ASC " +
                            "LIMIT 1", code, lat, lon)
                .FirstOrDefaultAsync();

            return GeoCalculator.GetDist(landUse.Lon, landUse.Lat, lon, lat);
        }

        public async Task<int> AddFavoriteAsync(int userId, double lat, double lon, string note)
        {
            try
            {
                var favorite = new Favorite
                {
                    UserId = userId,
                    Longitude = lon,
                    Latitude = lat,
                    Note = note
                };

                var allFavorites = await _context.Favorites.ToListAsync();
                favorite.FavoriteId = allFavorites.Count == 0 ? 0 : await _context.Favorites.MaxAsync(f => f.FavoriteId) + 1;

                // Set distances (example)
                favorite.Distance0101 = await GetNearDist(lat, lon, 101);
                favorite.Distance0201 = await GetNearDist(lat, lon, 201);
                favorite.Distance0202 = await GetNearDist(lat, lon, 202);
                favorite.Distance0301 = await GetNearDist(lat, lon, 301);
                favorite.Distance0401 = 1;
                favorite.Distance0402 = await GetNearDist(lat, lon, 402);
                favorite.Distance0403 = await GetNearDist(lat, lon, 403);
                favorite.Distance0501 = await GetNearDist(lat, lon, 501);
                favorite.Distance0502 = await GetNearDist(lat, lon, 502);
                favorite.Distance0503 = await GetNearDist(lat, lon, 503);
                favorite.Distance0504 = await GetNearDist(lat, lon, 504);
                favorite.Distance0505 = await GetNearDist(lat, lon, 505);

                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();

                return 1;
            }
            catch (Exception)
            {
                return -404;
            }
        }

        public async Task<List<NewsItem>> GetLatestNewsAsync()
        {
            return await _context.NewsItems
                .OrderByDescending(news => news.NewsTime)
                .Take(5)
                .ToListAsync();
        }

        public async Task<StreetView> GetStreetViewInfoAsync(double lng, double lat)
        {
            var result = await _context.StreetViews
                .FromSqlRaw("SELECT * FROM street_view ORDER BY 6371 * ACOS(COS(RADIANS({0})) * COS(RADIANS(lat)) * COS(RADIANS(lng) - RADIANS({1})) + SIN(RADIANS({0})) * SIN(RADIANS(lat))) ASC LIMIT 1", lat, lng)
                .ToListAsync();

            return result.FirstOrDefault();
        }

        

        public async Task<UserFavorite> GetUserFavoriteAsync(int userId)
        {
            var userFavorite = new UserFavorite();
            try
            {
                userFavorite.Value = await _context.Favorites.Where(f => f.UserId == userId).ToListAsync();
                userFavorite.ResultCode = 1;
                return userFavorite;
            }
            catch (Exception)
            {
                userFavorite.ResultCode = -404;
                return userFavorite;
            }
        }

        public async Task<int> DeleteUserFavoriteAsync(int favoriteId)
        {
            try
            {
                var favorite = await _context.Favorites.FindAsync(favoriteId);
                if (favorite != null)
                {
                    _context.Favorites.Remove(favorite);
                    await _context.SaveChangesAsync();
                    return 1;
                }
                return -404;
            }
            catch (Exception)
            {
                return -404;
            }
        }

    }
}
