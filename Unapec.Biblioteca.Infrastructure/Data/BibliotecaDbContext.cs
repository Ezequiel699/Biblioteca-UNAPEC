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
    public DbSet<Libro> Libros => Set<Libro>();
    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    // 👇 NUEVA TABLA PARA AUTENTICACIÓN
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // ... (todo tu código existente)

        // ================================
        //        APP USER (LOGIN)
        // ================================
        mb.Entity<AppUser>(e =>
        {
            e.ToTable("AppUsers");
            e.HasKey(x => x.Id);

            e.Property(x => x.Nombre).IsRequired().HasMaxLength(120);
            e.Property(x => x.Cedula).IsRequired().HasMaxLength(11);

            e.Property(x => x.UserName).IsRequired().HasMaxLength(50);
            e.Property(x => x.PasswordHash).IsRequired().HasMaxLength(200);

            e.Property(x => x.Rol).IsRequired().HasMaxLength(20); // admin, empleado, usuario

            e.Property(x => x.Estado).HasDefaultValue(true);

            e.Property(x => x.CreadoEn)
                .HasColumnType("datetime(6)")
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            e.Property(x => x.ActualizadoEn)
                .HasColumnType("datetime(6)")
                .IsRequired(false);

            e.HasIndex(x => x.UserName).IsUnique();
            e.HasIndex(x => x.Cedula).IsUnique();
        });
    }
}
