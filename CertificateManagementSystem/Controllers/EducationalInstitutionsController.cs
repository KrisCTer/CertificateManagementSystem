using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationalInstitutionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly ApplicationDbContext _context;

        public EducationalInstitutionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EducationalInstitutions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EducationalInstitution>>> GetInstitutions()
        {
            return await _context.EducationalInstitutions.ToListAsync();
        }

        // GET: api/EducationalInstitutions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EducationalInstitution>> GetInstitution(string id)
        {
            var institution = await _context.EducationalInstitutions.FindAsync(id);

            if (institution == null)
            {
                return NotFound();
            }

            return institution;
        }

        // GET: api/EducationalInstitutions/5/Certificates
        [HttpGet("{id}/Certificates")]
        public async Task<ActionResult<IEnumerable<Certificate>>> GetInstitutionCertificates(string id)
        {
            if (!InstitutionExists(id))
            {
                return NotFound();
            }

            return await _context.Certificates
                .Where(c => c.IssuingInstitutionId == id)
                .Include(c => c.Citizen)
                .Include(c => c.CertificateType)
                .ToListAsync();
        }

        // POST: api/EducationalInstitutions
        [HttpPost]
        public async Task<ActionResult<EducationalInstitution>> CreateInstitution(EducationalInstitution institution)
        {
            institution.CreatedDate = DateTime.Now;
            _context.EducationalInstitutions.Add(institution);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (InstitutionExists(institution.InstitutionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetInstitution", new { id = institution.InstitutionId }, institution);
        }

        // PUT: api/EducationalInstitutions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInstitution(string id, EducationalInstitution institution)
        {
            if (id != institution.InstitutionId)
            {
                return BadRequest();
            }

            institution.UpdatedDate = DateTime.Now;
            _context.Entry(institution).State = EntityState.Modified;
            _context.Entry(institution).Property(x => x.CreatedDate).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstitutionExists(id))
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

        // DELETE: api/EducationalInstitutions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EducationalInstitution>> DeleteInstitution(string id)
        {
            var institution = await _context.EducationalInstitutions.FindAsync(id);
            if (institution == null)
            {
                return NotFound();
            }

            _context.EducationalInstitutions.Remove(institution);
            await _context.SaveChangesAsync();

            return institution;
        }

        private bool InstitutionExists(string id)
        {
            return _context.EducationalInstitutions.Any(e => e.InstitutionId == id);
        }
    }
}
