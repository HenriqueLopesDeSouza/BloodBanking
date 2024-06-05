using BloodBanking.Application.Service;
using BloodBanking.Application.ViewModel;
using BloodBanking.Core.Entities;
using BloodBanking.Core.Enums;
using BloodBanking.Core.Repositories;
using Moq;

namespace BloodBanking.Teste.Services
{
    public class DonationServiceTests
    {
        private readonly Mock<IDonationRepository> _donationRepositoryMock;
        private readonly Mock<IDonorRepository> _donorRepositoryMock;
        private readonly DonationService _donationService;

        public DonationServiceTests()
        {
            _donationRepositoryMock = new Mock<IDonationRepository>();
            _donorRepositoryMock = new Mock<IDonorRepository>();
            _donationService = new DonationService(_donationRepositoryMock.Object, _donorRepositoryMock.Object);

            // Seed initial data if necessary
            SeedData();
        }

        private void SeedData()
        {
            var donors = new List<Donor>
        {
            new Donor
            {
                Id = Guid.NewGuid(),
                FullName = "John Doe",
                Email = "new.donor@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                Weight = 70,
                BloodType = BloodType.A,
                RhFactor = RhFactor.Positive,
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "Anystate",
                    ZipCode = "12345"
                }
            },
            new Donor
            {
                Id = Guid.NewGuid(),
                FullName = "Jane Doe",
                Email = "new.donor@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                Weight = 60,
                BloodType = BloodType.B,
                RhFactor = RhFactor.Negative,
                Address = new Address
                {
                    Street = "456 Elm St",
                    City = "Othertown",
                    State = "Otherstate",
                    ZipCode = "67890"
                }
            }
        };

            _donorRepositoryMock.Setup(repo => repo.GetDonorByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Guid id) => donors.FirstOrDefault(d => d.Id == id));
            _donationRepositoryMock.Setup(repo => repo.GetLastDonationAsync(It.IsAny<Guid>())).ReturnsAsync((Guid id) => null);
        }

        [Fact]
        public async Task AddDonationAsync_WithValidDonation_ShouldAddDonation()
        {
            // Arrange
            var donor = new Donor
            {
                Id = Guid.NewGuid(),
                FullName = "John Doe",
                Email = "new.donor@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                Weight = 70,
                BloodType = BloodType.A,
                RhFactor = RhFactor.Positive,
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "Anystate",
                    ZipCode = "12345"
                }
            };

            var donationViewModel = new CreateDonationViewModel
            {
                DonorId = donor.Id,
                DonationDate = DateTime.Now,
                QuantityML = 450
            };

            _donorRepositoryMock.Setup(repo => repo.GetDonorByIdAsync(donationViewModel.DonorId)).ReturnsAsync(donor);
            _donationRepositoryMock.Setup(repo => repo.GetLastDonationAsync(donor.Id)).ReturnsAsync((Donation)null);

            // Act
            await _donationService.AddDonationAsync(donationViewModel);

            // Assert
            _donationRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Donation>()), Times.Once);
        }

        [Fact]
        public async Task AddDonationAsync_WithNonExistentDonor_ShouldThrowArgumentException()
        {
            // Arrange
            var donationViewModel = new CreateDonationViewModel
            {
                DonorId = Guid.NewGuid(),
                DonationDate = DateTime.Now,
                QuantityML = 450
            };

            _donorRepositoryMock.Setup(repo => repo.GetDonorByIdAsync(donationViewModel.DonorId)).ReturnsAsync((Donor)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _donationService.AddDonationAsync(donationViewModel));
            Assert.Equal($"No donor found with ID {donationViewModel.DonorId}.", exception.Message);
        }

        [Fact]
        public async Task AddDonationAsync_WithUnderageDonor_ShouldThrowArgumentException()
        {
            // Arrange
            var donor = new Donor
            {
                Id = Guid.NewGuid(),
                FullName = "Underage Donor",
                Email = "new.donor@example.com",
                DateOfBirth = DateTime.Now.AddYears(-17), // Underage donor
                Gender = Gender.Male,
                Weight = 70,
                BloodType = BloodType.A,
                RhFactor = RhFactor.Positive,
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "Anystate",
                    ZipCode = "12345"
                }
            };

            var donationViewModel = new CreateDonationViewModel
            {
                DonorId = donor.Id,
                DonationDate = DateTime.Now,
                QuantityML = 450
            };

            _donorRepositoryMock.Setup(repo => repo.GetDonorByIdAsync(donationViewModel.DonorId)).ReturnsAsync(donor);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _donationService.AddDonationAsync(donationViewModel));
            Assert.Equal("The donor is not old enough. Minimum age required: 18 years.", exception.Message);
        }

        [Fact]
        public void ValidateDonationQuantityRange_WithInvalidQuantity_ShouldThrowArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _donationService.ValidateDonationQuantityRange(400));
            Assert.Equal("The quantity of blood donated must be between 420ml and 470ml. (Parameter 'quantityML')", exception.Message);
        }

        [Fact]
        public async Task AddDonationAsync_WithTooSoonDonation_ShouldThrowArgumentException()
        {
            // Arrange
            var donor = new Donor
            {
                Id = Guid.NewGuid(),
                FullName = "Jane Doe",
                Email = "new.donor@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Female,
                Weight = 70,
                BloodType = BloodType.B,
                RhFactor = RhFactor.Negative,
                Address = new Address
                {
                    Street = "456 Elm St",
                    City = "Othertown",
                    State = "Otherstate",
                    ZipCode = "67890"
                }
            };

            var donationViewModel = new CreateDonationViewModel
            {
                DonorId = donor.Id,
                DonationDate = DateTime.Now,
                QuantityML = 450
            };

            var lastDonation = new Donation
            {
                DonorId = donor.Id,
                DonationDate = DateTime.Now.AddDays(-30), // Too soon for a female donor
                QuantityML = 450,
                Donor = donor
            };

            _donorRepositoryMock.Setup(repo => repo.GetDonorByIdAsync(donationViewModel.DonorId)).ReturnsAsync(donor);
            _donationRepositoryMock.Setup(repo => repo.GetLastDonationAsync(donor.Id)).ReturnsAsync(lastDonation);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _donationService.AddDonationAsync(donationViewModel));
            Assert.Equal($"Donation interval not respected for {donor.Gender}. The next donation can only occur after 90 days.", exception.Message);
        }
    }
}
