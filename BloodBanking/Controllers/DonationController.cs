using BloodBanking.Application.IService;
using BloodBanking.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BloodBanking.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class DonationController :  ControllerBase
    {
        private readonly IDonationService _donationService;

        public DonationController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        [HttpPost]
        public async Task<IActionResult> AddDonation(CreateDonationViewModel donor)
        {
            try
            {
                await _donationService.AddDonationAsync(donor);

                return Ok("Donation added successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
