using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CertificateManagementSystem.Models;
using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    public class AttachmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttachmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách tài liệu đính kèm của yêu cầu chứng nhận
        public async Task<IActionResult> Index(int requestId)
        {
            var certificateRequest = await _context.CertificateRequests
                .Include(cr => cr.Attachments)
                .FirstOrDefaultAsync(cr => cr.RequestId == requestId);

            if (certificateRequest == null)
            {
                return NotFound();
            }

            return View(certificateRequest.Attachments);
        }

        // Hiển thị form tạo tài liệu đính kèm mới
        public IActionResult Create(int requestId)
        {
            ViewData["RequestId"] = requestId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int requestId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn một tệp để tải lên.");
                return View();
            }

            var certificateRequest = await _context.CertificateRequests.FindAsync(requestId);
            if (certificateRequest == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new Attachmentz
            {
                RequestId = requestId,
                DocumentName = file.FileName,
                DocumentType = file.ContentType,
                FilePath = filePath,
                UploadDate = DateTime.Now,
                UploadedBy = User.Identity.Name // Nếu có hệ thống xác thực người dùng
            };

            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { requestId = requestId });
        }

        // Hiển thị form chỉnh sửa tài liệu đính kèm
        public async Task<IActionResult> Edit(int id)
        {
            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Attachmentz attachment)
        {
            if (id != attachment.AttachmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attachment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttachmentExists(attachment.AttachmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { requestId = attachment.RequestId });
            }

            return View(attachment);
        }

        // Hiển thị form xác nhận xóa tài liệu đính kèm
        public async Task<IActionResult> Delete(int id)
        {
            var attachment = await _context.Attachments
                .Include(a => a.CertificateRequest)
                .FirstOrDefaultAsync(m => m.AttachmentId == id);

            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }

        // Xóa tài liệu đính kèm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment != null)
            {
                _context.Attachments.Remove(attachment);
                await _context.SaveChangesAsync();
            }

            // Xóa tệp thực tế nếu có
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", attachment.DocumentName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return RedirectToAction(nameof(Index), new { requestId = attachment.RequestId });
        }

        private bool AttachmentExists(int id)
        {
            return _context.Attachments.Any(e => e.AttachmentId == id);
        }
    }
}
