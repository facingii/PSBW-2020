using EmployeesWebService.Helpers;
using EmployeesWebService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesWebService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private IUserService _userService;

        public UsersController (IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate ([FromBody] User userParam)
        {
            var user = _userService.Authenticate (userParam.UserName, userParam.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or Password is incorrect!" });
            }

            return Ok (user);
        }

        [HttpGet]
        public IActionResult GetAll ()
        {
            var users = _userService.GetAll ();
            return Ok (users);
        }
    }
}
