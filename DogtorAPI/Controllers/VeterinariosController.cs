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
using DogtorAPI.ViewModel.Veterinario;
using DogtorAPI.ViewModel.Avaliacoes;
using System.Runtime.ConstrainedExecution;
using System.IO;
using DogtorAPI.ViewModel;

namespace DogtorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeterinariosController : ControllerBase
    {
        private readonly DogtorAPIContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public VeterinariosController(DogtorAPIContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Veterinarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllVeterinariosResponse>>> GetVeterinario()
        {
            if (_context.Veterinario == null)
            {
                return NotFound();
            }

            var veterinarios = await _context.Veterinario
                .Include(v => v.Especialidade)
                .Include(v => v.Avaliacoes)
                .Select(v => new GetAllVeterinariosResponse
                {
                    Id = v.Id,
                    Name = v.Name,
                    Email = v.Email,
                    Birth = v.Birth,
                    Phone = v.Phone,
                    Cep = v.Cep,
                    Street = v.Street,
                    Number = v.Number,
                    City = v.City,
                    Complement = v.Complement,
                    Neighborhood = v.Neighborhood,
                    UF = v.UF,
                    CRMV = v.CRMV,
                    Foto_CRMV = v.Foto_CRMV,
                    Photo = v.Photo,
                    BackgroundPhoto = v.BackgroundPhoto,
                    CPF = v.CPF,
                    Especialidades = v.Especialidade,
                    MediaAvaliacoes = v.Avaliacoes.Any() ? Math.Floor(v.Avaliacoes.Average(a => a.Nota)) : 0, // Calcula a média das avaliações
                    QuantidadeAvaliacoes = v.Avaliacoes.Count(), // Conta a quantidade de avaliações
                    Status = Convert.ToInt32(v.Status)
                }).ToListAsync();

            return Ok(veterinarios);    
        }


        // GET: api/Veterinarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VeterinarioResponse>> GetVeterinario(Guid id)
        {
            if (_context.Veterinario == null)
            {
                return NotFound();
            }

            var mediaAvaliacoes = await CalcularMediaAvaliacoes(id);

            var veterinario = await _context.Veterinario
                                         .Where(v => v.Id == id)
                                         .Include(v => v.Especialidade)
                                         .Include(v => v.VeterinarioFotos)
                                         .Include(v => v.Avaliacoes)
                                         .Select(v => new VeterinarioResponse
                                         {
                                             Id = v.Id,
                                             Name = v.Name,
                                             Email = v.Email,
                                             Birth = v.Birth,
                                             Phone = v.Phone,
                                             Cep = v.Cep,
                                             Street = v.Street,
                                             Number = v.Number,
                                             City = v.City,
                                             Complement = v.Complement,
                                             Neighborhood = v.Neighborhood,
                                             UF = v.UF,
                                             CRMV = v.CRMV,
                                             CPF = v.CPF,
                                             Photo = v.Photo,
                                             BackgroundPhoto = v.BackgroundPhoto,
                                             MediaAvaliacoes = Math.Floor(mediaAvaliacoes),
                                             Especialidades = v.Especialidade,
                                             VeterinarioFotos = v.VeterinarioFotos
                                         }).FirstOrDefaultAsync();



            if (veterinario == null)
            {
                return NotFound();
            }

            return veterinario;
        }


        [HttpGet("relatorios/{veterinarioId}")]
        public async Task<ActionResult> GetStatusCount(Guid veterinarioId)
        {
            var aceitasCount = await _context.Consulta
              .Where(c => c.VeterinarioId == veterinarioId && c.Status == "ACEITO")
              .CountAsync();
            var concluidoCount = await _context.Consulta
                .Where(c => c.VeterinarioId == veterinarioId && c.Status == "CONCLUIDO")
                .CountAsync();

            var pendenteCount = await _context.Consulta
                 .Where(c => c.VeterinarioId == veterinarioId && c.Status == "PENDENTE")
                 .CountAsync();

            var mediaAvaliacoes = await CalcularMediaAvaliacoes(veterinarioId);
            var quantidadeAvaliacoes = await _context.Avaliacoes.Where(a => a.VeterinarioID == veterinarioId).CountAsync();
            return Ok(new
            {
                Aceito = aceitasCount,
                Concluido = concluidoCount,
                Pendente = pendenteCount,
                MediaAvaliacoes = Math.Floor(mediaAvaliacoes),
                QuantidadeAvaliacoes = quantidadeAvaliacoes
            });
        }

        // PUT: api/Veterinarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeterinario(Guid id, [FromBody] PutRequestVeterinario veterinarioDto)
        {
            var veterinario = _context.Veterinario.FirstOrDefault(v => v.Id == id);
            if (veterinario == null)
            {
                return NotFound();
            }

            if (veterinarioDto.Name != null)
                veterinario.Name = veterinarioDto.Name;
            if (veterinarioDto.Email != null)
                veterinario.Email = veterinarioDto.Email;
            if (veterinarioDto.Birth != null)
                veterinario.Birth = veterinarioDto.Birth;
            if (veterinarioDto.Phone != null)
                veterinario.Phone = veterinarioDto.Phone;
            if (veterinarioDto.Cep != null)
                veterinario.Cep = veterinarioDto.Cep;
            if (veterinarioDto.Street != null)
                veterinario.Street = veterinarioDto.Street;
            if (veterinarioDto.Number != 0)
                veterinario.Number = veterinarioDto.Number;
            if (veterinarioDto.City != null)
                veterinario.City = veterinarioDto.City;
            if (veterinarioDto.Complement != null)
                veterinario.Complement = veterinarioDto.Complement;
            if (veterinarioDto.Neighborhood != null)
                veterinario.Neighborhood = veterinarioDto.Neighborhood;
            try
            {
                await _context.SaveChangesAsync(); // Salva as mudanças no banco de dados
            }
            catch (DbUpdateConcurrencyException)
            {
                // Trate aqui exceções de concorrência, se necessário
                throw;
            }
            return NoContent();
        }
        [NonAction]
        private bool VeterinarioPutExists(Guid id)
        {
            if (_context.Veterinario == null)
            {
                return false;
            }
            return _context.Veterinario.Any(e => e.Id == id);
        }

        // POST: api/Veterinarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Veterinario>> PostVeterinario(CreateVeterinarioRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingVeterinario = await _context.Veterinario.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (existingVeterinario != null)
                return BadRequest("Já possui um veterinário com este e-mail!");

            var userId = await Register(request.Email, request.Password);

            var veterinario = new Veterinario(userId, request.Name, request.Email, request.Birth, request.Phone, request.Cep, request.Street, request.Number, request.City, 
                request.Complement, request.Neighborhood, request.UF, request.CRMV, request.Foto_CRMV, request.CPF);

            var especialidades = request.Especialidade!.Select(especialidade =>
                new Especialidade(especialidade, userId));

            var veterinarioFotos = request.Link!.Select(foto =>
                new VeterinarioFotos(foto, userId)
            );
           

            await _context.Veterinario.AddAsync(veterinario);

            _context.Especialidade!.AddRange(especialidades);
            _context.VeterinarioFotos!.AddRange(veterinarioFotos);


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVeterinario", new { id = veterinario.Id }, veterinario);
        }

        // DELETE: api/Veterinarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeterinario(Guid id)
        {
            if (_context.Veterinario == null)
            {
                return NotFound();
            }
            var veterinario = await _context.Veterinario.FindAsync(id);
            if (veterinario == null)
            {
                return NotFound();
            }

            _context.Veterinario.Remove(veterinario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [NonAction]
        public async Task<double> CalcularMediaAvaliacoes(Guid veterinarioId)
        {
            if (_context.Avaliacoes == null)
            {
                return 0;
            }
            var notas = await _context.Avaliacoes
                                      .Where(a => a.VeterinarioID == veterinarioId)
                                      .Select(a => a.Nota)
                                      .ToListAsync();

            if (notas.Count == 0)
            {
                return 0; // Retorna 0 se não houver avaliações
            }

            return notas.Average();
        }

        [HttpPut("{id}/update-image-links")]
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
        public async Task<bool> UpdateImageLinksAsync(Guid veterinarioId, string photoLink, string backgroundPhotoLink)
        {
            var veterinario = await _context.Veterinario.FindAsync(veterinarioId);
            if (veterinario == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(photoLink))
            {
                veterinario.Photo = photoLink;
            }

            if (!string.IsNullOrEmpty(backgroundPhotoLink))
            {
                veterinario.BackgroundPhoto = backgroundPhotoLink;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        private bool VeterinarioExists(Guid id)
        {
            return (_context.Veterinario?.Any(e => e.Id == id)).GetValueOrDefault();
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
