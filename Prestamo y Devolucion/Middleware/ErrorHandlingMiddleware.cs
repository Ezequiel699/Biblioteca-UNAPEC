using System.Text.Json;
namespace BibliotecaAPEC.Middleware;
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next) { _next = next; }

    public async Task Invoke(HttpContext ctx)
    {
        try { await _next(ctx); }
        catch (Exception ex)
        {
            ctx.Response.StatusCode = 500;
            ctx.Response.ContentType = "application/json";
            var payload = new { message = "Error interno del servidor", detail = ex.Message };
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}
