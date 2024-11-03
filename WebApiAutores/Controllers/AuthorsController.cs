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

  [HttpPost]
  public async Task<ActionResult<Author>> Post(Author author)
  {
    context.Add(author);
    await context.SaveChangesAsync();
    return Ok();
  }
}
