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
using DogtorAPI.ViewModel;

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
                                            Name = t.Name,
                                            Email = t.Email,
                                            Birth = t.Birth,
                                            CPF = t.CPF,
                                            Phone = t.Phone,
                                            Cep = t.Cep,
                                            Street = t.Street,
                                            Number = t.Number,  
                                            City = t.City,
                                            Complement = t.Complement,
                                            Neighborhood = t.Neighborhood,
                                            Photo = t.Photo,
                                            BackgroundPhoto = t.BackgroundPhoto,
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
        public async Task<IActionResult> PutTutor(Guid id, PutTutorRequest tutorRequest)
        {
            if (id != tutorRequest.Id)
            {
                return BadRequest();
            }

            try
            {
                // Procura o Tutor no contexto do Entity Framework Core
                var existingTutor = await _context.Tutor.FindAsync(id);

                if (existingTutor == null)
                {
                    return NotFound();
                }

                // Atualiza propriedades do Tutor com base nos dados recebidos na requisição
                existingTutor.Name = tutorRequest.Name;
                existingTutor.Email = tutorRequest.Email;
                existingTutor.Phone = tutorRequest.Phone;
                // Atualiza outras propriedades conforme necessário

                // Atualiza também o usuário relacionado na tabela Users, se necessário
                var existingUser = await _context.Users.FindAsync(existingTutor.Id);

                if (existingUser == null)
                {
                    return NotFound("Usuário associado ao tutor não encontrado.");
                }

                existingUser.UserName = tutorRequest.Email;
                existingUser.Email = tutorRequest.Email;

                // Salva as mudanças no contexto do Entity Framework Core
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno ao tentar atualizar o tutor.");
            }
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

        [HttpPut("{id}/images")]
        public async Task<IActionResult> UpdateImageLinks(Guid id, [FromBody] UpdateImageLinksRequest request)
        {
            var result = await UpdateImageLinksAsync(id, request.PhotoLink, request.BackgroundPhotoLink);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [NonAction]
        public async Task<bool> UpdateImageLinksAsync(Guid tutorId, string photoLink, string backgroundPhotoLink)
        {

            var tutor = await _context.Tutor.FindAsync(tutorId);
            if (tutor == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(photoLink))
            {
                tutor.Photo = photoLink;
            }

            if (!string.IsNullOrEmpty(backgroundPhotoLink))
            {
                tutor.BackgroundPhoto = backgroundPhotoLink;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
