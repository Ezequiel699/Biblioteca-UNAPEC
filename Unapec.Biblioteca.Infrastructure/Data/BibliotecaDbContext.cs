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
    // Nuevo DbSet para la entidad Prestamo
    public DbSet<Prestamo> Prestamos => Set<Prestamo>();

    // TABLA PARA AUTENTICACIÓN
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Configuraciones existentes para otras entidades...
        // (aquí iría tu código existente para TiposBibliografia, etc., si lo tienes)

        // ================================
        //        PRESTAMO (Nueva Entidad)
        // ================================
        mb.Entity<Prestamo>(e =>
        {
            e.ToTable("Prestamos"); // Nombre de la tabla en la DB
            e.HasKey(p => p.Id); // Clave primaria

            // Configurar las relaciones
            e.HasOne(p => p.Usuario) // Un préstamo tiene un usuario
             .WithMany()             // (Suponiendo que Usuario no tenga una lista de préstamos aquí)
             .HasForeignKey(p => p.UsuarioId) // La FK en Prestamo
             .OnDelete(DeleteBehavior.ClientSetNull); // Otra opción común es Restrict si usas ReferentialAction.Restrict

            e.HasOne(p => p.Libro)   // Un préstamo tiene un libro
             .WithMany()             // (Suponiendo que Libro no tenga una lista de préstamos aquí)
             .HasForeignKey(p => p.LibroId) // La FK en Prestamo
             .OnDelete(DeleteBehavior.ClientSetNull); // Otra opción común es Restrict

            // Configurar propiedades
            e.Property(p => p.FechaPrestamo)
             .HasColumnType("datetime(6)")
             .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            e.Property(p => p.FechaDevolucion)
             .HasColumnType("datetime(6)")
             .IsRequired(false); // Es nullable

            e.Property(p => p.Devuelto)
             .HasDefaultValue(false);

            e.Property(p => p.CreadoEn)
             .HasColumnType("datetime(6)")
             .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            e.Property(p => p.ActualizadoEn)
             .HasColumnType("datetime(6)")
             .IsRequired(false);
        });


        // ================================
        //        APP USER (LOGIN)
        // ================================
        mb.Entity<AppUser>(e =>
        {
            e.ToTable("AppUsers");
            e.HasKey(x => x.Id);

            e.Property(x => x.Nombre).IsRequired().HasMaxLength(120);

            e.Property(x => x.PasswordHash).IsRequired().HasMaxLength(200);

            e.Property(x => x.Rol).IsRequired().HasMaxLength(20); // admin, empleado, usuario

            e.Property(x => x.Estado).HasDefaultValue(true);

            e.Property(x => x.CreadoEn)
                .HasColumnType("datetime(6)")
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            e.Property(x => x.ActualizadoEn)
                .HasColumnType("datetime(6)")
                .IsRequired(false);

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