using System.Linq;
using System.Threading.Tasks;
using CertificateManagementSystem.Models;
using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    public class SystemConfigurationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SystemConfigurationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách cấu hình hệ thống
        public async Task<IActionResult> Index()
        {
            var configurations = await _context.SystemConfigurations.ToListAsync();
            return View(configurations);
        }

        // Hiển thị chi tiết cấu hình hệ thống
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemConfiguration = await _context.SystemConfigurations
                .FirstOrDefaultAsync(m => m.ConfigId == id);

            if (systemConfiguration == null)
            {
                return NotFound();
            }

            return View(systemConfiguration);
        }

        // Hiển thị form tạo cấu hình hệ thống mới
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemConfiguration systemConfiguration)
        {

                _context.Add(systemConfiguration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

        }

        // Hiển thị form chỉnh sửa cấu hình hệ thống
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemConfiguration = await _context.SystemConfigurations.FindAsync(id);
            if (systemConfiguration == null)
            {
                return NotFound();
            }
            return View(systemConfiguration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, SystemConfiguration systemConfiguration)
        {
            if (id != systemConfiguration.ConfigId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(systemConfiguration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemConfigurationExists(systemConfiguration.ConfigId))
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
            return View(systemConfiguration);
        }

        // Hiển thị form xác nhận xóa cấu hình hệ thống
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemConfiguration = await _context.SystemConfigurations
                .FirstOrDefaultAsync(m => m.ConfigId == id);

            if (systemConfiguration == null)
            {
                return NotFound();
            }

            return View(systemConfiguration);
        }

        // Xóa cấu hình hệ thống
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var systemConfiguration = await _context.SystemConfigurations.FindAsync(id);
            _context.SystemConfigurations.Remove(systemConfiguration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemConfigurationExists(string id)
        {
            return _context.SystemConfigurations.Any(e => e.ConfigId == id);
        }
    }
}
