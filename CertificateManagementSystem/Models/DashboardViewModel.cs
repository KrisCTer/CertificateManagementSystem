namespace CertificateManagementSystem.Models
{
    public class DashboardViewModel
    {
        public int CitizensCount { get; set; } = 0;
        public int CertificatesCount { get; set; } = 0;
        public int PendingRequestsCount { get; set; } = 0;
        public int RecentVerificationsCount { get; set; } = 0;
    }
}
