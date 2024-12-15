using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entities;
using WebApiAutores.Values;

namespace WebApiAutores.Controllers;

[ApiController]
[Route("api/authors")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = DefaultStrings.IsAdmin)]
public class AuthorsController(ApplicationDBContext context, IMapper mapper,
  IAuthorizationService authorizationService) : ControllerBase
{
  private void GenerateLinks(AuthorDTO authorDTO, bool isAdmin = false)
  {
    authorDTO.Links.Add(new DataHATEOAS(link: Url.Link("getAuthorById", new { id = authorDTO.Id }), description: "self", method: "GET"));
    authorDTO.Links.Add(new DataHATEOAS(link: Url.Link("getAuthorByName", new { name = authorDTO.Name }), description: "self", method: "GET"));

    if (isAdmin)
    {
      authorDTO.Links.Add(new DataHATEOAS(link: Url.Link("updateAuthor", new { id = authorDTO.Id }), description: "self", method: "PUT"));
      authorDTO.Links.Add(new DataHATEOAS(link: Url.Link("deleteAuthor", new { id = authorDTO.Id }), description: "self", method: "DELETE"));
    }
  }

  [HttpGet(Name = "getAuthors")]
  [AllowAnonymous]
  public async Task<ActionResult<ResourceCollection<AuthorDTO>>> Get()
  {
    var authors = await context.Authors.ToListAsync();
    var dtos = mapper.Map<List<AuthorDTO>>(authors);
    var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");

    dtos.ForEach(dto => GenerateLinks(dto, isAdmin.Succeeded));

    var result = new ResourceCollection<AuthorDTO> { Values = dtos };
    result.Links.Add(new DataHATEOAS(link: Url.Link("getAuthors", new { }), description: "self", method: "GET"));

    if (isAdmin.Succeeded)
    {
      result.Links.Add(new DataHATEOAS(link: Url.Link("createAuthor", new { }), description: "create-author", method: "POST"));
    }

    return Ok(result);
  }

  [HttpGet("{id:int}", Name = "getAuthorById")]
  [AllowAnonymous]
  public async Task<ActionResult<AuthorDTOWithBooks>> GetById(int id)
  {
    var author = await context.Authors
        .Include(a => a.AuthorsBooks)
        .ThenInclude(ab => ab.Book)
        .FirstOrDefaultAsync(a => a.Id == id);

    if (author == null) return NotFound();

    var dto = mapper.Map<AuthorDTOWithBooks>(author);
    var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");

    GenerateLinks(dto, isAdmin.Succeeded);
    return Ok(dto);
  }

  [HttpGet("{name}", Name = "getAuthorByName")]
  [AllowAnonymous]
  public async Task<ActionResult<List<AuthorDTO>>> GetByName(string name)
  {
    var authors = await context.Authors.Where(a => a.Name.Contains(name)).ToListAsync();
    var dtos = mapper.Map<List<AuthorDTO>>(authors);
    var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");

    dtos.ForEach(dto => GenerateLinks(dto, isAdmin.Succeeded));

    return Ok(dtos);
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
