using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using ProEventos.Application.DTOs;

namespace MyFirstWebAPPWithAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        public ILoteService _loteService { get; }
        public LotesController(ILoteService loteService)
        {
            _loteService = loteService;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)//posso colocar qualquer nome no metodo, o que manda nesse caso é o atributo do controller [HttpGet]
        {
            try
            {
                var lotes = await _loteService.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return NoContent();
                
                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar recuperar lotes. Erro: {ex.Message}");
            }
        }
        
        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDTO[] models)
        {
            try
            {
                var lotes = await _loteService.SaveLotes(eventoId, models);
                if (lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar salvar lotes. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{eventoId/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteService.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null) return NoContent();

                return await _loteService.DeleteLote(lote.EventoId, lote.Id) 
                    ? Ok(new { message ="Lote deletado"})
                    :throw new Exception("Ocorreu um problema não especifico ao tentar deletar o lote");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar deletar os lotes. Erro: {ex.Message}");
            }
        }
    }
}
