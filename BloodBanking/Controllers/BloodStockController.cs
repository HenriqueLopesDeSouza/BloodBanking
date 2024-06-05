using BloodBanking.Core.Entities;
using BloodBanking.Core.Enums;
using BloodBanking.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BloodBanking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloodStockController : ControllerBase
    {
        private readonly IBloodStockRepository _bloodStockRepository;

        public BloodStockController(IBloodStockRepository bloodStockRepository)
        {
            _bloodStockRepository = bloodStockRepository;
        }

        [HttpGet("{bloodType}/{rhFactor}")]
        public async Task<ActionResult<BloodStock>> GetBloodStock(BloodType bloodType, RhFactor rhFactor)
        {
            var bloodStock = await _bloodStockRepository.GetByBloodTypeAndRhFactorAsync(bloodType, rhFactor);
            if (bloodStock == null)
            {
                throw new ArgumentException($"No blood found for BloodType {bloodType} and RhFactor {rhFactor}.");
            }
            return Ok(bloodStock);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BloodStock>>> GetAllBloodStock()
        {
            var bloodStock = await _bloodStockRepository.GetAllAsync();
            return Ok(bloodStock);
        }

        [HttpPut("{bloodType}/{rhFactor}/decrease")]
        public async Task<IActionResult> DecreaseQuantityAsync(BloodType bloodType, RhFactor rhFactor, int quantityML)
        {
            try
            {
                await _bloodStockRepository.DecreaseQuantityAsync(bloodType, rhFactor, quantityML);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
