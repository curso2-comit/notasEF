using Microsoft.EntityFrameworkCore;

namespace Notas.Models
{
    public class NotasContext : DbContext
    {
        public NotasContext(DbContextOptions<NotasContext> options)
            :base(options)
        {

        }

        public DbSet<Nota> Nota { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}