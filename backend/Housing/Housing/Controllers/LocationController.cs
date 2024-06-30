using Housing.Services;
using Microsoft.AspNetCore.Mvc;
using LocationTools;
using StandardTest;

namespace Housing.Controllers
{
    [ApiController]
    [Route("api/v1/location")]
    public class LocationController : ControllerBase
    {
        private readonly LocationGetter _locationGetter;

        public LocationController()
        {
            _locationGetter = new LocationGetter();
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentLocation([FromQuery] string ipAddress)
        {
            var location = await _locationGetter.GetLocationAsync(ipAddress);
            if (location == null)
            {
                return NotFound("Unable to get location data.");
            }

            return Ok(location);
        }

        [HttpGet("ip")]
        public async Task<IActionResult> GetIPAddress()
        {
            var ipAddress = await _locationGetter.GetIPAddressAsync();
            return Ok(new { ipAddress });
        }
    }


}
