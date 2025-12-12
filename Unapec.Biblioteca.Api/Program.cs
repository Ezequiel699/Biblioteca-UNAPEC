using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
// <<-- ADICIONADOS para JWT y claims
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Unapec.Biblioteca.Api.Controllers;
using Unapec.Biblioteca.Core.DTOs;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;
// -->> 

var builder = WebApplication.CreateBuilder(args);

// ========= Diagnóstico: leer cadena de conexión =========
string? cs =
    builder.Configuration.GetConnectionString("Default")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(cs))
    throw new InvalidOperationException(
        "No se encontró una cadena de conexión llamada 'Default' ni 'DefaultConnection' en appsettings.json");

var dbg = new MySqlConnectionStringBuilder(cs) { Password = "" };
Console.WriteLine($"[DBG] ConnectionStrings: Default={builder.Configuration.GetConnectionString("Default")}");
Console.WriteLine($"[DBG] ConnectionStrings: DefaultConnection={builder.Configuration.GetConnectionString("DefaultConnection")}");
Console.WriteLine($"[DB TEST] -> Server={dbg.Server}; Database={dbg.Database}; UserID={dbg.UserID}");
// =========================================================

// ---- EF Core + MySQL ----
builder.Services.AddDbContext<BibliotecaDbContext>(opt =>
    opt.UseMySql(cs, new MySqlServerVersion(new Version(8, 0, 36))));

// ---- Agregar Controllers (Importante para que funcione el AuthController) ----
builder.Services.AddControllers(); // <-- Añadido

// ---- Swagger ----
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---- FluentValidation ----
builder.Services.AddValidatorsFromAssembly(Assembly.Load("Unapec.Biblioteca.Core"));

// ---- CORS para React ----
var allowedOrigins = "_reactOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins, policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ========= ADICIONES: JWT Authentication & Authorization =========
// obtiene la clave desde appsettings.json: "Jwt:Key". Si no existe, usar valor por defecto.
// -> Cambia este valor por una clave larga en producción y guárdala en secrets / env vars.
var jwtKey = builder.Configuration["Jwt:Key"] ?? "CAMBIA_POR_UNA_CLAVE_MUY_LARGA_Y_SECRETA";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "unapec.biblioteca";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // si quieres validar issuer, cambia a true y configura ValidIssuer
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();
// ================================================================

var app = builder.Build();

// ---- Pipeline ----
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(allowedOrigins);

// <<-- ADICIONADAS: activar autent y authz en pipeline
app.UseAuthentication();
app.UseAuthorization();
// -->>

// ===== Mapear Controladores (Importante para que funcione el AuthController) =====
app.MapControllers(); // <-- Añadido ANTES de los otros endpoints minimal API
// ================================================================================

app.MapEditorasEndpoints();
app.MapCienciasEndpoints();
app.MapIdiomasEndpoints();

