using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.DTO;

namespace ProEventos.Application.DTOs
{
    public class RedeSocialDTO
    {
        public int Id { get; set; }
        public string Nome  { get; set; }
        public string URL { get; set; }
        public int? EventoId { get; set; }
        public EventoDTO Evento  { get; set; }
        public int? PalestranteId { get; set; }
        public PalestranteEventoDTO Palestrante { get; set; }
    }
}