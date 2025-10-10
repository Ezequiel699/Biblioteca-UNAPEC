using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Reflection;
using FluentValidation;
using Unapec.Biblioteca.Core.DTOs;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// ---- EF Core + MySQL ----
var cs = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<BibliotecaDbContext>(opt =>
    opt.UseMySql(cs, new MySqlServerVersion(new Version(8, 0, 36))));

// ---- Swagger ----
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---- FluentValidation (registrar validadores antes de Build) ----
builder.Services.AddValidatorsFromAssembly(Assembly.Load("Unapec.Biblioteca.Core"));

// ---- CORS para React ----
var allowedOrigins = "_reactOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins, policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173", // Vite
                "http://localhost:3000"  // Create React App
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ---- Pipeline ----
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// aplicar CORS antes de mapear endpoints
app.UseCors(allowedOrigins);

// ======= Ejemplo del template (lo dejamos) =======
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// ========== BEGIN CRUD CATÁLOGOS ==========

// Utilidad de paginación
IResult Paged<T>(IQueryable<T> q, int page, int pageSize) where T : BaseCatalog =>
    Results.Ok(new
    {
        page,
        pageSize,
        total = q.Count(),
        items = q.OrderBy(x => x.Id)
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .ToList()
    });

// Mapeo genérico de catálogos (el DbContext se inyecta en cada request)
void MapCatalog<T>(string baseRoute) where T : BaseCatalog
{
    var group = app.MapGroup($"/api/{baseRoute}").WithTags(baseRoute);

    // Listar con búsqueda/paginación/estado
    group.MapGet("", (BibliotecaDbContext db, int page = 1, int pageSize = 20, string? q = null, bool? estado = null) =>
    {
        var query = db.Set<T>().AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Descripcion.Contains(q));
        if (estado.HasValue) query = query.Where(x => x.Estado == estado);
        return Paged(query, page, pageSize);
    });

    // Obtener por id
    group.MapGet("{id:int}", async (BibliotecaDbContext db, int id) =>
        await db.Set<T>().FindAsync(id) is { } e ? Results.Ok(e) : Results.NotFound());

    // Crear
    group.MapPost("", async (BibliotecaDbContext db, IValidator<CatalogCreateDto> validator, CatalogCreateDto dto) =>
    {
        var vr = await validator.ValidateAsync(dto);
        if (!vr.IsValid) return Results.ValidationProblem(vr.ToDictionary());

        var entity = Activator.CreateInstance<T>()!;
        entity.Descripcion = dto.Descripcion;
        entity.Estado = dto.Estado;

        db.Set<T>().Add(entity);
        await db.SaveChangesAsync();
        return Results.Created($"/api/{baseRoute}/{entity.Id}", entity);
    });

    // Actualizar
    group.MapPut("{id:int}", async (BibliotecaDbContext db, IValidator<CatalogUpdateDto> validator, int id, CatalogUpdateDto dto) =>
    {
        var vr = await validator.ValidateAsync(dto);
        if (!vr.IsValid) return Results.ValidationProblem(vr.ToDictionary());

        var entity = await db.Set<T>().FindAsync(id);
        if (entity is null) return Results.NotFound();

        entity.Descripcion = dto.Descripcion;
        entity.Estado = dto.Estado;
        entity.ActualizadoEn = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Results.NoContent();
    });

    // Eliminar (soft-delete = Estado=false)
    group.MapDelete("{id:int}", async (BibliotecaDbContext db, int id) =>
    {
        var entity = await db.Set<T>().FindAsync(id);
        if (entity is null) return Results.NotFound();

        entity.Estado = false;
        entity.ActualizadoEn = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Results.NoContent();
    });
}

// Mapear los 4 catálogos
MapCatalog<TipoBibliografia>("tipos-bibliografia");
MapCatalog<Editora>("editoras");
MapCatalog<Ciencia>("ciencias");
MapCatalog<Idioma>("idiomas");

// ========== END CRUD CATÁLOGOS ==========

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
