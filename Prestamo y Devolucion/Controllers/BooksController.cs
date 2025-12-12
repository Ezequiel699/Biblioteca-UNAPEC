using AutoMapper;
using BibliotecaAPEC.DTOs;
using BibliotecaAPEC.Models;
using BibliotecaAPEC.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IGenericRepository<Book> _repo;
    private readonly IMapper _mapper;

    public BooksController(IGenericRepository<Book> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _repo.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<BookDto>>(list));
    }

    [HttpGet("{id}", Name = "GetBook")]
    public async Task<IActionResult> Get(int id)
    {
        var e = await _repo.GetByIdAsync(id);
        if (e == null) return NotFound();
        return Ok(_mapper.Map<BookDto>(e));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookDto dto)
    {
        var entity = _mapper.Map<Book>(dto);
        var created = await _repo.AddAsync(entity);
        var outDto = _mapper.Map<BookDto>(created);
        return CreatedAtRoute("GetBook", new { id = outDto.Id }, outDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateBookDto dto)
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
