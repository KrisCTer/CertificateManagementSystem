using CertificateManagementSystem.Models;
using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly ApplicationDbContext _context;

        public AttachmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Attachments/Request/5
        [HttpGet("Request/{requestId}")]
        public async Task<ActionResult<IEnumerable<Attachmentz>>> GetRequestAttachments(int requestId)
        {
            return await _context.Attachments
                .Where(a => a.RequestId == requestId)
                .ToListAsync();
        }

        // GET: api/Attachments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attachmentz>> GetAttachment(int id)
        {
            var attachment = await _context.Attachments.FindAsync(id);

            if (attachment == null)
            {
                return NotFound();
            }

            return attachment;
        }

        // POST: api/Attachments
        [HttpPost]
        public async Task<ActionResult<Attachmentz>> CreateAttachment(Attachmentz attachment)
        {
            attachment.UploadDate = DateTime.Now;
            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttachment", new { id = attachment.AttachmentId }, attachment);
        }

        // DELETE: api/Attachments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Attachmentz>> DeleteAttachment(int id)
        {
            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment == null)
            {
                return NotFound();
            }

            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();

            return attachment;
        }
    }
}
