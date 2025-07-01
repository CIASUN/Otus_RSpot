using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using RSpot.Booking.Infrastructure.Models;

namespace RSpot.Booking.Infrastructure.Persistence
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options)
            : base(options) { }

        public DbSet<Models.Booking> Bookings { get; set; } = null!;
        public DbSet<WaitingList> WaitingLists { get; set; } = null!;
        public DbSet<Workspace> Workspaces { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Booking — составной ключ
            modelBuilder.Entity<Models.Booking>()
                .HasKey(b => new { b.WorkspaceId, b.UserId, b.StartTime });

            modelBuilder.Entity<Models.Booking>()
                .HasOne(b => b.Workspace)
                .WithMany(w => w.Bookings)
                .HasForeignKey(b => b.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.Booking>()
                .Property(b => b.UserId)
                .IsRequired();

            // WaitingList — составной ключ
            modelBuilder.Entity<WaitingList>()
                .HasKey(w => new { w.WorkspaceId, w.UserId, w.StartTime });

            modelBuilder.Entity<WaitingList>()
                .HasOne(w => w.Workspace)
                .WithMany(w => w.WaitingLists)
                .HasForeignKey(w => w.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WaitingList>()
                .Property(b => b.UserId)
                .IsRequired();
        }
    }
}
