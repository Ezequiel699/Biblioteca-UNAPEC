// Controllers/IdiomasEndpoints.cs
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;

namespace Unapec.Biblioteca.Api.Controllers;

public static class IdiomasEndpoints
{
    public record CreateIdiomaRequest(string Descripcion, bool Estado = true);
    public record UpdateIdiomaRequest(string Descripcion, bool Estado);

    public static void MapIdiomasEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/idiomas").WithTags("idiomas");

        group.MapGet("", (BibliotecaDbContext db, int page = 1, int pageSize = 20, string? q = null, bool? estado = null) =>
        {
            var query = db.Idiomas.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(i => i.Descripcion.Contains(q));
            if (estado.HasValue)
                query = query.Where(i => i.Estado == estado);

            return Results.Ok(new
            {
                page,
                pageSize,
                total = query.Count(),
                items = query.OrderBy(i => i.Descripcion)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList()
            });
        });

        group.MapGet("{id:int}", async (BibliotecaDbContext db, int id) =>
            await db.Idiomas.FindAsync(id) is { } i ? Results.Ok(i) : Results.NotFound());

        group.MapPost("", async (BibliotecaDbContext db, CreateIdiomaRequest request) =>
        {
            var entity = new Idioma
            {
                Descripcion = request.Descripcion,
                Estado = request.Estado
            };

            db.Idiomas.Add(entity);
            await db.SaveChangesAsync();

            return Results.Created($"/api/idiomas/{entity.Id}", entity);
        });

        group.MapPut("{id:int}", async (BibliotecaDbContext db, int id, UpdateIdiomaRequest request) =>
        {
            var entity = await db.Idiomas.FindAsync(id);
            if (entity == null)
                return Results.NotFound();

            entity.Descripcion = request.Descripcion;
            entity.Estado = request.Estado;
            entity.ActualizadoEn = DateTime.UtcNow;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("{id:int}", async (BibliotecaDbContext db, int id) =>
        {
            var entity = await db.Idiomas.FindAsync(id);
            if (entity == null)
                return Results.NotFound();

            entity.Estado = false;
            entity.ActualizadoEn = DateTime.UtcNow;
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}