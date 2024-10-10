using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Data.Entities.Identity;
using StoreApi.Interfaces;
using StoreApi.Models.Account;

namespace StoreApi.Controllers
{
    public class AccountController : Controller
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly UserManager<UserEntity> _userManager;
            private IJwtTokenService _jwtTokenService;
            public AuthController(UserManager<UserEntity> userManager, IJwtTokenService jwtTokenService)
            {
                _userManager = userManager;
                _jwtTokenService = jwtTokenService;
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] LoginViewModel model)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                        return Unauthorized("Invalid data");

                    var token = _jwtTokenService.GenerateToken(user);
                    return Ok(new { token });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        }

    }
}