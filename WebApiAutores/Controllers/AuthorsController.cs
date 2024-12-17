using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;
using WebApiAutores.Utilities;
using WebApiAutores.Values;

namespace WebApiAutores.Controllers;

[ApiController]
[Route("api/authors")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = DefaultStrings.IsAdmin)]
public class AuthorsController(ApplicationDBContext context, IMapper mapper) : ControllerBase
{
  [HttpGet(Name = "getAuthors")]
  [AllowAnonymous]
  [ServiceFilter(typeof(HATEOASAuthorFilterAttribute))]
  public async Task<ActionResult<List<AuthorDTO>>> Get([FromHeader] string? includeHATEOAS)
  {
    var authors = await context.Authors.ToListAsync();
    return Ok(mapper.Map<List<AuthorDTO>>(authors));
  }

  [HttpGet("{id:int}", Name = "getAuthorById")]
  [AllowAnonymous]
  [ServiceFilter(typeof(HATEOASAuthorFilterAttribute))]
  public async Task<ActionResult<AuthorDTOWithBooks>> GetById(int id, [FromHeader] string? includeHATEOAS)
  {
    var author = await context.Authors
        .Include(a => a.AuthorsBooks)
        .ThenInclude(ab => ab.Book)
        .FirstOrDefaultAsync(a => a.Id == id);

    if (author == null) return NotFound();

    var dto = mapper.Map<AuthorDTOWithBooks>(author);
    return Ok(dto);
  }

  [HttpGet("{name}", Name = "getAuthorByName")]
  [AllowAnonymous]
  [ServiceFilter(typeof(HATEOASAuthorFilterAttribute))]
  public async Task<ActionResult<List<AuthorDTO>>> GetByName(string name, [FromHeader] string? includeHATEOAS)
  {
    var authors = await context.Authors.Where(a => a.Name.Contains(name)).ToListAsync();
    return Ok(mapper.Map<List<AuthorDTO>>(authors));
  }

  [HttpPost(Name = "createAuthor")]
  public async Task<ActionResult<AuthorDTO>> Post(CreateAuthorDTO createAuthorDTO)
  {
    var author = mapper.Map<Author>(createAuthorDTO);

    context.Add(author);
    await context.SaveChangesAsync();

    var authorDTO = mapper.Map<AuthorDTO>(author);

    return CreatedAtRoute("getAuthorById", new { id = author.Id }, authorDTO);
  }

  [HttpPut("{id:int}", Name = "updateAuthor")]
  public async Task<ActionResult> Put(CreateAuthorDTO createAuthorDTO, int id)
  {
    var authorExists = await context.Authors.AnyAsync(a => a.Id == id);
    if (!authorExists) return NotFound();

    var author = mapper.Map<Author>(createAuthorDTO);
    author.Id = id;

    context.Update(author);
    await context.SaveChangesAsync();

    return NoContent();
  }

  [HttpDelete("{id:int}", Name = "deleteAuthor")]
  public async Task<ActionResult> Delete(int id)
  {
    var author = await context.Authors.FirstOrDefaultAsync(a => a.Id == id);
    if (author == null) return NotFound();

    context.Remove(author);
    await context.SaveChangesAsync();

    return NoContent();
  }
}
