using System.Collections.Generic;
using ProEventos.Domain;

namespace ProEventos.Application.DTOs
{
    public class PalestranteEventoDTO
    {
         public int Id { get; set; }
        public string  Nome { get; set; }
        public string MiniCurriculo { get; set; }
        public string ImagemURL { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public IEnumerable<RedeSocial> RedeSociais { get; set; }
        public IEnumerable<PalestranteEvento> Palestrantes { get; set; }
    }
}