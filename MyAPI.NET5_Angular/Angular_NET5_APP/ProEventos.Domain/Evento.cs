using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProEventos.Domain
{
    //[Table("TB_EVENTOS_DETALHES")] caso eu quisesse mudar o nome da tabela
    public class Evento
    {
        [Key]
        public int EventoId { get; set; }
        public string Local { get; set; }
        public DateTime? DataEvento { get; set; }

        [Required]
        public string Tema { get; set; }
        public int QtdPessoas { get; set; }
        public string ImagemURL { get; set; }

        [Required]
        public string Telefone { get; set; }
        public string Email { get; set; }
        public IEnumerable<Lote> Lotes { get; set; }
        public IEnumerable<RedeSocial> RedesSociais { get; set; }
        public IEnumerable<PalestranteEvento> PalestrantesEventos { get; set; }
    }


}
