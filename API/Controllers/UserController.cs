using API.Services;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UserController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            var (status, message) =await _authService.Registeration(model);

            if (status==1)
            {
                return Ok();
            }else 
            {
                return BadRequest(message);
            }
        }

        [HttpPost("login")]
        public async Task<LoginResult> Login(LoginModel model)
        {
            var res =await _authService.Login(model);

            //if (status==1)
            //{
                return res;
            //}else 
            //{
            //    return BadRequest(res);
            //}
        }

        [Authorize]
        [HttpPut("users/password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var (status, message) =await _authService.ChangePassword(model);

            if (status==1)
            {
                return Ok();
            }else 
            {
                return BadRequest(message);
            }
        }

        [Authorize]
        [HttpPut("users/profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid token");
            }
            var (status, message) =await _authService.UpdateProfile(model, userId);

            if (status==1)
            {
                return Ok();
            }
            else 
            {
                return BadRequest(message);
            }
        }
        [Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] UserFilter dto)
        {
            try
            {
                List<UserResult> result = await _userService.GetUsers(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
