using BloodBanking.Application.IService;
using BloodBanking.Application.ViewModel;
using BloodBanking.Core.Repositories;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace BloodBanking.Application.Service
{
    public class ReportService : IReportService
    {
        private readonly IBloodStockRepository _bloodStockRepository;
        private readonly IDonationRepository _donationRepository;


        public ReportService(IBloodStockRepository bloodStockRepository, IDonationRepository donationRepository)
        {
            _bloodStockRepository = bloodStockRepository;
            _donationRepository = donationRepository;
        }
        public async Task<IEnumerable<BloodTypeReportViewModel>> GetTotalBloodByTypeAsync()
        {
            var bloodStocks = await _bloodStockRepository.GetAllAsync();

            return bloodStocks
                .GroupBy(b => new { b.BloodType, b.RhFactor })
                .Select(g => new BloodTypeReportViewModel
                {
                    BloodType = g.Key.BloodType,
                    RhFactor = g.Key.RhFactor,
                    TotalQuantity = g.Sum(b => b.QuantityML)
                })
                .ToList();
        }

        public async Task<IEnumerable<DonationReportViewModel>> GetDonationsLast30DaysAsync()
        {
            var dateFrom = DateTime.Now.AddDays(-30);
            var donations = await _donationRepository.GetDonationsAfterDateAsync(dateFrom);

            return donations
                .Select(d => new DonationReportViewModel
                {
                    DonationDate = d.DonationDate,
                    QuantityML = d.QuantityML,
                    DonorFullName = d.Donor.FullName,
                    DonorEmail = d.Donor.Email,
                    DonorBloodType = d.Donor.BloodType.ToString(),
                    DonorRhFactor = d.Donor.RhFactor.ToString()
                })
                .ToList();
        }


        public async Task<byte[]> GenerateBloodTypeReportPdfAsync()
        {
            try
            {
                var reportData = await GetTotalBloodByTypeAsync();

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    document.Add(new Paragraph("Blood Type Report"));
                    document.Add(new Paragraph($"Generated on: {DateTime.Now}"));

                    foreach (var data in reportData)
                    {
                        document.Add(new Paragraph($"Blood Type: {data.BloodType}"));
                        document.Add(new Paragraph($"Rh Factor: {data.RhFactor}"));
                        document.Add(new Paragraph($"Total Quantity: {data.TotalQuantity}ml"));

                    }

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving report data.", ex);
            }
        }

        public async Task<byte[]> GenerateDonationsLast30DaysReportPdfAsync()
        {
            try
            {
                var reportData = await GetDonationsLast30DaysAsync();
                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    document.Add(new Paragraph("Donations Last 30 Days Report"));
                    document.Add(new Paragraph($"Generated on: {DateTime.Now}"));

                    foreach (var data in reportData)
                    {
                        document.Add(new Paragraph($"Donation Date: {data.DonationDate}, Quantity: {data.QuantityML}ml, Donor: {data.DonorFullName}, Email: {data.DonorEmail}, Blood Type: {data.DonorBloodType}, Rh Factor: {data.DonorRhFactor}"));
                    }

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving report data.", ex);

            }
        }

    }
}

