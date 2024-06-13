using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class LotePersist : ILotePersist
    {
        private readonly ProEventosContext _context;

        public LotePersist(ProEventosContext context)
        {
            _context = context;
            // Evita que o Entity Framework traga para memória os objetos referenciados em suas consultas
            // Isso ajuda a melhorar o desempenho de queries que apenas precisam de dados para exibição
            // Como essa classe é responsável por persistir dados, não há necessidade de trazer esses objetos para memória
            /*_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; // vamos comentar esse codigo
            //porque vou fazer o cchange em cada metodo*/
        }

        public async Task<Lote> GetLoteByIdsAsync(int eventoId, int id)
        {
            IQueryable<Lote> query = _context.Lotes;
            query = query.AsNoTracking()
            .Where(lote => lote.EventoId == eventoId && lote.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes;
            query = query.AsNoTracking()
            .Where(lote => lote.EventoId == eventoId);

            return await query.ToArrayAsync();
        }
    }
}