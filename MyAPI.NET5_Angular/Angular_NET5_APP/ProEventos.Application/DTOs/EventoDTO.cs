
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProEventos.Application.DTOs;
using ProEventos.Domain.Identity;

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

        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", 
            ErrorMessage = "Imagem inválida. Os formatos permitidos são '.jpg', '.jpeg', '.png', '.gif', '.bmp'")]

        public string ImagemURL { get; set; }

        [Required(ErrorMessage = "O campo {0} e obrigatorio")]
        [Phone(ErrorMessage = "O campo {0} deve ser valido")]
        public string Telefone { get; set; }     

        [Required(ErrorMessage = "O campo {0} é obrigatorio"),
            Display(Name = "Email do Palestrante"),
            EmailAddress(ErrorMessage = "O campo {0} deve ser valido")]
        public string Email { get; set; }
        public int UserId { get; set; }
        public User UserDTO { get; set; }
        public IEnumerable<LoteDTO> Lotes { get; set; }
        public IEnumerable<RedeSocialDTO> RedesSociais { get; set; }
        public IEnumerable<PalestranteDTO> PalestrantesEventos { get; set; }
    }
}