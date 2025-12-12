using AutoMapper;
using BibliotecaAPEC.Mapping;
using BibliotecaAPEC.Middleware;
using BibliotecaAPEC.Models;
using BibliotecaAPEC.Repositories;
using BibliotecaAPEC.Seed;
using BibliotecaAPEC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Repositorios en memoria: registrar con lambdas para Id get/set
builder.Services.AddSingleton<IGenericRepository<Book>>(sp =>
    new InMemoryGenericRepository<Book>(b => b.Id, (b, id) => b.Id = id));
builder.Services.AddSingleton<IGenericRepository<Usuario>>(sp =>
    new InMemoryGenericRepository<Usuario>(u => u.Id, (u, id) => u.Id = id));
builder.Services.AddSingleton<IPrestamoRepository, InMemoryPrestamoRepository>();

// Servicios
builder.Services.AddScoped<IPrestamoService, PrestamoService>();

// CORS
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowFrontend", p => p.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));
});

var app = builder.Build();

// Middleware
app.UseCors("AllowFrontend");
app.UseMiddleware<ErrorHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();

// Seed inicial (sin DB): ejecuta after build
using (var scope = app.Services.CreateScope())
{
    var bookRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Book>>();
    var userRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Usuario>>();
    await DbSeeder.SeedBooksAndUsers(bookRepo, userRepo);
}

app.Run();
