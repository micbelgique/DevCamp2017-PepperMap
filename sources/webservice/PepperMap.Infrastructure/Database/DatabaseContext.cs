using Microsoft.EntityFrameworkCore;
using PepperMap.Infrastructure.Database.Models;

namespace PepperMap.Infrastructure.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Route> Routes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Route>().ToTable("Routes");
            modelBuilder.Entity<Location>().ToTable("Locations");
            modelBuilder.Entity<Person>().ToTable("People");
        }
    }
}
