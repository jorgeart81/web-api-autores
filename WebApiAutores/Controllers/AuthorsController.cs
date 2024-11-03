using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorsController(ApplicationDBContext context) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<List<Author>>> Get()
  {
    return await context.Authors.ToListAsync();
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<List<Author>>> GetById(int id)
  {
    var author = await context.Authors.FindAsync(id);

    if (author == null) return NotFound();

    return Ok(author);
  }

  [HttpPost]
  public async Task<ActionResult<Author>> Post(Author author)
  {
    context.Add(author);
    await context.SaveChangesAsync();
    return Ok();
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
}
