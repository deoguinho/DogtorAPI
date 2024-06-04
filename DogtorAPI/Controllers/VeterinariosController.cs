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
                    CPF = v.CPF,
                    Especialidades = v.Especialidade,
                    MediaAvaliacoes = v.Avaliacoes.Any() ? Math.Floor(v.Avaliacoes.Average(a => a.Nota)) : 0, // Calcula a média das avaliações
                    QuantidadeAvaliacoes = v.Avaliacoes.Count(), // Conta a quantidade de avaliações
                    Status = v.Status
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

        // PUT: api/Veterinarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeterinario(Guid id, PutRequestVeterinario veterinario)
        {
            if (_context.Veterinario == null)
            {
                return NotFound();
            }

            if (id != veterinario.Id)
            {
                return BadRequest("ID do caminho não coincide com o ID do corpo da requisição.");
            }

            var existingVeterinario = await _context.Veterinario.FindAsync(id);
            if (existingVeterinario == null)
            {
                return NotFound();
            }
            existingVeterinario.Name = veterinario.Name;
            _context.Entry(existingVeterinario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeterinarioPutExists(id))
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
