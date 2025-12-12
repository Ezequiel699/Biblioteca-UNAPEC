using AutoMapper;
using BibliotecaAPEC.DTOs;
using BibliotecaAPEC.Models;
using BibliotecaAPEC.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IGenericRepository<Usuario> _repo;
    private readonly IMapper _mapper;

    public UsuariosController(IGenericRepository<Usuario> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _repo.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<UsuarioDto>>(list));
    }

    [HttpGet("{id}", Name = "GetUsuario")]
    public async Task<IActionResult> Get(int id)
    {
        var e = await _repo.GetByIdAsync(id);
        if (e == null) return NotFound();
        return Ok(_mapper.Map<UsuarioDto>(e));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUsuarioDto dto)
    {
        var entity = _mapper.Map<Usuario>(dto);
        var created = await _repo.AddAsync(entity);
        var outDto = _mapper.Map<UsuarioDto>(created);
        return CreatedAtRoute("GetUsuario", new { id = outDto.Id }, outDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateUsuarioDto dto)
    {
        var e = await _repo.GetByIdAsync(id);
        if (e == null) return NotFound();
        _mapper.Map(dto, e);
        await _repo.UpdateAsync(e);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _repo.GetByIdAsync(id);
        if (e == null) return NotFound();
        await _repo.RemoveAsync(id);
        return NoContent();
    }
}
