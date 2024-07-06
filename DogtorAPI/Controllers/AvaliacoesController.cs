using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DogtorAPI.Data;
using DogtorAPI.Model;
using DogtorAPI.ViewModel.Avaliacoes;
using DogtorAPI.ViewModel.Veterinario;
using System.Drawing;

namespace DogtorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacoesController : ControllerBase
    {
        private readonly DogtorAPIContext _context;

        public AvaliacoesController(DogtorAPIContext context)
        {
            _context = context;
        }

        // GET: api/Avaliacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Avaliacoes>>> GetAvaliacoes()
        {
          if (_context.Avaliacoes == null)
          {
              return NotFound();
          }
            return await _context.Avaliacoes.ToListAsync();
        }

        // GET: api/Avaliacoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AvaliacaoByIDResponse>>> GetAvaliacoes(Guid id)
        {
            if (_context.Avaliacoes == null)
            {
                return BadRequest(); // Retorna 400 quando não há avaliações
            }

            // Verifique se existem avaliações para o Veterinário com o ID fornecido
            var avaliacoesQuery = _context.Avaliacoes
                                          .Where(a => a.VeterinarioID == id)
                                          .Include(a => a.Tutor)
                                          .Include(v => v.Veterinario)
                                          .Select(a => new AvaliacaoByIDResponse
                                          {
                                              Id = a.Id,
                                              Nota = a.Nota,
                                              Comentario = a.Comentario,
                                              TutorID = a.TutorID,
                                              TutorName = a.Tutor.Name,
                                              TutorPhoto = a.Tutor.Photo,
                                              VeterinarioID = a.VeterinarioID,
                                              VeterinarioName = a.Veterinario.Name,
                                              VeterinarioPhoto = a.Veterinario.Photo,
                                              Resposta = a.Resposta,
                                              CreatedAt = a.CreatedAt // Adicione CreatedAt à seleção
                                          });

            // Para garantir que a ordenação seja feita no banco de dados, adicione OrderBy antes do ToListAsync
            var avaliacoes = await avaliacoesQuery.OrderBy(a => a.CreatedAt).ToListAsync();

            if (!avaliacoes.Any())
            {
                return NoContent(); // Retorna 204 quando há avaliações mas a lista está vazia
            }

            return Ok(avaliacoes); // Retorna 200 com a lista de avaliações
        }

        [HttpGet("GetMedia/{id}")]
        public async Task<ActionResult<VeterinarioDTO>> GetVeterinario(Guid id)
        {
            var veterinario = await _context.Veterinario.FindAsync(id);

            if (veterinario == null)
            {
                return NotFound();
            }

            // Calcular a média das avaliações
            var mediaAvaliacoes = await CalcularMediaAvaliacoes(id);

            var veterinarioDTO = new VeterinarioDTO
            {
                Id = veterinario.Id,
                Name = veterinario.Name,
                // Outras propriedades do veterinário...
                MediaAvaliacoes = Math.Round(mediaAvaliacoes) // Adicionar a média das avaliações ao DTO
            };

            return veterinarioDTO;
        }

        // PUT: api/Avaliacoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAvaliacoes(Guid id, Avaliacoes avaliacoes)
        {
            if (id != avaliacoes.Id)
            {
                return BadRequest();
            }

            _context.Entry(avaliacoes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvaliacoesExists(id))
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

        // POST: api/Avaliacoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Avaliacoes>> PostAvaliacoes(CreateAvaliacoesRequest avaliacoes)
        {
            if (_context.Avaliacoes == null)
            {
                return Problem("Entity set 'DogtorAPIContext.Avaliacoes' is null.");
            }

            // Cria a nova avaliação a partir do request
            var newAvaliacao = Avaliacoes.CreateAvaliacoesFromConsultaRequest(avaliacoes);

            // Adiciona a nova avaliação ao contexto
            _context.Avaliacoes.Add(newAvaliacao);

            // Salva as alterações no banco de dados
            await _context.SaveChangesAsync();

            // Retorna a ação CreatedAtAction com o id da nova avaliação e a própria avaliação
            return CreatedAtAction("GetAvaliacoes", new { id = newAvaliacao.Id }, newAvaliacao);
        }

        // DELETE: api/Avaliacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvaliacoes(Guid id)
        {
            if (_context.Avaliacoes == null)
            {
                return NotFound();
            }
            var avaliacoes = await _context.Avaliacoes.FindAsync(id);
            if (avaliacoes == null)
            {
                return NotFound();
            }

            _context.Avaliacoes.Remove(avaliacoes);
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
        public class ResponderAvaliacaoRequest
        {
            public string Resposta { get; set; }
        }

        [HttpPost("{id}/responder")]
        public async Task<IActionResult> ResponderAvaliacao(Guid id, [FromBody] ResponderAvaliacaoRequest request)
        {
            var sucesso = await ResponderAvaliacaoAsync(id, request.Resposta);
            if (!sucesso)
            {
                return NotFound();
            }

            return NoContent();
        }

        [NonAction]
        public async Task<bool> ResponderAvaliacaoAsync(Guid avaliacaoId, string resposta)
        {
            var avaliacao = await _context.Avaliacoes.FindAsync(avaliacaoId);
            if (avaliacao == null)
            {
                return false; // Avaliação não encontrada
            }

            avaliacao.Resposta = resposta;
            avaliacao.DataResposta = DateTime.UtcNow;

            _context.Avaliacoes.Update(avaliacao);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool AvaliacoesExists(Guid id)
        {
            return (_context.Avaliacoes?.Any(e => e.Id == id)).GetValueOrDefault();
        }



    }
}
