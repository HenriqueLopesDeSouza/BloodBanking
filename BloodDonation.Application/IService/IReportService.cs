using BloodBanking.Application.ViewModel;
namespace BloodBanking.Application.IService
{
    public interface IReportService
    {
        Task<IEnumerable<BloodTypeReportViewModel>> GetTotalBloodByTypeAsync();
        Task<IEnumerable<DonationReportViewModel>> GetDonationsLast30DaysAsync();

        Task<byte[]> GenerateBloodTypeReportPdfAsync();
        Task<byte[]> GenerateDonationsLast30DaysReportPdfAsync();
    }
}
