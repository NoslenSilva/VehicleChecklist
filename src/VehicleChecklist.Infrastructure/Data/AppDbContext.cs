using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using VehicleChecklist.Domain.Entities;

namespace VehicleChecklist.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Checklist> Checklists => Set<Checklist>();
        public DbSet<ChecklistItem> ChecklistItems => Set<ChecklistItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<Vehicle>(b =>
            {
                b.HasKey(v => v.Id);
                b.HasIndex(v => v.Plate).IsUnique();
            });

            modelBuilder.Entity<Checklist>(b =>
            {
                b.HasKey(c => c.Id);
                b.HasOne(c => c.Vehicle).WithMany().HasForeignKey(c => c.VehicleId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(c => c.StartedBy).WithMany().HasForeignKey(c => c.StartedById).OnDelete(DeleteBehavior.Restrict);
                b.HasMany(c => c.Items).WithOne(i => i.Checklist).HasForeignKey(i => i.ChecklistId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ChecklistItem>(b =>
            {
                b.HasKey(i => i.Id);
            });
        }
    }
}
