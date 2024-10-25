using ProEventos.Application.DTO;
using System;

namespace ProEventos.Application.DTOs
{
    public class LoteDTO
    {
         public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string DataInicio { get; set; }   
        public string DataFim { get; set; }  
        public int Quantidade { get; set; }
        public int EventoId { get; set; }
        public EventoDTO Evento { get; set; }
    }
}