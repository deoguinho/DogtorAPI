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
using static DogtorAPI.ViewModel.Tutor.GetAllTutorResponse;

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
 

        public async Task<ActionResult<IEnumerable<TutorDto>>> GetTutor()
        {
            if (_context.Tutor == null)
            {
                return BadRequest(); // Retorna 400 quando _context.Tutor é nulo
            }

            var tutores = await _context.Tutor
                                        .Include(t => t.Pets)
                                        .Select(t => new TutorDto
                                        {
                                            Id = t.Id,
                                            Nome = t.Name,
                                            Pets = t.Pets.Select(p => new PetDto
                                            {
                                                Id = p.Id,
                                                Nome = p.Name
                                            }).ToList()
                                        })
                                        .ToListAsync();

            if (!tutores.Any())
            {
                return NoContent(); // Retorna 204 quando a lista de tutores está vazia
            }

            return Ok(tutores); // Retorna 200 com a lista de tutores
        }

        // GET: api/Tutors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tutor>> GetTutor(Guid id)
        {
          if (_context.Tutor == null)
          {
              return NotFound();
          }
            var tutor = await _context.Tutor.Include(Pet => Pet.Pets).FirstAsync(x => x.Id == id);

            if (tutor == null)
            {
                return NotFound();
            }

            return tutor;
        }

        // PUT: api/Tutors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTutor(Guid id, PutTutorRequest tutor)
        {
            if (id != tutor.Id)
            {
                return BadRequest();
            }

            var existingTutor = await _context.Users.FindAsync(id.ToString());

            if (existingTutor == null)
            {
                return NotFound();
            }

            existingTutor.UserName = tutor.Email;
            existingTutor.Email = tutor.Email;

            var result = await _userManager.UpdateAsync(existingTutor);

            if (!result.Succeeded)
            {
                return StatusCode(500, result.Errors);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TutorExistsPut(id))
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
        [NonAction]
        private bool TutorExistsPut(Guid id)
        {
            return _context.Users.Any(e => e.Id == id.ToString());
        }

        // POST: api/Tutors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tutor>> PostTutor(CreateTutorRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest(ModelState);

                var existingTutor = await _context.Tutor.FirstOrDefaultAsync(x => x.Email == request.Email);

                if (existingTutor != null)
                    return BadRequest("Já possui um tutor com este e-mail!");
     
                var userId = await Register(request.Email, request.Password);
            
                var tutor = new Tutor(userId, request.Name, request.Email, request.Birth, request.CPF, request.Phone, request.Cep, request.Street, request.Number, request.City, request.Complement, request.Neighborhood);

                await _context.Tutor.AddAsync(tutor);
                
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTutor", new { id = tutor.Id }, tutor);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
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
