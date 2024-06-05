using BloodBanking.Core.Entities;
using BloodBanking.Core.Enums;
using BloodBanking.Infrastructure.Persistence.Repositories;
using BloodBanking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BloodBanking.Teste.Util;

namespace BloodBanking.Teste.Repositories
{
    public class DonorRepositoryTests
    {
        private readonly BloodDonationDbContext _context;
        private readonly DonorRepository _repository;

        public DonorRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BloodDonationDbContext>()
                .UseInMemoryDatabase(databaseName: "BloodDonationDb")
                .Options;
            _context = new BloodDonationDbContext(options);

            DatabaseSeeder.Seed(_context);

            _repository = new DonorRepository(_context);

        }

        [Fact]
        public async Task EmailExist_ShouldReturnTrueIfEmailExists()
        {
            // Arrange
            var donor = new Donor
            {
                FullName = "Existing Donor",
                Email = "johndoe@example.com"
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _repository.EmailExist(donor));
        }

        [Fact]
        public async Task AddDonorAsync_ShouldAddDonor()
        {
            // Arrange
            var donor = new Donor
            {
                Id = Guid.NewGuid(),
                FullName = "New Donor",
                Email = "new.donor@example.com",
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

            // Act
            await _repository.AddDonorAsync(donor);
            var savedDonor = await _context.Donors.FindAsync(donor.Id);

            // Assert
            Assert.NotNull(savedDonor);
            Assert.Equal(donor.Email, savedDonor.Email);
        }


        [Fact]
        public async Task GetDonorByIdAsync_ShouldReturnDonor()
        {
            // Arrange
            var donor = _context.Donors.First();

            // Act
            var result = await _repository.GetDonorByIdAsync(donor.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(donor.Id, result.Id);
        }

        [Fact]
        public async Task GetAllDonorsAsync_ShouldReturnAllDonors()
        {
            // Act
            var result = await _repository.GetAllDonorsAsync();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteDonorAsync_ShouldRemoveDonor()
        {
            // Arrange
            var donor = _context.Donors.First();

            // Act
            await _repository.DeleteDonorAsync(donor.Id);
            var deletedDonor = await _context.Donors.FindAsync(donor.Id);

            // Assert
            Assert.Null(deletedDonor);
        }
    }
}
