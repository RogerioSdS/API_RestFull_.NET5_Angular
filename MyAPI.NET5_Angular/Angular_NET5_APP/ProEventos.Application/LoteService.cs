using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.DTOs;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class LoteService : ILoteService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly ILotePersist _lotePersist;
        private readonly IMapper _mapper;

        public LoteService(IGeralPersist geralPersist, ILotePersist lotePersist, IMapper mapper)
        {
            _geralPersist = geralPersist;
            _lotePersist = lotePersist;
            _mapper = mapper;
        }
        public async Task AddLote(int eventoId, LoteDTO model)
        {
            try
            {
                //Mapeia o objeto LoteDTO para o objeto Evento.
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;
                //Adiciona o obj. do tipo Lote no banco de dados.
                _geralPersist.Add<Lote>(lote);

                await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDTO[]> SaveLotes(int eventoId, LoteDTO[] models)
        {
            try
            {
                var lotes = await _lotePersist.GetLotesByEventoIdAsync(eventoId);

                if (lotes == null)
                    return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddLote(eventoId, model);
                    }
                    else
                    {
                        var lote = lotes
                            .FirstOrDefault(lote => lote.Id == model.Id);

                        model.EventoId = eventoId;
                        // Aqui estamos usando o método Map do mapper para mapear o objeto model para o objeto evento.
                        // O método Map recebe como argumentos o objeto de origem (model) e o objeto de destino (evento).
                        // O método Map atualiza o objeto evento com os valores do objeto model.
                        // Essa atualização é feita diretamente no objeto evento recuperado do banco de dados.
                        // Dessa forma, estamos atualizando o objeto evento diretamente no banco de dados.
                        // Isso é importante, pois quando usamos o método Update do geralPersist, ele cria um novo objeto evento
                        // e o insere no banco de dados, e não atualiza o objeto existente.
                        // O método Map é útil para atualizar os valores de um objeto existente.
                        _mapper.Map(model, lote);

                        _geralPersist.Update<Lote>(lote);

                        await _geralPersist.SaveChangesAsync();
                    }
                }

                {
                    var loteRetorno = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
                    return _mapper.Map<LoteDTO[]>(loteRetorno);
                }
               
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null)
                    throw new Exception("Lote para delete não encontrado");

                _geralPersist.Delete<Lote>(lote);
                return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDTO[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;

                var resultados = _mapper.Map<LoteDTO[]>(lotes);

                return resultados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDTO> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                //Busca no banco de dados o evento pelo id.
                var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null) return null;

                // Mapeia o evento recuperado do banco de dados para o objeto LoteDTO usando o mapper.
                // 
                // O método Map do mapper recebe como argumentos o objeto de origem (eventos) e o tipo de destino (LoteDTO).
                // 
                // O método Map retorna um objeto do tipo LoteDTO, que é atribuído à variável resultado.
                var resultado = _mapper.Map<LoteDTO>(lote);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}