using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DogtorAPI.Data;
using DogtorAPI.Model;
using DogtorAPI.ViewModel.Veterinario;

namespace DogtorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeterinarioFotosController : ControllerBase
    {
        private readonly DogtorAPIContext _context;

        public VeterinarioFotosController(DogtorAPIContext context)
        {
            _context = context;
        }

        // GET: api/VeterinarioFotos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VeterinarioFotos>>> GetVeterinarioFotos()
        {
          if (_context.VeterinarioFotos == null)
          {
              return NotFound();
          }
            return await _context.VeterinarioFotos.ToListAsync();
        }

        // GET: api/VeterinarioFotos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VeterinarioFotos>> GetVeterinarioFotos(Guid id)
        {
          if (_context.VeterinarioFotos == null)
          {
              return NotFound();
          }
            var veterinarioFotos = await _context.VeterinarioFotos.FindAsync(id);

            if (veterinarioFotos == null)
            {
                return NotFound();
            }

            return veterinarioFotos;
        }

        // PUT: api/VeterinarioFotos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeterinarioFotos(Guid id, VeterinarioFotos veterinarioFotos)
        {
            if (id != veterinarioFotos.Id)
            {
                return BadRequest();
            }

            _context.Entry(veterinarioFotos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeterinarioFotosExists(id))
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

        // POST: api/VeterinarioFotos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VeterinarioFotos>> PostVeterinarioFotos(VeterinarioFotosRequest request)
        {
          if (_context.VeterinarioFotos == null)
          {
              return Problem("Entity set 'DogtorAPIContext.VeterinarioFotos'  is null.");
          }

            var veterinarioFotos = request.Link!.Select(foto =>
             new VeterinarioFotos(foto, request.VeterinarioID)
         );

            _context.VeterinarioFotos!.AddRange(veterinarioFotos);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVeterinarioFotos", new { id = Guid.NewGuid() }, veterinarioFotos);
        }

        // DELETE: api/VeterinarioFotos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeterinarioFotos(Guid id)
        {
            if (_context.VeterinarioFotos == null)
            {
                return NotFound();
            }
            var veterinarioFotos = await _context.VeterinarioFotos.FindAsync(id);
            if (veterinarioFotos == null)
            {
                return NotFound();
            }

            _context.VeterinarioFotos.Remove(veterinarioFotos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VeterinarioFotosExists(Guid id)
        {
            return (_context.VeterinarioFotos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
