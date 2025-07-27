using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using RSpot.Booking.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RSpot.Booking.Infrastructure.Persistence
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options)
            : base(options) { }

        public DbSet<Domain.Models.Booking> Bookings { get; set; } = null!;
        public DbSet<WaitingList> WaitingLists { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Booking — составной ключ
            modelBuilder.Entity<Domain.Models.Booking>()
                .HasKey(b => new { b.WorkspaceId, b.UserId, b.StartTime });

            modelBuilder.Entity<Domain.Models.Booking>()
                .Property(b => b.UserId)
                .IsRequired();

            // WaitingList — составной ключ
            modelBuilder.Entity<WaitingList>()
                .HasKey(w => new { w.WorkspaceId, w.UserId, w.StartTime });

            modelBuilder.Entity<WaitingList>()
                .Property(b => b.UserId)
                .IsRequired();
        }
    }
}
