using JiraLikeSystem.Models.Users;

namespace JiraLikeSystem.WebApi.Authentication.JwtToken
{
    public interface IJwtTokenService
    {
        string GenerateToken(ApplicationUser user);
    }
}