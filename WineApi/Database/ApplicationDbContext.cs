using Microsoft.EntityFrameworkCore;
using WineApi.Database.Entities;

namespace WineApi.Database
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
        public DbSet<Wine> Wines { get; set; }
    }
}
