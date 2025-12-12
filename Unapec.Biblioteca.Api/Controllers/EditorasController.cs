// Controllers/EditorasEndpoints.cs
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;

namespace Unapec.Biblioteca.Api.Controllers;

public static class EditorasEndpoints
{
    public record CreateEditoraRequest(string Descripcion, bool Estado = true);
    public record UpdateEditoraRequest(string Descripcion, bool Estado);

    public static void MapEditorasEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/editoras").WithTags("editoras");

        group.MapGet("", (BibliotecaDbContext db, int page = 1, int pageSize = 20, string? q = null, bool? estado = null) =>
        {
            var query = db.Editoras.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(e => e.Descripcion.Contains(q));
            if (estado.HasValue)
                query = query.Where(e => e.Estado == estado);

            return Results.Ok(new
            {
                page,
                pageSize,
                total = query.Count(),
                items = query.OrderBy(e => e.Descripcion)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList()
            });
        });

        group.MapGet("{id:int}", async (BibliotecaDbContext db, int id) =>
            await db.Editoras.FindAsync(id) is { } e ? Results.Ok(e) : Results.NotFound());

        group.MapPost("", async (BibliotecaDbContext db, CreateEditoraRequest request) =>
        {
            var entity = new Editora
            {
                Descripcion = request.Descripcion,
                Estado = request.Estado
            };

            db.Editoras.Add(entity);
            await db.SaveChangesAsync();

            return Results.Created($"/api/editoras/{entity.Id}", entity);
        });

        group.MapPut("{id:int}", async (BibliotecaDbContext db, int id, UpdateEditoraRequest request) =>
        {
            var entity = await db.Editoras.FindAsync(id);
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
            var entity = await db.Editoras.FindAsync(id);
            if (entity == null)
                return Results.NotFound();

            entity.Estado = false;
            entity.ActualizadoEn = DateTime.UtcNow;
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}