using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Domain
{
    public class Lote
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public DateTime? DataInicio { get; set; }   
        public DateTime? DataFim { get; set; }  
        public int Quantidade { get; set; }
        [ForeignKey("TB_EVENTOS_DETALHES")]
        public int EventoId { get; set; }
        public Evento Evento { get; set; }
    }
}