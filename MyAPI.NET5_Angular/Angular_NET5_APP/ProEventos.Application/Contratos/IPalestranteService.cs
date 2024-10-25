using System.Threading.Tasks;
using ProEventos.Application.DTOs;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Contratos
{
    public interface IPalestranteService
    {
        Task<PalestranteDTO> AddPalestrantes(int userId, PalestranteAddDTO model);
        Task<PalestranteDTO> UpdatePalestrante(int userId, PalestranteUpdateDTO model);
        Task<PageList<PalestranteDTO>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
        Task<PalestranteDTO> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);
    }
}