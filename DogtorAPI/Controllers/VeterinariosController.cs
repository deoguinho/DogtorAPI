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
        public async Task<ActionResult<IEnumerable<Veterinario>>> GetVeterinario()
        {
            if (_context.Veterinario == null)
            {
                return NotFound();
            }

            return Ok(await _context.Veterinario.Include(Especialidade => Especialidade.Especialidade).ToListAsync());

        }

        // GET: api/Veterinarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Veterinario>> GetVeterinario(Guid id)
        {
            if (_context.Veterinario == null)
            {
                return NotFound();
            }
            var veterinario = await _context.Veterinario.Include(Especialidade => Especialidade.Especialidade)
                .FirstAsync(x => x.Id == id);



            if (veterinario == null)
            {
                return NotFound();
            }

            object getVeterinarioObject = new { veterinario };

            return Ok(getVeterinarioObject);
        }

        // PUT: api/Veterinarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeterinario(Guid id, Veterinario veterinario)
        {
            if (id != veterinario.Id)
            {
                return BadRequest();
            }

            _context.Entry(veterinario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeterinarioExists(id))
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

            await _context.Veterinario.AddAsync(veterinario);

            _context.Especialidade!.AddRange(especialidades); 

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
