using JiraLikeSystem.Configurations;
using JiraLikeSystem.Models.Entities;
using JiraLikeSystem.Models.Users;
using JiraLikeSytem.Domain.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JiraLikeSystem.Data;


public class JiraLikeDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public JiraLikeDbContext(DbContextOptions<JiraLikeDbContext> options) : base(options)
    {

    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectTaskConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");
    }
}

