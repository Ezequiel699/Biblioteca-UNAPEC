using AutoMapper;
using BibliotecaAPEC.DTOs;
using BibliotecaAPEC.Services;
using BibliotecaAPEC.Utils;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PrestamosController : ControllerBase
{
    private readonly IPrestamoService _service;
    private readonly IMapper _mapper;

    public PrestamosController(IPrestamoService service, IMapper mapper) { _service = service; _mapper = mapper; }

    [HttpPost("prestar")]
    public async Task<IActionResult> Prestar(CreatePrestamoDto dto)
    {
        var (ok, message, prestamo) = await _service.PrestarAsync(dto.UsuarioId, dto.LibroId);
        if (!ok) return BadRequest(new { message });
        var outDto = _mapper.Map<PrestamoDto>(prestamo);
        return CreatedAtAction(nameof(GetById), new { id = outDto.Id }, outDto);
    }

    [HttpPost("devolver/{id}")]
    public async Task<IActionResult> Devolver(int id)
    {
        var (ok, message) = await _service.DevolverAsync(id);
        if (!ok) return BadRequest(new { message });
        return Ok(new { message });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _service.GetByIdAsync(id);
        if (p == null) return NotFound();
        return Ok(_mapper.Map<PrestamoDto>(p));
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar([FromQuery] PrestamoQueryParams qp)
    {
        var paged = await _service.BuscarAsync(qp);
        var mapped = paged.Items.Select(p => _mapper.Map<PrestamoDto>(p));
        return Ok(new { items = mapped, total = paged.Total, page = paged.Page, pageSize = paged.PageSize });
    }

    [HttpGet("reporte")]
    public async Task<IActionResult> Reporte([FromQuery] DateTime desde, [FromQuery] DateTime hasta, [FromQuery] string? categoria, [FromQuery] string? idioma)
    {
        var qp = new PrestamoQueryParams { Desde = desde, Hasta = hasta, Categoria = categoria, Idioma = idioma, Page = 1, PageSize = int.MaxValue };
        var paged = await _service.BuscarAsync(qp);
        var totalPrestamos = paged.Total;
        var porCategoria = paged.Items.GroupBy(p => p.Libro.Categoria).Select(g => new { Categoria = g.Key, Count = g.Count() });
        var porIdioma = paged.Items.GroupBy(p => p.Libro.Idioma).Select(g => new { Idioma = g.Key, Count = g.Count() });

        return Ok(new { totalPrestamos, porCategoria, porIdioma });
    }
}
