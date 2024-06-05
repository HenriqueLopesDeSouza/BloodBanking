using BloodBanking.Core.Entities;
using BloodBanking.Core.Enums;

namespace BloodBanking.Core.Repositories
{
    public interface IBloodStockRepository
    {
        Task<BloodStock> GetByIdAsync(Guid id);
        Task<IEnumerable<BloodStock>> GetAllAsync();
        Task<BloodStock> GetByBloodTypeAndRhFactorAsync(BloodType bloodType, RhFactor rhFactor);
        Task DecreaseQuantityAsync(BloodType bloodType, RhFactor rhFactor, int quantityML);

    }

}
