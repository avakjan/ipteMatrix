using Microsoft.AspNetCore.Mvc;
using PeopleSkillsApp.Services;

namespace PeopleSkillsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // For simplicity, we're hardcoding the admin username and password.
            // You can replace this with a database check.
            if (model.Username == "a" && model.Password == "a")
            {
                var token = TokenService.GenerateToken(model.Username, isAdmin: true);
                return Ok(new { token });
            }

            return Unauthorized("Invalid username or password");
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}