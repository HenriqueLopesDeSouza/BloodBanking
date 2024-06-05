using BloodBanking.Core.Entities;
using BloodBanking.Core.Enums;
using BloodBanking.Infrastructure.Persistence.Repositories;
using BloodBanking.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using BloodBanking.Core.DomainEvents;

namespace BloodBanking.Teste.Repositories
{
    public class BloodStockRepositoryTests
    {
        private readonly BloodDonationDbContext _context;
        private readonly BloodStockRepository _repository;
        private readonly Mock<IMediator> _mediatorMock;

        public BloodStockRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<BloodDonationDbContext>()
                .UseInMemoryDatabase(databaseName: "BloodDonationDb")
                .Options;
            _context = new BloodDonationDbContext(options);
            _mediatorMock = new Mock<IMediator>();
            _repository = new BloodStockRepository(_context, _mediatorMock.Object);

            // Seed the database with initial data for testing
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var bloodStocks = new List<BloodStock>
            {
                new BloodStock
                {
                    Id = Guid.NewGuid(),
                    BloodType = BloodType.A,
                    RhFactor = RhFactor.Positive,
                    QuantityML = 200
                },
                new BloodStock
                {
                    Id = Guid.NewGuid(),
                    BloodType = BloodType.B,
                    RhFactor = RhFactor.Negative,
                    QuantityML = 150
                }
            };

            _context.BloodStocks.AddRange(bloodStocks);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnBloodStock()
        {
            // Arrange
            var bloodStock = _context.BloodStocks.First();

            // Act
            var result = await _repository.GetByIdAsync(bloodStock.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bloodStock.Id, result.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBloodStocks()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByBloodTypeAndRhFactorAsync_ShouldReturnBloodStock()
        {
            // Arrange
            var bloodType = BloodType.A;
            var rhFactor = RhFactor.Positive;

            // Act
            var result = await _repository.GetByBloodTypeAndRhFactorAsync(bloodType, rhFactor);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bloodType, result.BloodType);
            Assert.Equal(rhFactor, result.RhFactor);
        }

        [Fact]
        public async Task DecreaseQuantityAsync_ShouldDecreaseBloodStockQuantity()
        {
            // Arrange
            var bloodType = BloodType.A;
            var rhFactor = RhFactor.Positive;
            var quantityML = 200;
            var bloodStock = await _repository.GetByBloodTypeAndRhFactorAsync(bloodType, rhFactor);
            var quantityMLbloodStock =  bloodStock.QuantityML;
            // Act
            await _repository.DecreaseQuantityAsync(bloodType, rhFactor, quantityML);
            var updatedBloodStock = await _repository.GetByBloodTypeAndRhFactorAsync(bloodType, rhFactor);

            // Assert
            Assert.Equal(quantityMLbloodStock - quantityML, updatedBloodStock.QuantityML);
        }

        [Fact]
        public async Task DecreaseQuantityAsync_ShouldThrowException_WhenQuantityIsGreaterThanStock()
        {
            // Arrange
            var bloodType = BloodType.A;
            var rhFactor = RhFactor.Positive;
            var quantityML = 250; // More than available stock

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _repository.DecreaseQuantityAsync(bloodType, rhFactor, quantityML));
        }

        [Fact]
        public async Task DecreaseQuantityAsync_ShouldPublishLowStockEvent_WhenStockIsLow()
        {
            // Arrange
            var bloodType = BloodType.A;
            var rhFactor = RhFactor.Positive;
            var quantityML = 150; // This will reduce stock to 50, which is below the threshold

            // Act
            await _repository.DecreaseQuantityAsync(bloodType, rhFactor, quantityML);

            // Assert
            _mediatorMock.Verify(m => m.Publish(It.IsAny<LowBloodStockEvent>(), default), Times.Once);
        }
    }
}
