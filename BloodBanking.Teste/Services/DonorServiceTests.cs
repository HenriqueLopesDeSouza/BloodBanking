using BloodBanking.Application.Service;
using BloodBanking.Application.ViewModel;
using BloodBanking.Core.Entities;
using BloodBanking.Core.Enums;
using BloodBanking.Core.Repositories;
using Moq;

namespace BloodBanking.Teste.Services
{
    public class DonorServiceTests
    {
        private readonly Mock<IDonorRepository> _donorRepositoryMock;
        private readonly DonorService _donorService;

        public DonorServiceTests()
        {
            _donorRepositoryMock = new Mock<IDonorRepository>();
            _donorService = new DonorService(_donorRepositoryMock.Object);
        }

        [Fact]
        public async Task AddDonorAsync_WithValidDonor_ShouldAddDonor()
        {
            // Arrange
            var createDonorViewModel = new CreateDonorViewModel
            {
                FullName = "John Doe",
                Email = "johndoe@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                Weight = 70,
                BloodType = BloodType.A,
                RhFactor = RhFactor.Positive,
                Address = new CreateAddressViewModel
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "Anystate",
                    ZIPCode = "12345"
                }
            };

            // Act
            await _donorService.AddDonorAsync(createDonorViewModel);

            // Assert
            _donorRepositoryMock.Verify(repo => repo.AddDonorAsync(It.IsAny<Donor>()), Times.Once);
        }

        [Fact]
        public async Task AddDonorAsync_WithExistingEmail_ShouldThrowArgumentException()
        {
            // Arrange
            var createDonorViewModel = new CreateDonorViewModel
            {
                FullName = "Jane Doe",
                Email = "janedoe@example.com",
                DateOfBirth = new DateTime(1985, 5, 15),
                Gender = Gender.Female,
                Weight = 65,
                BloodType = BloodType.B,
                RhFactor = RhFactor.Negative,
                Address = new CreateAddressViewModel
                {
                    Street = "456 Elm St",
                    City = "Othertown",
                    State = "Otherstate",
                    ZIPCode = "67890"
                }
            };

            var existingDonor = new Donor
            {
                Id = Guid.NewGuid(),
                FullName = "Jane Doe",
                Email = "janedoe@example.com",
                DateOfBirth = new DateTime(1985, 5, 15),
                Gender = Gender.Female,
                Weight = 65,
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

            _donorRepositoryMock.Setup(repo => repo.EmailExist(It.IsAny<Donor>())).Throws(new ArgumentException("A donor with the same email already exists."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _donorService.AddDonorAsync(createDonorViewModel));
            Assert.Equal("A donor with the same email already exists.", exception.Message);
        }

     
        [Fact]
        public async Task DeleteDonorAsync_WithValidDonorId_ShouldDeleteDonor()
        {
            // Arrange
            var donorId = Guid.NewGuid();

            var existingDonor = new Donor
            {
                Id = donorId,
                FullName = "John Doe",
                Email = "johndoe@example.com",
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

            _donorRepositoryMock.Setup(repo => repo.GetDonorByIdAsync(donorId)).ReturnsAsync(existingDonor);

            // Act
            await _donorService.DeleteDonorAsync(donorId);

            // Assert
            _donorRepositoryMock.Verify(repo => repo.DeleteDonorAsync(donorId), Times.Once);
        }
    }
}
