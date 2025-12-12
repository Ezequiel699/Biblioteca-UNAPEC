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

    // TABLA PARA AUTENTICACIÓN
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Configuraciones existentes para otras entidades...
        // (aquí iría tu código existente para TiposBibliografia, etc., si lo tienes)

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

        // ================================
        //     RELACIONES CON APPUSER
        // ================================

        // Relación: Usuario puede tener 0 o 1 AppUser
        mb.Entity<Usuario>(e =>
        {
            e.HasOne(u => u.AppUser) // Suponiendo que tienes la propiedad de navegación 'AppUser' en Usuario
             .WithMany()             // 'WithMany()' vacío porque AppUser no tiene una colección de Usuarios
             .HasForeignKey(u => u.AppUserId) // La FK en Usuario es AppUserId
             .OnDelete(DeleteBehavior.SetNull); // Si AppUser se borra, AppUserId se vuelve NULL
        });

        // Relación: Empleado puede tener 0 o 1 AppUser
        mb.Entity<Empleado>(e =>
        {
            e.HasOne(emp => emp.AppUser) // Suponiendo que tienes la propiedad de navegación 'AppUser' en Empleado
             .WithMany()                 // 'WithMany()' vacío porque AppUser no tiene una colección de Empleados
             .HasForeignKey(emp => emp.AppUserId) // La FK en Empleado es AppUserId
             .OnDelete(DeleteBehavior.SetNull); // Si AppUser se borra, AppUserId se vuelve NULL
        });

        // Asegúrate de que tus entidades Usuario y Empleado tengan:
        // - Una propiedad int? AppUserId (clave foránea)
        // - Una propiedad AppUser? AppUser (propiedad de navegación)
    }
}