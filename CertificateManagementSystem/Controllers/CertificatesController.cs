using System.Linq;
using System.Threading.Tasks;
using CertificateManagementSystem.Models;
using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    public class CertificateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CertificateController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var certificates = await _context.Certificates.Include(c => c.Citizen).Include(c => c.CertificateType).Include(c => c.IssuingInstitution).ToListAsync();
            return View(certificates);
        }

        public async Task<IActionResult> Details(string id)
        {
            var certificate = await _context.Certificates.Include(c => c.Citizen).Include(c => c.CertificateType).Include(c => c.IssuingInstitution).FirstOrDefaultAsync(c => c.CertificateId == id);
            if (certificate == null)
            {
                return NotFound();
            }
            return View(certificate);
        }

        public IActionResult Create()
        {
            // Get the list of CertificateTypes from the database
            var certificateTypes = _context.CertificateTypes.ToList();

            // Populate the CertificateTypeId dropdown with the CertificateTypeId as the value and CertificateTypeName as the text
            ViewBag.CertificateTypeId = new SelectList(certificateTypes, "CertificateTypeId", "CertificateTypeName"); // adjust property names as necessary

            // Similarly populate other dropdowns
            ViewBag.CitizenId = new SelectList(_context.Citizens, "CitizenId", "FullName");
            ViewBag.IssuingInstitutionId = new SelectList(_context.EducationalInstitutions, "InstitutionId", "InstitutionName");

            return View();
        }






        // POST: Certificates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Certificate certificate)
        {

            _context.Add(certificate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Redirect to list view after creating the certificate
        }

        public async Task<IActionResult> Edit(string id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate == null)
            {
                return NotFound();
            }
            return View(certificate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Certificate certificate)
        {
            if (id != certificate.CertificateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(certificate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CertificateExists(certificate.CertificateId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(certificate);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate == null)
            {
                return NotFound();
            }
            return View(certificate);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> CertificateStatistics()
        {
            // Lấy thống kê số lượng chứng chỉ theo loại
            var statistics = await _context.Certificates
                .GroupBy(c => c.CertificateType.CertificateTypeName)
                .Select(g => new
                {
                    CertificateType = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return View(statistics);
        }
        private bool CertificateExists(string id)
        {
            return _context.Certificates.Any(e => e.CertificateId == id);
        }
    }
}
