using ProEventos.Application.DTO;
using ProEventos.Application.DTOs;
using ProEventos.Domain;

namespace MyFirstWebAPPWithAngular.Helpers
{
    public class ProEventosProfile : AutoMapper.Profile
    {
       
        public ProEventosProfile()
        {
            /// <summary>
            /// Mapeia a entidade <"Evento"/> para o objeto <"EventoDTO"/> e vice-versa.
            /// </summary>
            /// <remarks>
            /// Essa mapeamento é realizado usando a biblioteca AutoMapper.
            /// </remarks>
            CreateMap<Evento, EventoDTO>().ReverseMap();
            CreateMap<Lote, LoteDTO>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDTO>().ReverseMap();
            CreateMap<Palestrante, PalestranteEventoDTO>().ReverseMap();
        }

        /// <summary>
        /// O AutoMapper é uma biblioteca de mapeamento de objetos que permite mapear
        /// automaticamente objetos de uma classe para outra, transformando os dados de uma
        /// classe em uma outra de acordo com as regras de mapeamento definidas.
        /// 
        /// Ele é muito útil quando temos classes com propriedades similares ou iguais,
        /// mas com nomes diferentes. Com o AutoMapper, podemos definir regras de
        /// mapeamento de forma simples, evitando código repetitivo e propenso a erros.
        /// </summary>
    }
}