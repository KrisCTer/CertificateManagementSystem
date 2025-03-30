using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Route("api/[controller]")]
        [ApiController]
        public class CitizensController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public CitizensController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: api/Citizens
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Citizen>>> GetCitizens()
            {
                return await _context.Citizens.ToListAsync();
            }

            // GET: api/Citizens/5
            [HttpGet("{id}")]
            public async Task<ActionResult<Citizen>> GetCitizen(string id)
            {
                var citizen = await _context.Citizens.FindAsync(id);

                if (citizen == null)
                {
                    return NotFound();
                }

                return citizen;
            }

            // GET: api/Citizens/5/Certificates
            [HttpGet("{id}/Certificates")]
            public async Task<ActionResult<IEnumerable<Certificate>>> GetCitizenCertificates(string id)
            {
                if (!CitizenExists(id))
                {
                    return NotFound();
                }

                return await _context.Certificates
                    .Where(c => c.CitizenId == id)
                    .Include(c => c.CertificateType)
                    .Include(c => c.IssuingInstitution)
                    .ToListAsync();
            }

            // POST: api/Citizens
            [HttpPost]
            public async Task<ActionResult<Citizen>> CreateCitizen(Citizen citizen)
            {
                citizen.CreatedDate = DateTime.Now;
                _context.Citizens.Add(citizen);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (CitizenExists(citizen.CitizenId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtAction("GetCitizen", new { id = citizen.CitizenId }, citizen);
            }

            // PUT: api/Citizens/5
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateCitizen(string id, Citizen citizen)
            {
                if (id != citizen.CitizenId)
                {
                    return BadRequest();
                }

                citizen.UpdatedDate = DateTime.Now;
                _context.Entry(citizen).State = EntityState.Modified;
                _context.Entry(citizen).Property(x => x.CreatedDate).IsModified = false;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitizenExists(id))
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

            // DELETE: api/Citizens/5
            [HttpDelete("{id}")]
            public async Task<ActionResult<Citizen>> DeleteCitizen(string id)
            {
                var citizen = await _context.Citizens.FindAsync(id);
                if (citizen == null)
                {
                    return NotFound();
                }

                _context.Citizens.Remove(citizen);
                await _context.SaveChangesAsync();

                return citizen;
            }

            private bool CitizenExists(string id)
            {
                return _context.Citizens.Any(e => e.CitizenId == id);
            }
        }
    }
}
