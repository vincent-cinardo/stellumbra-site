using StellumbraSite.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace StellumbraSite.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<ForumThread> ForumThreads { get; set; }
    }
}
