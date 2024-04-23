using Microsoft.AspNetCore.Mvc;
using ProEventos.Persistence.Contextos;
using ProEventos.Domain;
using System.Collections.Generic;
using System.Linq;
using ProEventos.Application.Contratos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

namespace MyFirstWebAPPWithAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        public IEventosService _eventosService { get; }
        public EventosController(IEventosService eventosService)
        {
            _eventosService = eventosService;            
        }

        [HttpGet]
        public async Task<IActionResult> Get()//posso colocar qualquer nome no metodo, o que manda nesse caso é o atributo do controller [HttpGet]
        {
            try{
            var eventos = await _eventosService.GetAllEventosAsync(true);
            if(eventos == null) return NotFound("Nenhum evento encontrado.");

            return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)//posso colocar qualquer nome no metodo, o que manda nesse caso é o atributo do controller [HttpGet]
        {
            try{
            var evento = await _eventosService.GetEventoByIdAsync(id,true);
            if(evento == null) return NotFound("Evento por id não encontrado.");

            return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{tema}/tema")] // inserindo o termo tema, para que o http consiga identificar o caminho
        public async Task<IActionResult> GetByTema(string tema)//posso colocar qualquer nome no metodo, o que manda nesse caso é o atributo do controller [HttpGet]
        {
            try{
            var evento = await _eventosService.GetAllEventosByTemaAsync(tema, true);
            if(evento == null) return NotFound("Eventos por tema não encontrados.");

            return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            try{
            var evento = await _eventosService.AddEventos(model);
            if(evento == null) return NotFound("Erro ao tentar adicionar o evento");

            return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id ,Evento model)
        {
            try{
            var evento = await _eventosService.UpdateEvento(id, model);
            if(evento == null) return NotFound("Erro ao tentar alterar o evento");

            return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
            return await _eventosService.DeleteEvento(id) ? 
                Ok("Deletado") :
                BadRequest("Evento não deletado");            
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar deletar o evento. Erro: {ex.Message}");
            }
        }
    }
}
