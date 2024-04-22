using Microsoft.AspNetCore.Mvc;
using ProEventos.Persistence.Contextos;
using ProEventos.Domain;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstWebAPPWithAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
       
        private readonly ProEventosContext _context;

        public EventosController(ProEventosContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Evento> Get()//posso colocar qualquer nome no metodo, o que manda nesse caso é o atributo do controller [HttpGet]
        {
            return _context.Eventos;
        }
        [HttpGet("{id}")]
        public Evento GetById(int id)//posso colocar qualquer nome no metodo, o que manda nesse caso é o atributo do controller [HttpGet]
        {
            return _context.Eventos.FirstOrDefault(evento => evento.Id == id);
        }

        [HttpPost]
        public string Post()
        {
            return "Exemplo de Post";
        }

        [HttpPut("{id}")]
        public string Put(string id)
        {
            return $"Exemplo de Put com id {id}";
        }

        [HttpDelete("{id}")]
        public string Delete(string id)
        {
            return $"Exemplo de Delete com id {id}";
        }
    }
}
