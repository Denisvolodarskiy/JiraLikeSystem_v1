using JiraLikeSystem.Core.Interfaces;
using JiraLikeSystem.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace JiraLikeSystem.Core.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }
    public async Task AddToRoleAsync(ApplicationUser user, string role)
    {
        await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<ApplicationUser> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }
}

