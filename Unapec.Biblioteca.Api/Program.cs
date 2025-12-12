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



app.Run(); // El endpoint login ya no está aquí

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}