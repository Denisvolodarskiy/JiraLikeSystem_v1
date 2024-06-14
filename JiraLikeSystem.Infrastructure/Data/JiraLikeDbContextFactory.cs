using JiraLikeSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace JiraLikeSytem.Infrastructure.Data;

public class JiraLikeDbContextFactory : IDesignTimeDbContextFactory<JiraLikeDbContext>
{
    public JiraLikeDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<JiraLikeDbContext>();

        var connectionString = "Server=(localdb)\\LocalDbStorage;Database=JiraLikeDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        optionsBuilder.UseSqlServer(connectionString);

        return new JiraLikeDbContext(optionsBuilder.Options);
    }
}
