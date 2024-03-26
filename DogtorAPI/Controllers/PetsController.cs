using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DogtorAPI.Data;
using DogtorAPI.Model;
using DogtorAPI.ViewModel.Pet;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Linq;

namespace DogtorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly DogtorAPIContext _context;

        public PetsController(DogtorAPIContext context)
        {
            _context = context;
        }

        // GET: api/Pets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPet()
        {
          if (_context.Pet == null)
          {
              return NotFound();
          }
            return await _context.Pet.ToListAsync();
        }

        // GET: api/Pets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pet>> GetPet(Guid id)
        {
          if (_context.Pet == null)
          {
              return NotFound();
          }
            var pet = await _context.Pet.FindAsync(id);

            if (pet == null)
            {
                return NotFound();
            }

            return pet;
        }

        // PUT: api/Pets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPet(Guid id, Pet pet)
        {
            if (id != pet.Id)
            {
                return BadRequest();
            }

            _context.Entry(pet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(id))
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

        // POST: api/Pets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreatePetResponse>> PostPet(CreatePetRequest pet)
        {
            if (_context.Pet == null)
            {
                return Problem("Entity set 'DogtorAPIContext.Pet'  is null.");
            }
            var model = new Pet(pet.Name, pet.Race, pet.Color, pet.Description, pet.TutorID);

            _context.Pet.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPet", new { id = model.Id }, model);
        }

        // DELETE: api/Pets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(Guid id)
        {
            if (_context.Pet == null)
            {
                return NotFound();
            }
            var pet = await _context.Pet.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            _context.Pet.Remove(pet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PetExists(Guid id)
        {
            return (_context.Pet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
