using Microsoft.EntityFrameworkCore;
using StellumbraSite.Server.Models;

namespace StellumbraSite.Server.Services
{
    public class AppDBContext
    {
        public class AppDbContext : DbContext
        {
            protected readonly IConfiguration Configuration;

            public AppDbContext(IConfiguration configuration)
            {
                Configuration = configuration;
            }

            protected override void OnConfiguring(DbContextOptionsBuilder options)
            {
                // connect to postgres with connection string from app settings
                options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
            }

            public DbSet<UserProfile> Profiles { get; set; }
            public DbSet<ForumThread> Threads { get; set; }
            public DbSet<ForumPost> Posts { get; set; }
        }
    }
}
