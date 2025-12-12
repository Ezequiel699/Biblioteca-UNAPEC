using BibliotecaAPEC.Models;
using BibliotecaAPEC.Repositories;
namespace BibliotecaAPEC.Seed;
public static class DbSeeder
{
    public static async Task SeedBooksAndUsers(
        IGenericRepository<Book> bookRepo,
        IGenericRepository<Usuario> userRepo)
    {
        var books = await bookRepo.GetAllAsync();
        if (books.Any()) return;

        await bookRepo.AddAsync(new Book { Titulo = "Física Básica", Autor = "Halliday", Categoria = "Ciencia", Idioma = "Español" });
        await bookRepo.AddAsync(new Book { Titulo = "Historia Dominicana", Autor = "Varios", Categoria = "Historia", Idioma = "Español" });
        await bookRepo.AddAsync(new Book { Titulo = "Intro a Algoritmos", Autor = "Cormen", Categoria = "Ciencia", Idioma = "Inglés" });

        await userRepo.AddAsync(new Usuario { Nombre = "Juan Perez", Matricula = "A001" });
        await userRepo.AddAsync(new Usuario { Nombre = "Ana López", Matricula = "A002" });
    }
}
