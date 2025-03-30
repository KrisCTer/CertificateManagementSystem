using CitizenshipCertificateandDiplomaManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CertificateManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .Select(u => new User
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Role = u.Role,
                    Status = u.Status,
                    Organization = u.Organization,
                    CreatedDate = u.CreatedDate,
                    UpdatedDate = u.UpdatedDate
                    // Password is explicitly excluded
                })
                .ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users
                .Select(u => new User
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Role = u.Role,
                    Status = u.Status,
                    Organization = u.Organization,
                    CreatedDate = u.CreatedDate,
                    UpdatedDate = u.UpdatedDate
                    // Password is explicitly excluded
                })
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            // In a real application, password should be hashed before saving
            user.CreatedDate = DateTime.Now;
            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            // Don't return password in response
            user.Password = null;
            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            user.UpdatedDate = DateTime.Now;

            // Only update password if it's provided
            if (string.IsNullOrEmpty(user.Password))
            {
                // Get the current password
                var currentUser = await _context.Users.AsNoTracking()
                    .Select(u => new { u.UserId, u.Password })
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (currentUser == null)
                {
                    return NotFound();
                }

                user.Password = currentUser.Password;
            }
            else
            {
                // In a real application, password should be hashed before saving
            }

            _context.Entry(user).State = EntityState.Modified;
            _context.Entry(user).Property(x => x.CreatedDate).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            // Don't return password in response
            user.Password = null;
            return user;
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
