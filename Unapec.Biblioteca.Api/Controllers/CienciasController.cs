using Unapec.Biblioteca.Api.Controllers;
using Unapec.Biblioteca.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;

namespace Unapec.Biblioteca.Api.Controllers;

public static class CienciasEndpoints
{
    // Definimos los records localmente — como harías inline en Program.cs
    public record CreateCienciaRequest(string Descripcion, bool Estado = true);
    public record UpdateCienciaRequest(string Descripcion, bool Estado);

    public static void MapCienciasEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/ciencias").WithTags("ciencias");

        group.MapGet("", (BibliotecaDbContext db, int page = 1, int pageSize = 20, string? q = null, bool? estado = null) =>
        {
            var query = db.Ciencias.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(c => c.Descripcion.Contains(q));
            if (estado.HasValue)
                query = query.Where(c => c.Estado == estado);

            return Results.Ok(new
            {
                page,
                pageSize,
                total = query.Count(),
                items = query.OrderBy(c => c.Descripcion)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList()
            });
        });

        group.MapGet("{id:int}", async (BibliotecaDbContext db, int id) =>
            await db.Ciencias.FindAsync(id) is { } c ? Results.Ok(c) : Results.NotFound());

        group.MapPost("", async (BibliotecaDbContext db, CreateCienciaRequest request) =>
        {
            var entity = new Ciencia
            {
                Descripcion = request.Descripcion,
                Estado = request.Estado
            };

            db.Ciencias.Add(entity);
            await db.SaveChangesAsync();

            return Results.Created($"/api/ciencias/{entity.Id}", entity);
        });

        group.MapPut("{id:int}", async (BibliotecaDbContext db, int id, UpdateCienciaRequest request) =>
        {
            var entity = await db.Ciencias.FindAsync(id);
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
            var entity = await db.Ciencias.FindAsync(id);
            if (entity == null)
                return Results.NotFound();

            entity.Estado = false;
            entity.ActualizadoEn = DateTime.UtcNow;
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}