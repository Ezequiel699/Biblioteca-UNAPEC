using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.Entities;

namespace Unapec.Biblioteca.Infrastructure.Data;

public class BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options) : DbContext(options)
{
    public DbSet<TipoBibliografia> TiposBibliografia => Set<TipoBibliografia>();
    public DbSet<Editora> Editoras => Set<Editora>();
    public DbSet<Ciencia> Ciencias => Set<Ciencia>();
    public DbSet<Idioma> Idiomas => Set<Idioma>();
    public DbSet<Autor> Autores => Set<Autor>();
    public DbSet<Libro> Libros => Set<Libro>(); // 👈 NUEVO
    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();



    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Mapeo genérico para catálogos base
        void MapCatalog<T>(string table) where T : BaseCatalog
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
                e.Property(x => x.ActualizadoEn)
                    .HasColumnType("datetime(6)")
                    .IsRequired(false);
            });
        }

        mb.Entity<Usuario>(e =>
        {
            e.ToTable("Usuarios");
            e.HasKey(x => x.Id);

            e.Property(x => x.Nombre).IsRequired().HasMaxLength(120);
            e.Property(x => x.Cedula).IsRequired().HasMaxLength(11);
            e.Property(x => x.NoCarnet).IsRequired().HasMaxLength(20);
            e.Property(x => x.TipoPersona).IsRequired().HasMaxLength(20);
            e.Property(x => x.Estado).HasDefaultValue(true);

            e.HasIndex(x => x.Cedula).IsUnique();
            e.HasIndex(x => x.NoCarnet).IsUnique();
        });


        mb.Entity<Empleado>(e =>
        {
            e.ToTable("Empleados");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nombre).IsRequired().HasMaxLength(120);
            e.Property(x => x.Cedula).IsRequired().HasMaxLength(11);
            e.Property(x => x.TandaLabor).HasMaxLength(50);
            e.Property(x => x.PorcientoComision).HasPrecision(5, 2);
            e.Property(x => x.FechaIngreso).HasColumnType("datetime(6)");
            e.Property(x => x.Estado).HasDefaultValue(true);
        });


        MapCatalog<TipoBibliografia>("TiposBibliografia");
        MapCatalog<Editora>("Editoras");
        MapCatalog<Ciencia>("Ciencias");
        MapCatalog<Idioma>("Idiomas");

        // Autor (no hereda de BaseCatalog)
        mb.Entity<Autor>(e =>
        {
            e.ToTable("Autores");
            e.HasKey(x => x.Id);

            e.Property(x => x.Nombre).IsRequired().HasMaxLength(120);
            e.Property(x => x.PaisOrigen).HasMaxLength(80);

            e.Property(x => x.Estado).HasDefaultValue(true);
            e.Property(x => x.CreadoEn)
                .HasColumnType("datetime(6)")
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            e.Property(x => x.ActualizadoEn)
                .HasColumnType("datetime(6)")
                .IsRequired(false);

            e.HasOne(x => x.IdiomaNativo)
                .WithMany()
                .HasForeignKey(x => x.IdiomaNativoId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.Nombre);
            e.HasIndex(x => x.IdiomaNativoId);
        });

        // Libro + relaciones
        mb.Entity<Libro>(e =>
        {
            e.ToTable("Libros");
            e.HasKey(x => x.Id);

            e.Property(x => x.Descripcion).IsRequired().HasMaxLength(200);
            e.Property(x => x.SignaturaTopografica).HasMaxLength(50);
            e.Property(x => x.ISBN).HasMaxLength(20);

            e.Property(x => x.Estado).HasDefaultValue(true);
            e.Property(x => x.CreadoEn)
                .HasColumnType("datetime(6)")
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            e.Property(x => x.ActualizadoEn)
                .HasColumnType("datetime(6)")
                .IsRequired(false);

            // FKs con catálogos
            e.HasOne(x => x.TipoBibliografia)
                .WithMany()
                .HasForeignKey(x => x.TipoBibliografiaId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Editora)
                .WithMany()
                .HasForeignKey(x => x.EditoraId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Ciencia)
                .WithMany()
                .HasForeignKey(x => x.CienciaId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Idioma)
                .WithMany()
                .HasForeignKey(x => x.IdiomaId)
                .OnDelete(DeleteBehavior.Restrict);

            // N–M Libros <-> Autores
            e.HasMany(x => x.Autores)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "AutoresLibros",
                    r => r.HasOne<Autor>()
                          .WithMany()
                          .HasForeignKey("AutorId")
                          .OnDelete(DeleteBehavior.Cascade),
                    l => l.HasOne<Libro>()
                          .WithMany()
                          .HasForeignKey("LibroId")
                          .OnDelete(DeleteBehavior.Cascade),
                    je =>
                    {
                        je.ToTable("AutoresLibros");
                        je.HasKey("AutorId", "LibroId");
                        je.HasIndex("LibroId");
                    });

            // Índices útiles
            e.HasIndex(x => x.ISBN);
            e.HasIndex(x => x.Descripcion);
            e.HasIndex(x => new { x.AnioPublicacion, x.IdiomaId });
        });
    }
}
