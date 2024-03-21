using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPPWithAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstWebAPPWithAngular.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public IEnumerable<Evento> _evento = new List<Evento>()
        {
            new Evento()
            {
            EventoId = 1,
            Tema = "Angular 11 e .Net 5",
            Local = "Belo Horizonte",
            Lote = "1º Lote",
            QtdPessoas = 250,
            DataEvento = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy"),
            ImagemURL = "imagem.png"
           },
           new Evento()
            {
            EventoId = 2,
            Tema = "Angular 11 e .Net 5 e suas novidades",
            Local = "São Paulo",
            Lote = "2º Lote",
            QtdPessoas = 350,
            DataEvento = DateTime.Now.AddDays(4).ToString("dd/MM/yyyy"),
            ImagemURL = "imagem2.png"
           }
        };
        public EventoController()
        {

        }

        [HttpGet]
        public IEnumerable<Evento> Get()//posso colocar qualquer nome no metodo, o que manda nesse caso é o atributo do controller [HttpGet]
        {
            return _evento;
        }
        [HttpGet("{id}")]
        public IEnumerable<Evento> GetById(int id)//posso colocar qualquer nome no metodo, o que manda nesse caso é o atributo do controller [HttpGet]
        {
            return _evento.Where(evento => evento.EventoId == id);
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
