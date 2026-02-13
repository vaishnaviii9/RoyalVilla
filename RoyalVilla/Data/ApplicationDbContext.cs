using Microsoft.EntityFrameworkCore;
using RoyalVilla.Models;

namespace RoyalVilla.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Villa> Villa { get; set; }
        public DbSet<User> Users {get; set;}
        public DbSet<VillaAmenities> VillaAmenities {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Villa>().HasData(
       new Villa
       {
           Id = 1,
           Name = "Royal Villa",
           Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
           ImageUrl = "https://placehold.co/600x400",
           Occupancy = 4,
           Rate = 200,
           Sqft = 550
       },
       new Villa
       {
           Id = 2,
           Name = "Premium Pool Villa",
           Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
           ImageUrl = "https://placehold.co/600x401",
           Occupancy = 4,
           Rate = 300,
           Sqft = 550
       },
       new Villa
       {
           Id = 3,
           Name = "Luxury Pool Villa",
           Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
           ImageUrl = "https://placehold.co/600x402",
           Occupancy = 4,
           Rate = 400,
           Sqft = 750
       },
       new Villa
       {
           Id = 4,
           Name = "Garden View Villa",
           Details = "Villa surrounded by lush gardens with peaceful ambiance.",
           ImageUrl = "https://placehold.co/600x403",
           Occupancy = 3,
           Rate = 180,
           Sqft = 500
       },
       new Villa
       {
           Id = 5,
           Name = "Presidential Villa",
           Details = "Ultra-luxury villa with private pool, butler service, and ocean view.",
           ImageUrl = "https://placehold.co/600x404",
           Occupancy = 10,
           Rate = 900,
           Sqft = 3000
       }
   );

        }
    }
}
