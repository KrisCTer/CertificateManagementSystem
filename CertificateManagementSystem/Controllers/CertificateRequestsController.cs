
using CertificateManagementSystem.Models;
using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateRequestsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly ApplicationDbContext _context;

        public CertificateRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CertificateRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CertificateRequestz>>> GetCertificateRequests()
        {
            return await _context.CertificateRequests
                .Include(r => r.Citizen)
                .Include(r => r.CertificateType)
                .Include(r => r.Processor)
                .ToListAsync();
        }

        // GET: api/CertificateRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CertificateRequestz>> GetCertificateRequest(int id)
        {
            var request = await _context.CertificateRequests
                .Include(r => r.Citizen)
                .Include(r => r.CertificateType)
                .Include(r => r.Processor)
                .Include(r => r.Attachments)
                .FirstOrDefaultAsync(r => r.RequestId == id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // GET: api/CertificateRequests/Citizen/5
        [HttpGet("Citizen/{citizenId}")]
        public async Task<ActionResult<IEnumerable<CertificateRequestz>>> GetCitizenRequests(string citizenId)
        {
            return await _context.CertificateRequests
                .Where(r => r.CitizenId == citizenId)
                .Include(r => r.CertificateType)
                .Include(r => r.Processor)
                .ToListAsync();
        }

        // POST: api/CertificateRequests
        [HttpPost]
        public async Task<ActionResult<CertificateRequestz>> CreateCertificateRequest(CertificateRequestz request)
        {
            request.SubmissionDate = DateTime.Now;
            request.Status = "Pending"; // Default status for new requests

            _context.CertificateRequests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCertificateRequest", new { id = request.RequestId }, request);
        }

        // PUT: api/CertificateRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCertificateRequest(int id, CertificateRequestz request)
        {
            if (id != request.RequestId)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;
            _context.Entry(request).Property(x => x.SubmissionDate).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificateRequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CertificateRequests/5/Process
        [HttpPost("{id}/Process")]
        public async Task<IActionResult> ProcessRequest(int id, [FromBody] RequestProcessModel model)
        {
            var request = await _context.CertificateRequests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = model.NewStatus;
            request.ProcessedBy = model.ProcessedBy;
            request.Notes = model.Notes;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/CertificateRequests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CertificateRequestz>> DeleteCertificateRequest(int id)
        {
            var request = await _context.CertificateRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.CertificateRequests.Remove(request);
            await _context.SaveChangesAsync();

            return request;
        }

        private bool CertificateRequestExists(int id)
        {
            return _context.CertificateRequests.Any(e => e.RequestId == id);
        }
    }
}
