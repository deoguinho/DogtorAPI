using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DogtorAPI.Data;
using DogtorAPI.Model;
using Microsoft.AspNetCore.Identity;
using DogtorAPI.ViewModel.Tutor;

namespace DogtorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorsController : ControllerBase
    {
        private readonly DogtorAPIContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TutorsController(DogtorAPIContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Tutors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetTutor()
        {
            if (_context.Tutor == null)
            {
                return NotFound();
            }

            return Ok(await _context.Tutor.Include(pets => pets.Pets).ToListAsync());
        }

        // GET: api/Tutors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tutor>> GetTutor(Guid id)
        {
          if (_context.Tutor == null)
          {
              return NotFound();
          }
            var tutor = await _context.Tutor.FindAsync(id);
    
            if (tutor == null)
            {
                return NotFound();
            }

            object getTutorObject = new { tutor };

            return Ok(getTutorObject);
            
        }

        // PUT: api/Tutors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTutor(Guid id, Tutor tutor)
        {
            if (id != tutor.Id)
            {
                return BadRequest();
            }

            _context.Entry(tutor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TutorExists(id))
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

        // POST: api/Tutors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tutor>> PostTutor(CreateTutorRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingTutor = await _context.Tutor.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (existingTutor != null)
                return BadRequest("Já possui um tutor com este e-mail!");

            var userId = await Register(request.Email, request.Password);
            var type = "Tutor";

            var tutor = new Tutor(userId, type, request.Name, request.Email, request.Birth, request.CPF, request.Phone, request.Cep, request.Street, request.Number, request.City, request.Complement, request.Neighborhood);

            await _context.Tutor.AddAsync(tutor);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTutor", new { id = tutor.Id }, tutor);
        }

        // DELETE: api/Tutors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTutor(Guid id)
        {
            if (_context.Tutor == null)
            {
                return NotFound();
            }
            var tutor = await _context.Tutor.FindAsync(id);
            if (tutor == null)
            {
                return NotFound();
            }

            _context.Tutor.Remove(tutor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TutorExists(Guid id)
        {
            return (_context.Tutor?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<Guid> Register(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
                return Guid.Parse(user.Id);

            var identityUser = new IdentityUser() { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(identityUser, password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Erro ao cadastrar usuário!");
            }

            var newUser = await _userManager.FindByEmailAsync(email);

            return Guid.Parse(newUser.Id);
        }
    }
}
