using System.Linq;
using System.Threading.Tasks;
using CertificateManagementSystem.Models;
using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    public class SystemLogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SystemLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách tất cả các nhật ký hệ thống
        public async Task<IActionResult> Index()
        {
            var systemLogs = await _context.SystemLogs
                .Include(sl => sl.User)  // Bao gồm thông tin người dùng liên quan
                .OrderByDescending(sl => sl.Timestamp) // Sắp xếp theo thời gian giảm dần
                .ToListAsync();
            return View(systemLogs);
        }

        // Hiển thị chi tiết của một nhật ký
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var systemLog = await _context.SystemLogs
                .Include(sl => sl.User) // Bao gồm thông tin người dùng liên quan
                .FirstOrDefaultAsync(m => m.LogId == id);

            if (systemLog == null)
            {
                return NotFound();
            }

            return View(systemLog);
        }

        // Tạo mới nhật ký hệ thống (thông thường nhật ký sẽ được tạo tự động)
        // Tuy nhiên, nếu có yêu cầu từ người dùng, bạn có thể tạo nhật ký thủ công trong UI
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemLog systemLog)
        {
            if (ModelState.IsValid)
            {
                systemLog.Timestamp = DateTime.Now; // Ghi lại thời gian tạo nhật ký
                _context.Add(systemLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(systemLog);
        }

        // Hiển thị form xóa nhật ký hệ thống
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var systemLog = await _context.SystemLogs
                .FirstOrDefaultAsync(m => m.LogId == id);

            if (systemLog == null)
            {
                return NotFound();
            }

            return View(systemLog);
        }

        // Xử lý xóa nhật ký hệ thống
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var systemLog = await _context.SystemLogs.FindAsync(id);
            if (systemLog != null)
            {
                _context.SystemLogs.Remove(systemLog);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SystemLogExists(int id)
        {
            return _context.SystemLogs.Any(e => e.LogId == id);
        }
    }
}
