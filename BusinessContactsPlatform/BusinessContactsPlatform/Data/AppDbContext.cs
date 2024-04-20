using BusinessContactsPlatform.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BusinessContactsPlatform.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventUser> UserEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .Property(e => e.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Event>()
                .HasMany(e => e.EventUsers) // An Event can have many EventUsers
                .WithOne(eu => eu.Event) // An EventUser belongs to only one Event
                .HasForeignKey(eu => eu.EventId); // Define the foreign key property name in the EventUser entity
        }
    }
}
