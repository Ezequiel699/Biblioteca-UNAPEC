using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.Entities;

namespace Unapec.Biblioteca.Infrastructure.Data;

public class BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options) : DbContext(options)
{
    public DbSet<TipoBibliografia> TiposBibliografia => Set<TipoBibliografia>();
    public DbSet<Editora> Editoras => Set<Editora>();
    public DbSet<Ciencia> Ciencias => Set<Ciencia>();
    public DbSet<Idioma> Idiomas => Set<Idioma>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        void Map<T>(string table) where T : BaseCatalog
        {
            mb.Entity<T>(e =>
            {
                e.ToTable(table);
                e.HasKey(x => x.Id);
                e.Property(x => x.Descripcion).IsRequired().HasMaxLength(120);
                e.Property(x => x.Estado).HasDefaultValue(true);
                e.Property(x => x.CreadoEn)
                .HasColumnType("datetime(6)")
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            });
        }

        Map<TipoBibliografia>("TiposBibliografia");
        Map<Editora>("Editoras");
        Map<Ciencia>("Ciencias");
        Map<Idioma>("Idiomas");
    }
}
