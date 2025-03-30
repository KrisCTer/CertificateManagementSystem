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

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                CitizensCount = _context.Citizens?.Count() ?? 0,
                CertificatesCount = _context.Certificates?.Count() ?? 0,
                PendingRequestsCount = _context.CertificateRequests?.Count(r => r.Status == "Pending") ?? 0,
                RecentVerificationsCount = _context.CertificateVerifications?.Count() ?? 0
            };

            // Kiểm tra model có dữ liệu không
            if (model == null)
            {
                return NotFound("Dữ liệu không tồn tại!");
            }

            return View(model);
        }
    }

}
