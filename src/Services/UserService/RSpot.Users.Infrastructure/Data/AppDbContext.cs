namespace RSpot.Users.Infrastructure.Data
{
    using System.Reflection.Emit;
    using Microsoft.EntityFrameworkCore;
    using RSpot.Users.Domain.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<WaitingList> WaitingLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Organization → Workspace
            modelBuilder.Entity<Workspace>()
                .HasOne(w => w.Organization)
                .WithMany(o => o.Workspaces)
                .HasForeignKey(w => w.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Workspace → Booking
            modelBuilder.Entity<Booking>()
                .HasKey(b => new { b.WorkspaceId, b.UserId, b.StartTime });

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Workspace)
                .WithMany(w => w.Bookings)
                .HasForeignKey(b => b.WorkspaceId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            // Workspace → WaitingList
            modelBuilder.Entity<WaitingList>()
                .HasKey(w => new { w.WorkspaceId, w.UserId, w.StartTime });

            modelBuilder.Entity<WaitingList>()
                .HasOne(w => w.Workspace)
                .WithMany(ws => ws.WaitingLists)
                .HasForeignKey(w => w.WorkspaceId);

            modelBuilder.Entity<WaitingList>()
                .HasOne(w => w.User)
                .WithMany(u => u.WaitingLists)
                .HasForeignKey(w => w.UserId);
        }
    }

}
