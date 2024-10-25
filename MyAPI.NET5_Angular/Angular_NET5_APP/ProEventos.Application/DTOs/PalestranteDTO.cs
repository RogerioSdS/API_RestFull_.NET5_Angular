using System.Collections.Generic;
using ProEventos.Application.DTO;

namespace ProEventos.Application.DTOs
{
    public class PalestranteDTO
    {
        public int Id { get; set; }
        public string MiniCurriculo { get; set; }
        public int UserId { get; set; }
        public UserUpdateDTO User { get; set; }
        public IEnumerable<RedeSocialDTO> RedesSociais { get; set; }
        public IEnumerable<EventoDTO> Eventos { get; set; }
    }
}