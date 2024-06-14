using JiraLikeSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace JiraLikeSystem.Models.Users;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Role { get; set; }
    public ICollection<ProjectTask> AssignedTasks { get; set; }
}
