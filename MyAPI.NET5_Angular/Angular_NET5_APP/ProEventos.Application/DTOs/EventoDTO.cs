
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProEventos.Application.DTOs;

namespace ProEventos.Application.DTO
{
    public class EventoDTO
    {
        public int EventoId { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} e obrigatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres")]
        public string Tema { get; set; }

        [Display(Name = "Quantidade de Pessoas")]
        [Range(1, 120000, ErrorMessage = "O campo {0} deve ser entre {1} e {2}")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int QtdPessoas { get; set; }

        [RegularExpression(@".\*\.(gif|jpe?g|bmp|png)$", 
            ErrorMessage = "Imagem invalida. O formato permitido e '.jpg', '.jpeg', '.png', '.gif'")]
        public string ImagemURL { get; set; }

        [Required(ErrorMessage = "O campo {0} e obrigatorio")]
        [Phone(ErrorMessage = "O campo {0} deve ser valido")]
        public string Telefone { get; set; }     

        [Required(ErrorMessage = "O campo {0} é obrigatorio"),
            Display(Name = "Email do Palestrante"),
            EmailAddress(ErrorMessage = "O campo {0} deve ser valido")]
        public string Email { get; set; }
        public IEnumerable<LoteDTO> Lotes { get; set; }
        public IEnumerable<RedeSocialDTO> RedesSociais { get; set; }
        public IEnumerable<PalestranteEventoDTO> PalestrantesEventos { get; set; }
    }
}