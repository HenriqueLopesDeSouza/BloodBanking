using BloodBanking.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BloodBanking.Infrastructure.Persistence
{
    public class BloodDonationDbContext : DbContext
    {
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<BloodStock> BloodStocks { get; set; }

        public BloodDonationDbContext(DbContextOptions<BloodDonationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring the Donor and Donations relationship
            modelBuilder.Entity<Donation>()
                .HasOne(d => d.Donor)
                .WithMany(d => d.Donations)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuring the Donor and Address relationship
            modelBuilder.Entity<Donor>()
                .HasOne(d => d.Address)
                .WithOne(a => a.Donor)
                .HasForeignKey<Donor>(d => d.AddressId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
