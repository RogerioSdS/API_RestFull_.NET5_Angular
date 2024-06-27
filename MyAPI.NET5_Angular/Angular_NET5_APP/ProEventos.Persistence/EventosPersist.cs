using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
    public class EventosPersist : IEventoPersist
    {
        private readonly ProEventosContext _context;

        public EventosPersist(ProEventosContext context)
        {
            _context = context;
            // Evita que o Entity Framework traga para memória os objetos referenciados em suas consultas
            // Isso ajuda a melhorar o desempenho de queries que apenas precisam de dados para exibição
            // Como essa classe é responsável por persistir dados, não há necessidade de trazer esses objetos para memória
            /*_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; // vamos comentar esse codigo
            //porque vou fazer o cchange em cada metodo*/
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);
            
            if(includePalestrantes)
            {
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }
            
            query = query.AsNoTracking()
                .OrderBy(e => e.EventoId)
                .Where(e => e.Tema.ToLower().Contains(tema.ToLower())
                        && e.UserId == userId);

            return await query.ToArrayAsync();            
        }

        public async Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);
            
            if(includePalestrantes)
            {
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }
            
            query = query.AsNoTracking()
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.EventoId);

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
             IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);
            
            if(includePalestrantes)
            {
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }
            
            query = query
                .AsNoTracking()
                .Where(e => e.EventoId == eventoId
                        && e.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }
    }
}