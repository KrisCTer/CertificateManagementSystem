using CertificateManagementSystem.Models;
using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CertificateManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var dashboardData = new DashboardViewModel
            {
                CitizensCount = _context.Citizens.Count(),
                CertificatesCount = _context.Certificates.Count(),
                PendingRequestsCount = _context.CertificateRequests.Count(r => r.Status == "Pending"), // Adjust condition as needed
                RecentVerificationsCount = _context.CertificateVerifications.Count(v => v.VerificationDate >= DateTime.Now.AddMonths(-1)) // Example condition for recent verifications
            };

            return View(dashboardData);
        }
    }
}
