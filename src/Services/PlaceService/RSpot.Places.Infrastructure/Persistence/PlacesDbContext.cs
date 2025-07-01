using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using RSpot.Places.Infrastructure.Models;

namespace RSpot.Places.Infrastructure.Persistence
{
    public class PlacesDbContext : DbContext
    {
        public PlacesDbContext(DbContextOptions<PlacesDbContext> options)
        : base(options) { }

        public DbSet<Organization> Organizations { get; set; } = null!;
        public DbSet<Workspace> Workspaces { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Organization → Workspace (один-ко-многим)
            modelBuilder.Entity<Workspace>()
                .HasOne(w => w.Organization)
                .WithMany(o => o.Workspaces)
                .HasForeignKey(w => w.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Уникальное имя организации
            modelBuilder.Entity<Organization>()
                .HasIndex(o => o.Name)
                .IsUnique();
        }
    }
}
