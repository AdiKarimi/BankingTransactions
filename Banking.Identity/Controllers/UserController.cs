using Banking.Identity.Models;
using Banking.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Identity.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] Login loginParam)
        {
            var token = _userService.Authenticate(loginParam.Username, loginParam.Password);

            if (token == null)
                return BadRequest(new { auth_token = "Username or password is incorrect" });

            return Ok(token);
        }
    }
}
