using JiraLikeSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JiraLikeSystem.Configurations
{
    public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.HasKey(pt => pt.Id);
            builder.HasOne(pt => pt.AssignedTo)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(pt => pt.UserId)
                .IsRequired(false);
        }
    }
}
