using System.Linq;
using System.Threading.Tasks;
using CertificateManagementSystem.Models;
using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    public class CertificateHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CertificateHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var histories = await _context.CertificateHistories.ToListAsync();
            return View(histories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var history = await _context.CertificateHistories.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }
            return View(history);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CertificateHistory history)
        {
            if (ModelState.IsValid)
            {
                _context.CertificateHistories.Add(history);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(history);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var history = await _context.CertificateHistories.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }
            return View(history);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CertificateHistory history)
        {
            if (id != history.HistoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(history);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoryExists(history.HistoryId))
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
            return View(history);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var history = await _context.CertificateHistories.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }
            return View(history);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var history = await _context.CertificateHistories.FindAsync(id);
            _context.CertificateHistories.Remove(history);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoryExists(int id)
        {
            return _context.CertificateHistories.Any(e => e.HistoryId == id);
        }
    }
}
