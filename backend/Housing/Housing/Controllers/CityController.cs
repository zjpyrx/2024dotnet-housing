using StandardTest;
using Housing.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Runtime.Loader;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Housing.Controllers
{

    public class GeoCalculatorLoader : AssemblyLoadContext
    {
        private AssemblyDependencyResolver _resolver;

        public GeoCalculatorLoader(string assemblyPath)
        {
            _resolver = new AssemblyDependencyResolver(assemblyPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
            return null;
        }
    }


    [ApiController]
    [Route("api/v1/cityInfo")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        [DllImport("D:\\Work\\大三下\\.Net\\期末\\proj\\GeoLibCPP\\x64\\Debug\\GeoLibCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int add(int a, int b);

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

            [HttpGet("getDistInfo")]
        public async Task<IActionResult> GetDistInfo(double lon, double lat)
        {
            var distInfo = await _cityService.GetDistInfoAsync(lon, lat);

            if (distInfo.ResultCode == -1)
            {
                return BadRequest(new { message = "Information retrieval failed" });
            }

            return Ok(new
            {
                message = "Information retrieval successful",
                schoolDist = distInfo.SchoolDist,
                parkDist = distInfo.ParkDist,
                hospitalDist = distInfo.HospitalDist,
                factoryDist = distInfo.FactoryDist,
                resultCode = 1
            });
        }

        [HttpGet("testdll")]
        public IActionResult TestDll()
        {
            try
            {
                var dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DLL", "GeoLibCppCli.dll");
                Console.WriteLine($"Loading assembly from: {dllPath}");

                // 使用自定义的 AssemblyLoadContext 加载 DLL
                var loadContext = new GeoCalculatorLoader(dllPath);
                var assembly = loadContext.LoadFromAssemblyPath(dllPath);

                var geoCalculatorType = assembly.GetType("GeoLibCppCli.GeoCalculator");
                if (geoCalculatorType == null)
                {
                    Console.WriteLine("GeoCalculator type not found in assembly.");
                    return StatusCode(500, "GeoCalculator type not found.");
                }

                var getDistMethod = geoCalculatorType.GetMethod("GetDist");
                if (getDistMethod == null)
                {
                    Console.WriteLine("GetDist method not found in GeoCalculator type.");
                    return StatusCode(500, "GetDist method not found.");
                }

                // 创建 GeoCalculator 实例
                var geoCalculatorInstance = Activator.CreateInstance(geoCalculatorType);

                // 调用 GetDist 方法
                double result = (double)getDistMethod.Invoke(geoCalculatorInstance, new object[] { 121.0, 31.0, 121.0, 31.0 });

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error invoking GetDist: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("cityNews")]
        public async Task<IActionResult> GetNews()
        {
            var newsItems = await _cityService.GetLatestNewsAsync();
            return Ok(newsItems);
        }

        [HttpGet("getStreetViewInfo")]
        public async Task<IActionResult> GetStreetViewInfo(double Lng, double Lat)
        {
            var streetView = await _cityService.GetStreetViewInfoAsync(Lng, Lat);
            if (streetView == null)
            {
                return BadRequest("信息获取失败");
            }
            return Ok(streetView);
        }

        [HttpGet("addFavorite")]
        public async Task<IActionResult> AddFavorite(int userId, double lat, double lon, string note)
        {
            var result = await _cityService.AddFavoriteAsync(userId, lat, lon, note);
            if (result == 1)
            {
                return Ok(new { message = "Favorite added successfully", resultCode = 1 });
            }
            return BadRequest(new { message = "Error adding favorite", resultCode = -404 });
        }

        [HttpGet("getUserFavorite")]
        public async Task<IActionResult> GetUserFavorite(int userId)
        {
            var userFavorite = await _cityService.GetUserFavoriteAsync(userId);
            if (userFavorite.ResultCode == 1)
            {
                return Ok(userFavorite);
            }
            return BadRequest(new { message = "Error fetching favorites", resultCode = -404 });
        }

        [HttpDelete("deleteUserFavorite")]
        public async Task<IActionResult> DeleteUserFavorite(int favoriteId)
        {
            var result = await _cityService.DeleteUserFavoriteAsync(favoriteId);
            if (result == 1)
            {
                return Ok(new { message = "Favorite deleted successfully", resultCode = 1 });
            }
            return BadRequest(new { message = "Error deleting favorite", resultCode = -404 });
        }
    }
}
