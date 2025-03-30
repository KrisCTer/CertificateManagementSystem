using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly ApplicationDbContext _context;

        public CertificatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Certificates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Certificate>>> GetCertificates()
        {
            return await _context.Certificates
                .Include(c => c.Citizen)
                .Include(c => c.CertificateType)
                .Include(c => c.IssuingInstitution)
                .ToListAsync();
        }

        // GET: api/Certificates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Certificate>> GetCertificate(string id)
        {
            var certificate = await _context.Certificates
                .Include(c => c.Citizen)
                .Include(c => c.CertificateType)
                .Include(c => c.IssuingInstitution)
                .FirstOrDefaultAsync(c => c.CertificateId == id);

            if (certificate == null)
            {
                return NotFound();
            }

            return certificate;
        }

        // GET: api/Certificates/5/History
        [HttpGet("{id}/History")]
        public async Task<ActionResult<IEnumerable<CertificateHistory>>> GetCertificateHistory(string id)
        {
            if (!CertificateExists(id))
            {
                return NotFound();
            }

            return await _context.CertificateHistories
                .Where(h => h.CertificateId == id)
                .OrderByDescending(h => h.ChangeDate)
                .ToListAsync();
        }

        // GET: api/Certificates/5/Verifications
        [HttpGet("{id}/Verifications")]
        public async Task<ActionResult<IEnumerable<CertificateVerification>>> GetCertificateVerifications(string id)
        {
            if (!CertificateExists(id))
            {
                return NotFound();
            }

            return await _context.CertificateVerifications
                .Where(v => v.CertificateId == id)
                .OrderByDescending(v => v.VerificationDate)
                .ToListAsync();
        }

        // POST: api/Certificates
        [HttpPost]
        public async Task<ActionResult<Certificate>> CreateCertificate(Certificate certificate)
        {
            certificate.CreatedDate = DateTime.Now;
            _context.Certificates.Add(certificate);

            try
            {
                await _context.SaveChangesAsync();

                // Create initial history record
                var history = new CertificateHistory
                {
                    CertificateId = certificate.CertificateId,
                    NewStatus = certificate.Status,
                    ChangeDate = DateTime.Now,
                    Reason = "Certificate created",
                    ChangedBy = "System" // This would typically be the logged-in user
                };

                _context.CertificateHistories.Add(history);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CertificateExists(certificate.CertificateId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCertificate", new { id = certificate.CertificateId }, certificate);
        }

        // PUT: api/Certificates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCertificate(string id, Certificate certificate)
        {
            if (id != certificate.CertificateId)
            {
                return BadRequest();
            }

            // Get the original certificate to track status changes
            var originalCertificate = await _context.Certificates.AsNoTracking()
                .FirstOrDefaultAsync(c => c.CertificateId == id);

            if (originalCertificate == null)
            {
                return NotFound();
            }

            certificate.UpdatedDate = DateTime.Now;
            _context.Entry(certificate).State = EntityState.Modified;
            _context.Entry(certificate).Property(x => x.CreatedDate).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();

                // Add history record if status changed
                if (originalCertificate.Status != certificate.Status)
                {
                    var history = new CertificateHistory
                    {
                        CertificateId = certificate.CertificateId,
                        PreviousStatus = originalCertificate.Status,
                        NewStatus = certificate.Status,
                        ChangeDate = DateTime.Now,
                        Reason = "Status updated",
                        ChangedBy = "System" // This would typically be the logged-in user
                    };

                    _context.CertificateHistories.Add(history);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificateExists(id))
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

        // DELETE: api/Certificates/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Certificate>> DeleteCertificate(string id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate == null)
            {
                return NotFound();
            }

            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();

            return certificate;
        }

        // POST: api/Certificates/5/Verify
        [HttpPost("{id}/Verify")]
        public async Task<ActionResult<CertificateVerification>> VerifyCertificate(string id, CertificateVerification verification)
        {
            if (id != verification.CertificateId)
            {
                return BadRequest();
            }

            if (!CertificateExists(id))
            {
                return NotFound();
            }

            verification.VerificationDate = DateTime.Now;
            _context.CertificateVerifications.Add(verification);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCertificateVerifications", new { id = verification.CertificateId }, verification);
        }

        private bool CertificateExists(string id)
        {
            return _context.Certificates.Any(e => e.CertificateId == id);
        }
    }
}
