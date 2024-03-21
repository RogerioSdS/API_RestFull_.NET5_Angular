using Microsoft.EntityFrameworkCore;
using MyFirstWebAPPWithAngular.Models;

namespace MyFirstWebAPPWithAngular.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
       public DbSet<Evento> Eventos { get; set; } 
    }
}