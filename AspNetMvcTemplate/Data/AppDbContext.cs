using AspNetMvcTemplate.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetMvcTemplate.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<long>, long>
    {
        public DbSet<AppMetadata> AppMetadata { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
