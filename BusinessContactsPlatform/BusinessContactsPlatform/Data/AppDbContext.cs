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
        public DbSet<ContactRequest> ContactRequests { get; set; }
        public DbSet<SocialMedia> SocialMedia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .Property(e => e.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Event>()
                .HasMany(e => e.EventUsers)
                .WithOne(eu => eu.Event)
                .HasForeignKey(eu => eu.EventId);
        }
    }
}
