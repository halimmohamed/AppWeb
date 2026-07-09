using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using WebAppSafeJourney.Models;

namespace WebAppSafeJourney.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Guide> Guides { get; set; }
        public DbSet<GuideLanguage> GuideLanguages { get; set; }
        public DbSet<GuideRegion> GuideRegions { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<TripBooking> TripBookings { get; set; }
        public DbSet<GuideBooking> GuideBookings { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // علاقات 1:1
            modelBuilder.Entity<Guide>()
                .HasOne(g => g.User).WithOne(u => u.GuideProfile)
                .HasForeignKey<Guide>(g => g.UserId);
            // تطبيق قاعدة "منع الحذف" على كل الجداول (Restrict)
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // استثناء الجداول التابعة للحذف المتسلسل (Cascade)
            modelBuilder.Entity<GuideLanguage>()
                .HasOne(gl => gl.Guide).WithMany(g => g.Languages)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GuideRegion>()
                .HasOne(gr => gr.Guide).WithMany(g => g.CoveredRegions)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}