using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.DTO;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventosService : IEventosService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventoPersist _eventoPersist;
        private readonly IMapper _mapper;

        public EventosService(IGeralPersist geralPersist, IEventoPersist eventoPersist, IMapper mapper)
        {
            _geralPersist = geralPersist;
            _eventoPersist = eventoPersist;
            _mapper = mapper;
        }
        public async Task<EventoDTO> AddEventos(EventoDTO model)
        {
            try
            {
                //Mapeia o objeto EventoDTO para o objeto Evento.
                var evento = _mapper.Map<Evento>(model);
                //Adiciona o obj. do tipo vento no banco de dados.
                _geralPersist.Add<Evento>(evento);
                
                if (await _geralPersist.SaveChangesAsync())
                {
                    //Retorna o evento recuperado do banco de dados.
                    var retorno = await _eventoPersist.GetEventoByIdAsync(evento.EventoId, false);
                  //Mapeia o evento recuperado do banco de dados para o objeto EventoDTO e o retorna.
                    return _mapper.Map<EventoDTO>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO> UpdateEvento(int eventoId, EventoDTO model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false);

                if (evento == null)
                    return null;       

                model.EventoId = evento.EventoId;       

                // Aqui estamos usando o método Map do mapper para mapear o objeto model para o objeto evento.
                // O método Map recebe como argumentos o objeto de origem (model) e o objeto de destino (evento).
                // O método Map atualiza o objeto evento com os valores do objeto model.
                // Essa atualização é feita diretamente no objeto evento recuperado do banco de dados.
                // Dessa forma, estamos atualizando o objeto evento diretamente no banco de dados.
                // Isso é importante, pois quando usamos o método Update do geralPersist, ele cria um novo objeto evento
                // e o insere no banco de dados, e não atualiza o objeto existente.
                // O método Map é útil para atualizar os valores de um objeto existente.
                _mapper.Map(model, evento);  

                _geralPersist.Update<Evento>(evento);
                
                if (await _geralPersist.SaveChangesAsync())
                {
                    var retorno = await _eventoPersist.GetEventoByIdAsync(evento.EventoId, false);
                    return _mapper.Map<EventoDTO>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false);
                if (evento == null)
                    throw new Exception("Evento para delete não encontrado");

                _geralPersist.Delete(evento);
                return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;

                var resultados = _mapper.Map<EventoDTO[]>(eventos);

                return resultados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDTO[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null) return null;

                var resultados = _mapper.Map<EventoDTO[]>(eventos);

                return resultados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDTO> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                //Busca no banco de dados o evento pelo id.
                var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);
                if (evento == null) return null;

                // Mapeia o evento recuperado do banco de dados para o objeto EventoDTO usando o mapper.
                // 
                // O método Map do mapper recebe como argumentos o objeto de origem (eventos) e o tipo de destino (EventoDTO).
                // 
                // O método Map retorna um objeto do tipo EventoDTO, que é atribuído à variável resultado.
                var resultado = _mapper.Map<EventoDTO>(evento);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}