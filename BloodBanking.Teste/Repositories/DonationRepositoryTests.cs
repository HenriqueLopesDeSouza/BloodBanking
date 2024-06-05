using BloodBanking.Core.Entities;
using BloodBanking.Core.Enums;
using BloodBanking.Infrastructure.Persistence;
using BloodBanking.Infrastructure.Persistence.Repositories;
using BloodBanking.Teste.Util;
using Microsoft.EntityFrameworkCore;
namespace BloodBanking.Teste.Repositories
{
    public class DonationRepositoryTests
    {
        private readonly BloodDonationDbContext _context;
        private readonly DonationRepository _repository;

        public DonationRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BloodDonationDbContext>()
                .UseInMemoryDatabase(databaseName: "BloodDonationDb")
                .Options;
            _context = new BloodDonationDbContext(options);
            DatabaseSeeder.Seed(_context);

            _repository = new DonationRepository(_context);
        }


        [Fact]
        public async Task AddAsync_ShouldAddDonationAndIncreaseBloodStock()
        {
            // Arrange
            var donor = new Donor
            {
                Id = Guid.NewGuid(),
                FullName = "John Smith",
                Email = "john.smith@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                Weight = 70,
                BloodType = BloodType.A,
                RhFactor = RhFactor.Positive,
                Address = new Address
                {
                    Street = "456 Elm St",
                    City = "Othertown",
                    State = "Otherstate",
                    ZipCode = "67890"
                }
            };
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();

            var donation = new Donation
            {
                Id = Guid.NewGuid(),
                DonorId = donor.Id,
                Donor = donor,
                DonationDate = DateTime.Now,
                QuantityML = 500
            };

            // Act
            await _repository.AddAsync(donation);
            var savedDonation = await _context.Donations.FindAsync(donation.Id);
            var bloodStock = await _context.BloodStocks
                .FirstOrDefaultAsync(bs => bs.BloodType == donor.BloodType && bs.RhFactor == donor.RhFactor);

            // Assert
            Assert.NotNull(savedDonation);
            Assert.NotNull(bloodStock);
            Assert.Equal(donation.QuantityML, bloodStock.QuantityML);
        }

        [Fact]
        public async Task GetDonorByIdAsync_ShouldReturnDonation()
        {
            // Arrange
            var donation = _context.Donations.First();

            // Act
            var result = await _repository.GetDonorByIdAsync(donation.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(donation.Id, result.Id);
        }

        [Fact]
        public async Task GetAllDonorsAsync_ShouldReturnAllDonations()
        {
            // Act
            var result = await _repository.GetAllDonorsAsync();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetLastDonationAsync_ShouldReturnLastDonation()
        {
            // Arrange
            var donor = _context.Donors
                .Where(d => d.Donations.Any()) 
                .FirstOrDefault();
            // Act
            var result = await _repository.GetLastDonationAsync(donor.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(donor.Id, result.DonorId);
        }

        [Fact]
        public async Task GetDonationsByDonorIdAsync_ShouldReturnDonations()
        {
            // Arrange
            var donor = new Donor
            {
                Id = Guid.NewGuid(),
                FullName = "Alice Johnson",
                Email = "alice.johnson@example.com",
                DateOfBirth = new DateTime(1992, 6, 15),
                Gender = Gender.Female,
                Weight = 60,
                BloodType = BloodType.O,
                RhFactor = RhFactor.Positive,
                Address = new Address
                {
                    Street = "456 Elm St",
                    City = "Othertown",
                    State = "Otherstate",
                    ZipCode = "67890"
                }
            };
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();

            var donation1 = new Donation
            {
                Id = Guid.NewGuid(),
                DonorId = donor.Id,
                Donor = donor,
                DonationDate = DateTime.Now.AddDays(-10),
                QuantityML = 500
            };
            var donation2 = new Donation
            {
                Id = Guid.NewGuid(),
                DonorId = donor.Id,
                Donor = donor,
                DonationDate = DateTime.Now.AddDays(-5),
                QuantityML = 450
            };
            _context.Donations.AddRange(donation1, donation2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetDonationsByDonorIdAsync(donor.Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetDonationsAfterDateAsync_ShouldReturnDonations()
        {
            // Arrange
            var dateFrom = DateTime.Now.AddDays(-30);

            // Act
            var result = await _repository.GetDonationsAfterDateAsync(dateFrom);

            // Assert
            Assert.NotNull(result);
        }
    }

}

