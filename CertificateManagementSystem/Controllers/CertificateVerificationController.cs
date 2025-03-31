using System.Linq;
using System.Threading.Tasks;
using CertificateManagementSystem.Models;
using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    public class CertificateVerificationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CertificateVerificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var verifications = await _context.CertificateVerifications
                                               .Include(c => c.Certificate)  // Optional: Include related certificate data
                                               .ToListAsync();
            return View(verifications);
        }

        public async Task<IActionResult> Details(int id)
        {
            var verification = await _context.CertificateVerifications
                                              .Include(c => c.Certificate)  // Optional: Include related certificate data
                                              .FirstOrDefaultAsync(m => m.VerificationId == id);
            if (verification == null)
            {
                return NotFound();
            }
            return View(verification);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CertificateVerification verification)
        {
            if (ModelState.IsValid)
            {
                _context.CertificateVerifications.Add(verification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(verification);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var verification = await _context.CertificateVerifications
                                              .Include(c => c.Certificate)  // Optional: Include related certificate data
                                              .FirstOrDefaultAsync(m => m.VerificationId == id);
            if (verification == null)
            {
                return NotFound();
            }
            return View(verification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CertificateVerification verification)
        {
            if (id != verification.VerificationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(verification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VerificationExists(verification.VerificationId))
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
            return View(verification);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var verification = await _context.CertificateVerifications
                                              .Include(c => c.Certificate)  // Optional: Include related certificate data
                                              .FirstOrDefaultAsync(m => m.VerificationId == id);
            if (verification == null)
            {
                return NotFound();
            }
            return View(verification);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var verification = await _context.CertificateVerifications.FindAsync(id);
            _context.CertificateVerifications.Remove(verification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VerificationExists(int id)
        {
            return _context.CertificateVerifications.Any(e => e.VerificationId == id);
        }
    }
}
