using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DogtorAPI.Data;
using DogtorAPI.Model;

namespace DogtorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        private readonly DogtorAPIContext _context;

        public EspecialidadesController(DogtorAPIContext context)
        {
            _context = context;
        }

        // GET: api/Especialidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Especialidade>>> GetEspecialidade()
        {
          if (_context.Especialidade == null)
          {
              return NotFound();
          }
            return await _context.Especialidade.ToListAsync();
        }

        // GET: api/Especialidades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Especialidade>> GetEspecialidade(Guid id)
        {
          if (_context.Especialidade == null)
          {
              return NotFound();
          }
            var especialidade = await _context.Especialidade.FindAsync(id);

            if (especialidade == null)
            {
                return NotFound();
            }

            return especialidade;
        }

        // PUT: api/Especialidades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEspecialidade(Guid id, Especialidade especialidade)
        {
            if (id != especialidade.Id)
            {
                return BadRequest();
            }

            _context.Entry(especialidade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EspecialidadeExists(id))
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

        // POST: api/Especialidades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Especialidade>> PostEspecialidade(Especialidade especialidade)
        {
          if (_context.Especialidade == null)
          {
              return Problem("Entity set 'DogtorAPIContext.Especialidade'  is null.");
          }
            _context.Especialidade.Add(especialidade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEspecialidade", new { id = especialidade.Id }, especialidade);
        }

        // DELETE: api/Especialidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEspecialidade(Guid id)
        {
            if (_context.Especialidade == null)
            {
                return NotFound();
            }
            var especialidade = await _context.Especialidade.FindAsync(id);
            if (especialidade == null)
            {
                return NotFound();
            }

            _context.Especialidade.Remove(especialidade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EspecialidadeExists(Guid id)
        {
            return (_context.Especialidade?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        
    }
}
