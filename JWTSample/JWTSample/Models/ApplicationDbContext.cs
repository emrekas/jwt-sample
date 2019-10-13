using Microsoft.EntityFrameworkCore;

namespace JWTSample.Models
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=jwtSample.db");
    }
}