// ======= Ejemplo del template (lo dejamos) =======
var summaries = new[]
{
    "Freezing","Bracing","Chilly","Cool","Mild","Warm","Balmy","Hot","Sweltering","Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        )).ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();



// ========== LIBROS ==========
{
    var group = app.MapGroup("/api/libros").WithTags("libros");

    group.MapGet("", (BibliotecaDbContext db, int page = 1, int pageSize = 20, string? q = null, bool? estado = null) =>
    {
        var query = db.Set<Libro>()
            .Include(l => l.TipoBibliografia)
            .Include(l => l.Editora)
            .Include(l => l.Ciencia)
            .Include(l => l.Idioma)
            .Include(l => l.Autores)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(l => l.Descripcion.Contains(q) || (l.ISBN != null && l.ISBN.Contains(q)));

        if (estado.HasValue)
            query = query.Where(l => l.Estado == estado);

        return Results.Ok(new
        {
            page,
            pageSize,
            total = query.Count(),
            items = query.OrderBy(l => l.Descripcion)
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .ToList()
        });
    });

    group.MapGet("{id:int}", async (BibliotecaDbContext db, int id) =>
        await db.Set<Libro>()
            .Include(l => l.TipoBibliografia)
            .Include(l => l.Editora)
            .Include(l => l.Ciencia)
            .Include(l => l.Idioma)
            .Include(l => l.Autores)
            .FirstOrDefaultAsync(l => l.Id == id)
            is { } l ? Results.Ok(l) : Results.NotFound());

    group.MapPost("", async (BibliotecaDbContext db, IValidator<LibroCreateDto> validator, LibroCreateDto dto) =>
    {
        var vr = await validator.ValidateAsync(dto);
        if (!vr.IsValid) return Results.ValidationProblem(vr.ToDictionary());

        // validar FKs
        async Task<bool> Exists<T>(DbSet<T> set, int id) where T : BaseCatalog => await set.AnyAsync(x => x.Id == id && x.Estado);
        if (!await Exists(db.TiposBibliografia, dto.TipoBibliografiaId)) return Results.BadRequest(new { error = "TipoBibliografiaId inválido" });
        if (!await Exists(db.Editoras, dto.EditoraId)) return Results.BadRequest(new { error = "EditoraId inválido" });
        if (!await Exists(db.Ciencias, dto.CienciaId)) return Results.BadRequest(new { error = "CienciaId inválido" });
        if (!await Exists(db.Idiomas, dto.IdiomaId)) return Results.BadRequest(new { error = "IdiomaId inválido" });

        var autores = (dto.AutorIds is { Count: > 0 })
            ? await db.Autores.Where(a => dto.AutorIds!.Contains(a.Id) && a.Estado).ToListAsync()
            : new List<Autor>();

        if (dto.AutorIds is { Count: > 0 } && autores.Count != dto.AutorIds.Count)
            return Results.BadRequest(new { error = "Uno o más AutorIds no existen o están inactivos" });

        var entity = new Libro
        {
            Descripcion = dto.Descripcion,
            SignaturaTopografica = dto.SignaturaTopografica,
            ISBN = dto.ISBN,
            TipoBibliografiaId = dto.TipoBibliografiaId,
            EditoraId = dto.EditoraId,
            AnioPublicacion = dto.AnioPublicacion,
            CienciaId = dto.CienciaId,
            IdiomaId = dto.IdiomaId,
            Estado = dto.Estado,
            Autores = autores
        };

        db.Add(entity);
        await db.SaveChangesAsync();
        return Results.Created($"/api/libros/{entity.Id}", entity);
    });

    group.MapPut("{id:int}", async (BibliotecaDbContext db, IValidator<LibroUpdateDto> validator, int id, LibroUpdateDto dto) =>
    {
        var vr = await validator.ValidateAsync(dto);
        if (!vr.IsValid) return Results.ValidationProblem(vr.ToDictionary());

        var entity = await db.Set<Libro>()
            .Include(l => l.Autores)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (entity is null) return Results.NotFound();

        // validar FKs
        async Task<bool> Exists<T>(DbSet<T> set, int fk) where T : BaseCatalog => await set.AnyAsync(x => x.Id == fk && x.Estado);
        if (!await Exists(db.TiposBibliografia, dto.TipoBibliografiaId)) return Results.BadRequest(new { error = "TipoBibliografiaId inválido" });
        if (!await Exists(db.Editoras, dto.EditoraId)) return Results.BadRequest(new { error = "EditoraId inválido" });
        if (!await Exists(db.Ciencias, dto.CienciaId)) return Results.BadRequest(new { error = "CienciaId inválido" });
        if (!await Exists(db.Idiomas, dto.IdiomaId)) return Results.BadRequest(new { error = "IdiomaId inválido" });

        entity.Descripcion = dto.Descripcion;
        entity.SignaturaTopografica = dto.SignaturaTopografica;
        entity.ISBN = dto.ISBN;
        entity.TipoBibliografiaId = dto.TipoBibliografiaId;
        entity.EditoraId = dto.EditoraId;
        entity.AnioPublicacion = dto.AnioPublicacion;
        entity.CienciaId = dto.CienciaId;
        entity.IdiomaId = dto.IdiomaId;
        entity.Estado = dto.Estado;
        entity.ActualizadoEn = DateTime.UtcNow;

        // actualizar autores (many-to-many)
        entity.Autores.Clear();
        if (dto.AutorIds is { Count: > 0 })
        {
            var autores = await db.Autores.Where(a => dto.AutorIds.Contains(a.Id) && a.Estado).ToListAsync();
            if (autores.Count != dto.AutorIds.Count)
                return Results.BadRequest(new { error = "Uno o más AutorIds no existen o están inactivos" });
            foreach (var a in autores) entity.Autores.Add(a);
        }

        await db.SaveChangesAsync();
        return Results.NoContent();
    });

    // Soft delete
    group.MapDelete("{id:int}", async (BibliotecaDbContext db, int id) =>
    {
        var entity = await db.Set<Libro>().FindAsync(id);
        if (entity is null) return Results.NotFound();
        entity.Estado = false;
        entity.ActualizadoEn = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return Results.NoContent();
    });
} //  Cerrar el scope de libros aquí

// ======= CRUD EMPLEADOS =======
{
    var group = app.MapGroup("/api/empleados").WithTags("empleados");

    // Listar con paginación
    group.MapGet("", (BibliotecaDbContext db, int page = 1, int pageSize = 20, bool? estado = null) =>
    {
        var query = db.Empleados.AsQueryable();
        if (estado.HasValue) query = query.Where(e => e.Estado == estado);

        return Results.Ok(new
        {
            page,
            pageSize,
            total = query.Count(),
            items = query.OrderBy(e => e.Nombre)
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .ToList()
        });
    });

    // Obtener por ID
    group.MapGet("{id:int}", async (BibliotecaDbContext db, int id) =>
        await db.Empleados.FindAsync(id) is { } e ? Results.Ok(e) : Results.NotFound());

    // Crear
    group.MapPost("", async (BibliotecaDbContext db, IValidator<EmpleadoCreateDto> validator, EmpleadoCreateDto dto) =>
    {
        var vr = await validator.ValidateAsync(dto);
        if (!vr.IsValid) return Results.ValidationProblem(vr.ToDictionary());

        var empleado = new Empleado
        {
            Nombre = dto.Nombre,
            Cedula = dto.Cedula,
            TandaLabor = dto.TandaLabor,
            PorcientoComision = dto.PorcientoComision,
            FechaIngreso = dto.FechaIngreso,
            Estado = dto.Estado
        };

        db.Empleados.Add(empleado);
        await db.SaveChangesAsync();
        return Results.Created($"/api/empleados/{empleado.Id}", empleado);
    });

    // Actualizar
    group.MapPut("{id:int}", async (BibliotecaDbContext db, IValidator<EmpleadoUpdateDto> validator, int id, EmpleadoUpdateDto dto) =>
    {
        var vr = await validator.ValidateAsync(dto);
        if (!vr.IsValid) return Results.ValidationProblem(vr.ToDictionary());

        var empleado = await db.Empleados.FindAsync(id);
        if (empleado is null) return Results.NotFound();

        empleado.Nombre = dto.Nombre;
        empleado.Cedula = dto.Cedula;
        empleado.TandaLabor = dto.TandaLabor;
        empleado.PorcientoComision = dto.PorcientoComision;
        empleado.FechaIngreso = dto.FechaIngreso;
        empleado.Estado = dto.Estado;

        await db.SaveChangesAsync();
        return Results.NoContent();
    });

    // Eliminar lógico
    group.MapDelete("{id:int}", async (BibliotecaDbContext db, int id) =>
    {
        var empleado = await db.Empleados.FindAsync(id);
        if (empleado is null) return Results.NotFound();

        empleado.Estado = false;
        await db.SaveChangesAsync();
        return Results.NoContent();
    });
}

// ======= CRUD USUARIOS =======
{
    var group = app.MapGroup("/api/usuarios").WithTags("usuarios");

    // Listar sin paginación (como está ahora)
    group.MapGet("", (BibliotecaDbContext db, bool? estado = null) =>
    {
        var query = db.Usuarios.AsQueryable();
        if (estado.HasValue) query = query.Where(u => u.Estado == estado);
        return Results.Ok(query.OrderBy(u => u.Nombre).ToList());
    });

    // Obtener por ID
    group.MapGet("{id:int}", async (BibliotecaDbContext db, int id) =>
        await db.Usuarios.FindAsync(id) is { } u ? Results.Ok(u) : Results.NotFound());

    // Crear
    group.MapPost("", async (BibliotecaDbContext db, IValidator<UsuarioCreateDto> validator, UsuarioCreateDto dto) =>
    {
        var vr = await validator.ValidateAsync(dto);
        if (!vr.IsValid) return Results.ValidationProblem(vr.ToDictionary());

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Cedula = dto.Cedula,
            NoCarnet = dto.NoCarnet,
            TipoPersona = dto.TipoPersona,
            Estado = dto.Estado
        };

        db.Usuarios.Add(usuario);
        await db.SaveChangesAsync();
        return Results.Created($"/api/usuarios/{usuario.Id}", usuario);
    });

    // Actualizar
    group.MapPut("{id:int}", async (BibliotecaDbContext db, IValidator<UsuarioUpdateDto> validator, int id, UsuarioUpdateDto dto) =>
    {
        var vr = await validator.ValidateAsync(dto);
        if (!vr.IsValid) return Results.ValidationProblem(vr.ToDictionary());

        var usuario = await db.Usuarios.FindAsync(id);
        if (usuario is null) return Results.NotFound();

        usuario.Nombre = dto.Nombre;
        usuario.Cedula = dto.Cedula;
        usuario.NoCarnet = dto.NoCarnet;
        usuario.TipoPersona = dto.TipoPersona;
        usuario.Estado = dto.Estado;

        await db.SaveChangesAsync();
        return Results.NoContent();
    });

    // Eliminar lógico
    group.MapDelete("{id:int}", async (BibliotecaDbContext db, int id) =>
    {
        var usuario = await db.Usuarios.FindAsync(id);
        if (usuario is null) return Results.NotFound();

        usuario.Estado = false;
        await db.SaveChangesAsync();
        return Results.NoContent();
    });
}

app.Run(); // El endpoint login ya no está aquí

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}