using Microsoft.AspNetCore.Mvc;
using Housing1.Services;

namespace Housing1.Controllers
{
    [ApiController]
    [Route("api/v1/cityInfo")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string userId, string password)
        {
            Console.WriteLine("hello");
            var result = await _cityService.LoginAsync(userId, password);
            if (result == 1)
            {
                return Ok(new { message = "Login successful" });
            }
            return BadRequest(new { message = "Illegal username or password" });

        }
    }
}
