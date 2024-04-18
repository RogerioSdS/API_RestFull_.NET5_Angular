using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;

namespace MyFirstWebAPPWithAngular.Data
{
    public class ProEventosContext : DbContext
    {
        public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options) { }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }
        public DbSet<RedeSocial> RedesSociais { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PalestranteEvento>()
            .HasKey(PE => new { PE.EventoId, PE.PalestranteId });
            //Configurando chave composta para a entidade PalestranteEvento
            //O método HasKey do ModelBuilder recebe como parâmetro um objeto anônimo
            //com as propriedades que compõem a chave composta. Nesse caso, é a combinação
            //de EventoId e PalestranteId
            //Essa configuração é necessária pois a relação entre Palestrante e Evento
            //é muitos para muitos, ou seja, um Palestrante pode ter vários Eventos e
            //um Evento pode ter vários Palestrante. Com essa chave composta, o EF Core
            //entende que a combinação de EventoId e PalestranteId é uma chave única
            //para a entidade PalestranteEvento e não criará uma chave composta 
            //adicional com as mesmas colunas, gerando assim uma chave duplicada na 
            //tabela do banco de dados.
            
        }
    }
}