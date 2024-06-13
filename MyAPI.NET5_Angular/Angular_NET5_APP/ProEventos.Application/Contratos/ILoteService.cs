using System.Threading.Tasks;
using ProEventos.Application.DTO;
using ProEventos.Application.DTOs;

namespace ProEventos.Application.Contratos
{
    public interface ILoteService
    {
        Task<LoteDTO[]> SaveLotes(int eventoId, LoteDTO[] model);
        Task<bool> DeleteLote(int eventoId, int loteId);
        Task<LoteDTO[]> GetLotesByEventoIdAsync(int eventoId);
        Task<LoteDTO> GetLoteByIdsAsync(int eventoId, int loteId);
    }
}