
using JiraLikeSystem.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JiraLikeSystem.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            //builder.HasKey(u => u.Id);
            //builder.HasMany(u => u.AssignedTasks)
            //    .WithOne(pt => pt.AssignedTo)
            //    .HasForeignKey(pt => pt.UserId);

            //builder.HasMany(u => u.AssignedTasks)
            //  .WithOne(pt => pt.AssignedTo)
            //  .HasForeignKey(pt => pt.UserId);

            builder.HasKey(u => u.Id);
            builder.HasMany(u => u.AssignedTasks)
                .WithOne(pt => pt.AssignedTo)
                .HasForeignKey(pt => pt.UserId);
        }
    }
}
