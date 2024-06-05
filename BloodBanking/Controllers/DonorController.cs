using BloodBanking.Application.IService;
using BloodBanking.Application.ViewModel;
using BloodBanking.Core.Entities;
using BloodBanking.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BloodBanking.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;
        private readonly IDonorRepository _donorRepository;
        private readonly IDonationRepository _donationRepository;

        public DonorController(IDonorService donorService, 
                               IDonorRepository donorRepository,
                               IDonationRepository donationRepository)
        {
            _donorRepository = donorRepository;
            _donorService = donorService;
            _donationRepository = donationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDonors()
        {
            var donors = await _donorRepository.GetAllDonorsAsync();
            return Ok(donors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonorById(Guid id)
        {
            var donor = await _donorRepository.GetDonorByIdAsync(id);
            if (donor == null)
                return NotFound();

            return Ok(donor);
        }

        [HttpPost]
        public async Task<IActionResult> AddDonor(CreateDonorViewModel donor)
        {
            try
            {
                await _donorService.AddDonorAsync(donor);

                return Ok("Donor added successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDonor(Guid idDonor)
        {
            try
            {
                var donor = await _donorRepository.GetDonorByIdAsync(idDonor);

                await _donorRepository.DeleteDonorAsync(donor.Id);

                return Ok("Donor Delete.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{donorId}/donations")]
        public async Task<ActionResult<IEnumerable<Donation>>> GetDonationHistory(Guid donorId)
        {
            if (donorId == Guid.Empty)
            {
                return BadRequest("Invalid donor ID.");
            }

            var donor = await _donorRepository.GetDonorByIdAsync(donorId);
            if (donor == null)
            {
                return NotFound($"No donor found with ID {donorId}.");
            }

            var donations = await _donationRepository.GetDonationsByDonorIdAsync(donorId);
            return Ok(donations);
        }
    }

}

