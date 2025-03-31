using System.Linq;
using System.Threading.Tasks;
using CertificateManagementSystem.Models;
using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    public class CertificateTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CertificateTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var certificateTypes = await _context.CertificateTypes.ToListAsync();
            return View(certificateTypes);
        }

        public async Task<IActionResult> Details(string id)
        {
            var certificateType = await _context.CertificateTypes.FindAsync(id);
            if (certificateType == null)
            {
                return NotFound();
            }
            return View(certificateType);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CertificateType certificateType)
        {
            if (ModelState.IsValid)
            {
                _context.CertificateTypes.Add(certificateType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(certificateType);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var certificateType = await _context.CertificateTypes.FindAsync(id);
            if (certificateType == null)
            {
                return NotFound();
            }
            return View(certificateType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CertificateType certificateType)
        {
            if (id != certificateType.CertificateTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(certificateType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CertificateTypeExists(certificateType.CertificateTypeId))
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
            return View(certificateType);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var certificateType = await _context.CertificateTypes.FindAsync(id);
            if (certificateType == null)
            {
                return NotFound();
            }
            return View(certificateType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var certificateType = await _context.CertificateTypes.FindAsync(id);
            _context.CertificateTypes.Remove(certificateType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CertificateTypeExists(string id)
        {
            return _context.CertificateTypes.Any(e => e.CertificateTypeId == id);
        }
    }
}
