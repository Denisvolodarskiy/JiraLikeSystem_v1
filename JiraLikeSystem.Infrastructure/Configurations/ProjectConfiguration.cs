using JiraLikeSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JiraLikeSystem.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasMany(p => p.ProjectTasks)
                .WithOne(pt => pt.Project)
                .HasForeignKey(pt => pt.ProjectId);
        }
    }
}
