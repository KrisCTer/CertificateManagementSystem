using System.Linq;
using System.Threading.Tasks;
using CertificateManagementSystem.Models;
using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    public class CertificateRequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CertificateRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị tất cả các yêu cầu chứng nhận
        public async Task<IActionResult> Index()
        {
            var certificateRequests = await _context.CertificateRequests
                .Include(cr => cr.Citizen)
                .Include(cr => cr.CertificateType)
                .Include(cr => cr.Processor)
                .OrderByDescending(cr => cr.SubmissionDate)
                .ToListAsync();

            return View(certificateRequests);
        }

        // Hiển thị chi tiết yêu cầu chứng nhận
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var certificateRequest = await _context.CertificateRequests
                .Include(cr => cr.Citizen)
                .Include(cr => cr.CertificateType)
                .Include(cr => cr.Processor)
                .Include(cr => cr.Attachments) // Bao gồm các tài liệu đính kèm
                .FirstOrDefaultAsync(m => m.RequestId == id);

            if (certificateRequest == null)
            {
                return NotFound();
            }

            return View(certificateRequest);
        }

        // Tạo yêu cầu chứng nhận mới
        public IActionResult Create()
        {
            ViewData["CitizenId"] = new SelectList(_context.Citizens, "CitizenId", "FullName");
            ViewData["CertificateTypeId"] = new SelectList(_context.CertificateTypes, "CertificateTypeId", "CertificateTypeName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CertificateRequestz certificateRequest)
        {
            if (ModelState.IsValid)
            {
                certificateRequest.SubmissionDate = DateTime.Now; // Ghi nhận ngày gửi yêu cầu
                _context.Add(certificateRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CitizenId"] = new SelectList(_context.Citizens, "CitizenId", "FullName", certificateRequest.CitizenId);
            ViewData["CertificateTypeId"] = new SelectList(_context.CertificateTypes, "CertificateTypeId", "CertificateTypeName", certificateRequest.CertificateTypeId);
            return View(certificateRequest);
        }

        // Hiển thị form chỉnh sửa yêu cầu chứng nhận
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var certificateRequest = await _context.CertificateRequests.FindAsync(id);
            if (certificateRequest == null)
            {
                return NotFound();
            }

            ViewData["CitizenId"] = new SelectList(_context.Citizens, "CitizenId", "FullName", certificateRequest.CitizenId);
            ViewData["CertificateTypeId"] = new SelectList(_context.CertificateTypes, "CertificateTypeId", "CertificateTypeName", certificateRequest.CertificateTypeId);
            return View(certificateRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CertificateRequestz certificateRequest)
        {
            if (id != certificateRequest.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(certificateRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CertificateRequestExists(certificateRequest.RequestId))
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

            ViewData["CitizenId"] = new SelectList(_context.Citizens, "CitizenId", "FullName", certificateRequest.CitizenId);
            ViewData["CertificateTypeId"] = new SelectList(_context.CertificateTypes, "CertificateTypeId", "CertificateTypeName", certificateRequest.CertificateTypeId);
            return View(certificateRequest);
        }

        // Hiển thị form xóa yêu cầu chứng nhận
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var certificateRequest = await _context.CertificateRequests
                .Include(cr => cr.Citizen)
                .Include(cr => cr.CertificateType)
                .FirstOrDefaultAsync(m => m.RequestId == id);

            if (certificateRequest == null)
            {
                return NotFound();
            }

            return View(certificateRequest);
        }

        // Xử lý xóa yêu cầu chứng nhận
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var certificateRequest = await _context.CertificateRequests.FindAsync(id);
            if (certificateRequest != null)
            {
                _context.CertificateRequests.Remove(certificateRequest);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CertificateRequestExists(int id)
        {
            return _context.CertificateRequests.Any(e => e.RequestId == id);
        }
    }
}
