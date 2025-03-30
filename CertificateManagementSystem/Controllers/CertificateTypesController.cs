using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateTypesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly ApplicationDbContext _context;

        public CertificateTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CertificateTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CertificateType>>> GetCertificateTypes()
        {
            return await _context.CertificateTypes.ToListAsync();
        }

        // GET: api/CertificateTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CertificateType>> GetCertificateType(string id)
        {
            var certificateType = await _context.CertificateTypes.FindAsync(id);

            if (certificateType == null)
            {
                return NotFound();
            }

            return certificateType;
        }

        // POST: api/CertificateTypes
        [HttpPost]
        public async Task<ActionResult<CertificateType>> CreateCertificateType(CertificateType certificateType)
        {
            certificateType.CreatedDate = DateTime.Now;
            _context.CertificateTypes.Add(certificateType);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CertificateTypeExists(certificateType.CertificateTypeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCertificateType", new { id = certificateType.CertificateTypeId }, certificateType);
        }

        // PUT: api/CertificateTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCertificateType(string id, CertificateType certificateType)
        {
            if (id != certificateType.CertificateTypeId)
            {
                return BadRequest();
            }

            certificateType.UpdatedDate = DateTime.Now;
            _context.Entry(certificateType).State = EntityState.Modified;
            _context.Entry(certificateType).Property(x => x.CreatedDate).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificateTypeExists(id))
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

        // DELETE: api/CertificateTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CertificateType>> DeleteCertificateType(string id)
        {
            var certificateType = await _context.CertificateTypes.FindAsync(id);
            if (certificateType == null)
            {
                return NotFound();
            }

            _context.CertificateTypes.Remove(certificateType);
            await _context.SaveChangesAsync();

            return certificateType;
        }

        private bool CertificateTypeExists(string id)
        {
            return _context.CertificateTypes.Any(e => e.CertificateTypeId == id);
        }
    }
}
