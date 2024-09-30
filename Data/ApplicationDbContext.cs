using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BusTrackingSystem.Models;

namespace BusTrackingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<ApplicationUser>()
                .Property(u => u.Balance)
                .HasColumnType("decimal(18,2)"); 
        }

        public DbSet<Bus> Buses { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
}