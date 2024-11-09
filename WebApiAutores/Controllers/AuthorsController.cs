using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorsController(ApplicationDBContext context, IMapper mapper) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<List<AuthorDTO>>> Get()
  {
    var authors = await context.Authors.ToListAsync();
    return Ok(mapper.Map<List<AuthorDTO>>(authors));
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<AuthorDTO>> GetById(int id)
  {
    var author = await context.Authors.FindAsync(id);
    if (author == null) return NotFound();

    return Ok(mapper.Map<AuthorDTO>(author));
  }

  [HttpGet("{name}")]
  public async Task<ActionResult<List<AuthorDTO>>> GetByName(string name)
  {
    var authors = await context.Authors.Where(a => a.Name.Contains(name)).ToListAsync();

    return Ok(mapper.Map<List<AuthorDTO>>(authors));
  }

  [HttpPost]
  public async Task<ActionResult<Author>> Post(CreateAuthorDTO authorDTO)
  {
    var author = mapper.Map<Author>(authorDTO);

    context.Add(author);
    await context.SaveChangesAsync();

    return Created();
  }

  [HttpPut("{id:int}")]
  public async Task<ActionResult<Author>> Put(Author author, int id)
  {
    var existingAuthor = await context.Authors.FindAsync(id);
    if (existingAuthor == null) return NotFound();

    existingAuthor.Name = author.Name;
    context.Entry(existingAuthor).State = EntityState.Modified;
    await context.SaveChangesAsync();

    return Ok();
  }

  [HttpDelete("{id:int}")]
  public async Task<ActionResult> Delete(int id)
  {
    var existingAuthor = await context.Authors.FindAsync(id);
    if (existingAuthor == null) return NotFound();

    context.Remove(existingAuthor);
    await context.SaveChangesAsync();

    return NoContent();
  }
}
