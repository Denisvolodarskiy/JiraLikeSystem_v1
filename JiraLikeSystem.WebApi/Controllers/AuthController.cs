using JiraLikeSystem.Core.Interfaces;
using JiraLikeSystem.Core.Models;
using JiraLikeSystem.Core.Models.AuthModels;
using JiraLikeSystem.Models.Users;
using JiraLikeSystem.WebApi.Authentication.JwtToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JiraLikeSystem.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    public AuthController(IUserService userService, UserManager<ApplicationUser> userManager , IJwtTokenService jwtTokenService)
    {
        _userService = userService;
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            Role = model.Role,
        };

        var result = await _userService.CreateUserAsync(user, model.Password); 

        if (result.Succeeded)
        {
            await _userService.AddToRoleAsync(user, model.Role);

            var response = new RegisterResponseModel
            {
                Message = "User registered successfully!"
            };

            return Ok(response);
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userService.FindByEmailAsync(model.Email);
        if (user != null && await _userService.CheckPasswordAsync(user, model.Password))
        {
            var token = _jwtTokenService.GenerateToken(user);

            var response = new AuthResponseModel
            {
                Token = token,
                Expiration = DateTime.Now.AddMinutes(30)
            };
            return Ok(response);
        }
        return Unauthorized();
    }
}
