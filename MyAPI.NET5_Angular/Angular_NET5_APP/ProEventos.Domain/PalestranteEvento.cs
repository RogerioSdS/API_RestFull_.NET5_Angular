using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Domain
{
    public class PalestranteEvento
    {
        public int PalestranteId { get; set; } //è uma convenção, do .Net Framework, colocarmos o nome
        //da classe seguido de ID para ele saber que se trata de chave estrangeira, para não usar essa convenção
        //e colocar qualquer nome no atributo devemos colocar um data annotation
        public Palestrante Palestrante { get; set; }
        public int EventoId { get; set; }
        public Evento Evento { get; set; }
    }
}