using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DogtorAPI.Data;
using DogtorAPI.Model;
using DogtorAPI.ViewModel.Admin;

namespace DogtorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly DogtorAPIContext _context;

        public AdminsController(DogtorAPIContext context)
        {
            _context = context;
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmin()
        {
          if (_context.Admin == null)
          {
              return NotFound();
          }
            return await _context.Admin.ToListAsync();
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(Guid id)
        {
          if (_context.Admin == null)
          {
              return NotFound();
          }
            var admin = await _context.Admin.FindAsync(id);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        // PUT: api/Admins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmin(Guid id, Admin admin)
        {
            if (id != admin.ID)
            {
                return BadRequest();
            }

            _context.Entry(admin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
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

        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin(AdminRequest request)
        {
          if (_context.Admin == null)
          {
              return Problem("Entity set 'DogtorAPIContext.Admin'  is null.");
          }

            var admin = new Admin(request.Name, request.Email, request.Password);
            _context.Admin.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdmin", new { id = new Guid() }, admin);
        }

        [HttpPost("{id}/aceitar")]
        public async Task<IActionResult> AceitarVeterinario(Guid id)
        {
            var result = await AceitarVeterinarioAsync(id);

            if (!result)
            {
                return NotFound(new { message = "Veterinário não encontrado" });
            }

            return Ok(new { message = "Veterinário aceito com sucesso" });
        }
        [HttpPost("{id}/negar")]
        public async Task<IActionResult> NegarVeterinario(Guid id)
        {
            var result = await NegarVeterinarioAsync(id);

            if (!result)
            {
                return NotFound(new { message = "Veterinário não encontrado" });
            }

            return Ok(new { message = "Veterinário aceito com sucesso" });
        }

        [NonAction]
        public async Task<bool> AceitarVeterinarioAsync(Guid veterinarioId)
        {
            // Buscar o veterinário pelo ID
            var veterinario = await _context.Veterinario.FindAsync(veterinarioId);

            if (veterinario == null)
            {
                // Retorna falso se o veterinário não for encontrado
                return false;
            }

            // Alterar o status para true
            veterinario.Status = true;

            // Salvar as mudanças no banco de dados
            await _context.SaveChangesAsync();

            return true;
        }

        [NonAction]
        public async Task<bool> NegarVeterinarioAsync(Guid veterinarioId)
        {
            // Buscar o veterinário pelo ID
            var veterinario = await _context.Veterinario.FindAsync(veterinarioId);

            if (veterinario == null)
            {
                // Retorna falso se o veterinário não for encontrado
                return false;
            }

            // Alterar o status para true
            veterinario.Status = true;

            // Salvar as mudanças no banco de dados
            await _context.SaveChangesAsync();

            return true;
        }
        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(Guid id)
        {
            if (_context.Admin == null)
            {
                return NotFound();
            }
            var admin = await _context.Admin.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _context.Admin.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("estatisticas")]
        public async Task<IActionResult> ObterEstatisticas()
        {
            var estatisticas = await ObterEstatisticasAsync();
            return Ok(estatisticas);
        }
        [NonAction]
        public async Task<Estatisticas> ObterEstatisticasAsync()
        {
            var numeroDeTutores = await _context.Tutor.CountAsync();
            var numeroDeVeterinarios = await _context.Veterinario.CountAsync();
            var numeroDeUsuariosTotais = numeroDeTutores + numeroDeVeterinarios;
            var numeroDePets = await _context.Pet.CountAsync();
            var numeroDeConsultas = await _context.Consulta.CountAsync();

            return new Estatisticas
            {
                NumeroDeUsuariosTotais = numeroDeUsuariosTotais,
                NumeroDeTutores = numeroDeTutores,
                NumeroDeVeterinarios = numeroDeVeterinarios,
                NumeroDePets = numeroDePets,
                NumeroDeConsultas = numeroDeConsultas
            };
        }
        [HttpGet("ObterDistribuicaoDeEspeciesAsync")]

        public async Task<List<EspecieRelatorio>> ObterDistribuicaoDeEspeciesAsync()
        {
            return await _context.Pet
                .GroupBy(p => p.Race)
                .Select(g => new EspecieRelatorio
                {
                    Especie = g.Key,
                    NumeroDePets = g.Count()
                })
                .ToListAsync();
        }
        private bool AdminExists(Guid id)
        {
            return (_context.Admin?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
