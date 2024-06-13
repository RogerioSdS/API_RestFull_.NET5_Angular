using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface ILotePersist
    {   
        /// <summary>
        /// Metodo que retorna uma lista de lotes por eventoId
        /// </summary>
        /// <param name="eventoId">Codigo do evento</param>
        /// <returns>Lista de lotes</returns>
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
        /// <summary>
        /// Metodo que retorna apenas um lote
        /// </summary>
        /// <param name="eventoId">codigo do evento</param>
        /// <param name="id">Código do Lote</param>
        /// <returns>Apenas um lote</returns>
        Task<Lote> GetLoteByIdsAsync(int eventoId, int id);
    }
}