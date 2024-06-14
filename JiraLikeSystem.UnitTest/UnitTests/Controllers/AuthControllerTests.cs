using JiraLikeSystem.Core.Interfaces;
using JiraLikeSystem.Core.Models;
using JiraLikeSystem.Core.Models.AuthModels;
using JiraLikeSystem.Models.Users;
using JiraLikeSystem.WebApi.Authentication.JwtToken;
using JiraLikeSystem.WebApi.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace JiraLikeSystem.Tests.UnitTests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<IJwtTokenService> _mockJwtTokenService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _mockJwtTokenService = new Mock<IJwtTokenService>();
        _controller = new AuthController(_mockUserService.Object, _mockUserManager.Object, _mockJwtTokenService.Object);
    }

    [Fact]
    public async Task Register_ReturnsOkResult_WhenUserRegisteredSuccessfully()
    {
        var registerModel = new RegisterModel { Email = "test@example.com", Password = "Password123", Role = "User" };
        var user = new ApplicationUser { UserName = registerModel.Email, Email = registerModel.Email, Role = registerModel.Role };
        var identityResult = IdentityResult.Success;

        _mockUserService.Setup(service => service.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(identityResult);
        _mockUserService.Setup(service => service.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .Returns(Task.CompletedTask);


        var result = await _controller.Register(registerModel);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<RegisterResponseModel>(okResult.Value);

        Assert.NotNull(returnValue);
        Assert.Equal("User registered successfully!", returnValue.Message);
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenUserRegistrationFails()
    {
        var registerModel = new RegisterModel { Email = "test@example.com", Password = "Password123", Role = "User" };
        var errors = new IdentityError[] { new IdentityError { Description = "Error" } };
        var identityResultFailed = IdentityResult.Failed(errors);

        _mockUserService.Setup(service => service.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(identityResultFailed);

        var result = await _controller.Register(registerModel);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(errors, badRequestResult.Value);
    }

    [Fact]
    public async Task Login_ReturnsOkResult_WithToken_WhenLoginIsSuccessful()
    {
        var loginModel = new LoginModel { Email = "test@example.com", Password = "Password123" };
        var user = new ApplicationUser { UserName = loginModel.Email, Email = loginModel.Email };
        var token = "test_token";

        _mockUserService.Setup(service => service.FindByEmailAsync(loginModel.Email)).ReturnsAsync(user);
        _mockUserService.Setup(service => service.CheckPasswordAsync(user, loginModel.Password)).ReturnsAsync(true);
        _mockJwtTokenService.Setup(service => service.GenerateToken(user)).Returns(token);


        var result = await _controller.Login(loginModel);

      
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<AuthResponseModel>(okResult.Value);

        Assert.NotNull(returnValue);
        Assert.Equal(token, returnValue.Token);
        Assert.True(returnValue.Expiration > DateTime.Now);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
    {
 
        var loginModel = new LoginModel { Email = "test@example.com", Password = "Password123" };
        _mockUserService.Setup(service => service.FindByEmailAsync(loginModel.Email)).ReturnsAsync((ApplicationUser)null);

        var result = await _controller.Login(loginModel);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenPasswordIsIncorrect()
    {
        var loginModel = new LoginModel { Email = "test@example.com", Password = "Password123" };
        var user = new ApplicationUser { UserName = loginModel.Email, Email = loginModel.Email };

        _mockUserService.Setup(service => service.FindByEmailAsync(loginModel.Email)).ReturnsAsync(user);
        _mockUserService.Setup(service => service.CheckPasswordAsync(user, loginModel.Password)).ReturnsAsync(false);

        var result = await _controller.Login(loginModel);

        Assert.IsType<UnauthorizedResult>(result);
    }
}

