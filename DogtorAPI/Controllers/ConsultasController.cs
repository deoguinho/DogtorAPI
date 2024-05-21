using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DogtorAPI.Data;
using DogtorAPI.Model;
using DogtorAPI.ViewModel.Consulta;

namespace DogtorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly DogtorAPIContext _context;

        public ConsultasController(DogtorAPIContext context)
        {
            _context = context;
        }

        // GET: api/Consultas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consulta>>> GetConsulta()
        {
            if (_context.Consulta == null)
            {
                return NotFound();
            }
            var consultas = await _context.Consulta
                              .Include(c => c.Tutor)
                              .Include(c => c.Veterinario)
                              .Include(c => c.Pet)
                              .Select(c => new ConsultaDTO
                              {
                                  ConsultaId = c.ConsultaId,
                                  Data = c.Data,
                                  Observacoes = c.Observacoes,
                                  Status = c.Status,
                                  TutorID = c.TutorId,
                                  TutorNome = c.Tutor.Name,
                                  VeterinarioID = c.Veterinario.Id,
                                  VeterinarioNome = c.Veterinario.Name,
                                  PetID = c.Pet.Id,
                                  PetNome = c.Pet.Name
                                  // Adicione outras propriedades conforme necessário
                              })
                              .ToListAsync();

            if (consultas == null || !consultas.Any())
            {
                return NotFound();
            }

            return Ok(consultas);
        }

        [HttpGet("GetCountConsultas")]
        public async Task<int> GetTotalConsultas()
        {
            return await _context.Consulta.CountAsync();
        }
        // GET: api/Consultas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Consulta>> GetConsulta(Guid id)
        {
          if (_context.Consulta == null)
          {
              return NotFound();
          }
            var consulta = await _context.Consulta.FindAsync(id);

            if (consulta == null)
            {
                return NotFound();
            }

            return consulta;
        }

        // PUT: api/Consultas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsulta(Guid id, Consulta consulta)
        {
            if (id != consulta.ConsultaId)
            {
                return BadRequest();
            }

            _context.Entry(consulta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsultaExists(id))
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

        // POST: api/Consultas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Consulta>> PostConsulta(CreateConsultaRequest consulta)
        {
          if (_context.Consulta == null)
          {
              return Problem("Entity set 'DogtorAPIContext.Consulta'  is null.");
          }
            _context.Consulta.Add(Consulta.CreateConsultaFromConsultaRequest(consulta));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConsulta", new { id = consulta.ConsultaId }, consulta);
        }

        // DELETE: api/Consultas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsulta(Guid id)
        {
            if (_context.Consulta == null)
            {
                return NotFound();
            }
            var consulta = await _context.Consulta.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            _context.Consulta.Remove(consulta);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("{id}/status")]
        public async Task<IActionResult> PutConsultaStatus(Guid id, PutStatusRequest updateStatusDto)
        {
            if (_context.Consulta == null)
            {
                return NotFound();
            }

            var validStatuses = new List<string> { "PENDENTE", "ACEITO", "NEGADO", "CONCLUIDO", "CANCELADO" };
            if (!validStatuses.Contains(updateStatusDto.Status.ToUpper()))
            {
                return StatusCode(500, new { Message = "Esta opção não é válida" });
            }

            // Verifica se o ID da consulta é válido
            var consulta = await _context.Consulta.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            // Atualiza apenas o campo de status
            consulta.Status = updateStatusDto.Status.ToUpper();

            // Marca o campo de status como modificado
            _context.Entry(consulta).Property(c => c.Status).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsultaExists(id))
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

        [HttpGet("api/consultas/relatorio")]
        public async Task<ActionResult<RelatorioConsultaResponse>> GetRelatorioConsultas()
        {
            // Busca todas as consultas do banco de dados
            var consultas = await _context.Consulta.ToListAsync();

            // Calcula as informações do relatório
            var totalConsultas = consultas.Count;
            //PENDENTE, ACEITO, NEGADO, CONCLUIDO, CANCELADO
            var TotalConsultasPendentes = consultas.Count(c => c.Status == "PENDENTE");
            var TotalConsultasAceitas = consultas.Count(c => c.Status == "ACEITO");
            var TotalConsultasNegadas = consultas.Count(c => c.Status == "NEGADO");
            var TotalConsultasConcluidas = consultas.Count(c => c.Status == "CONCLUIDO");
            var totalConsultasCanceladas = consultas.Count(c => c.Status == "CANCELADO");

            // Cria o objeto de relatório
            var relatorio = new RelatorioConsultaResponse
            {
                TotalConsultas = totalConsultas,
                TotalConsultasPendentes = TotalConsultasPendentes,
                TotalConsultasAceitas = TotalConsultasAceitas,
                TotalConsultasNegadas = TotalConsultasNegadas,
                TotalConsultasConcluidas = TotalConsultasConcluidas,
                TotalConsultasCanceladas = totalConsultasCanceladas
                // Preencha outros campos conforme necessário
            };

            // Retorna o relatório
            return Ok(relatorio);
        }

        private bool ConsultaExists(Guid id)
        {
            return (_context.Consulta?.Any(e => e.ConsultaId == id)).GetValueOrDefault();
        }
    }
}
