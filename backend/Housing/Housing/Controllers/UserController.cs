using Housing.Services;
using Microsoft.AspNetCore.Mvc;
using StandardTest;

namespace Housing.Controllers
{
    [ApiController]
    [Route("api/v1/cityInfo")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string userId, string password)
        {
            var result = await _userService.LoginAsync(userId, password);
            if (result == 1)
            {
                return Ok(new { message = "Login successful", resultCode = 1 });
            }
            return BadRequest(new { message = "Illegal username or password" });
        }

        [HttpGet("register")]
        public async Task<IActionResult> Register(string userId, string name, string password)
        {
            var result = await _userService.RegisterAsync(userId, name, password);

            if (result == -102)
            {
                return BadRequest(new { message = "User ID already registered" });
            }
            if (result == -404)
            {
                return StatusCode(500, new { message = "Special fault" });
            }
            return Ok(new { message = "Registration successful", resultCode = 1 });
        }
    }
}
