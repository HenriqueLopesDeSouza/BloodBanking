using BloodBanking.Application.IService;
using BloodBanking.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BloodBanking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("total-blood-by-type")]
        public async Task<ActionResult<IEnumerable<BloodTypeReportViewModel>>> GetTotalBloodByType()
        {
            try
            {
                var report = await _reportService.GetTotalBloodByTypeAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while getting total blood by type. ", error = ex.Message });
            }

        }


        [HttpGet("donations-last-30-days")]
        public async Task<ActionResult<IEnumerable<DonationReportViewModel>>> GetDonationsLast30Days()
        {
            try
            {
                var report = await _reportService.GetDonationsLast30DaysAsync();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while getting donations for the last 30 days.", error = ex.Message });
            }
        }

        [HttpGet("total-blood-by-type/pdf")]
        public async Task<IActionResult> GetTotalBloodByTypePdf()
        {
            try
            {
                var pdfData = await _reportService.GenerateBloodTypeReportPdfAsync();
                return File(pdfData, "application/pdf", "TotalBloodByTypeReport.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while generating the total blood by type PDF.", error = ex.Message });
            }
        }

        [HttpGet("donations-last-30-days/pdf")]
        public async Task<IActionResult> GetDonationsLast30DaysPdf()
        {
            try
            {
                var pdfData = await _reportService.GenerateDonationsLast30DaysReportPdfAsync();
                return File(pdfData, "application/pdf", "DonationsLast30DaysReport.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while generating the donations last 30 days PDF.", error = ex.Message });
            }
        }

    }
}
