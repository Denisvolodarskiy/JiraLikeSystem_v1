using JiraLikeSystem.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace JiraLikeSystem.Core.Interfaces;

public interface IUserService
{
    Task AddToRoleAsync(ApplicationUser user, string role);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
    Task<ApplicationUser> FindByEmailAsync(string email);
}