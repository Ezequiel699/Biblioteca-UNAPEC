using AutoMapper;
using BibliotecaAPEC.Models;
using BibliotecaAPEC.DTOs;
namespace BibliotecaAPEC.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDto>().ReverseMap();
        CreateMap<CreateBookDto, Book>();
        CreateMap<Usuario, UsuarioDto>().ReverseMap();
        CreateMap<CreateUsuarioDto, Usuario>();
        CreateMap<Prestamo, PrestamoDto>()
            .ForMember(d => d.Usuario, opt => opt.MapFrom(s => s.Usuario))
            .ForMember(d => d.Libro, opt => opt.MapFrom(s => s.Libro));
        CreateMap<CreatePrestamoDto, Prestamo>();
    }
}
