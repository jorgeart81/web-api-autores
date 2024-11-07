using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;
using WebApiAutores.Filters;
using WebApiAutores.Services;

namespace WebApiAutores.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorsController(ApplicationDBContext context, IService service, TransientService transientService,
                              ScopedService scopedService, SingletonService singletonService) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<List<Author>>> Get()
  {
    return await context.Authors.Include(a => a.Books).ToListAsync();
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

  [HttpDelete("{id:int}")]
  public async Task<ActionResult> Delete(int id)
  {
    var existingAuthor = await context.Authors.FindAsync(id);
    if (existingAuthor == null) return NotFound();

    context.Remove(existingAuthor);
    await context.SaveChangesAsync();

    return NoContent();
  }

  [HttpGet("GUID")]
  // [ResponseCache(Duration = 10)]
  [ServiceFilter(typeof(MyActionFilter))]
  // [Authorize]
  public ActionResult GetGuids()
  {
    service.PerformTask();
    return Ok(new
    {
      Transient = new
      {
        AuthorsController = transientService.Guid,
        ServiceA = service.GetTransient(),
        IsEqual = transientService.Guid == service.GetTransient(),
        Note = "Transient is always different"
      },
      // Scoped 
      Scoped = new
      {
        AuthorsController = scopedService.Guid,
        ServiceA = service.GetScoped(),
        IsEqual = scopedService.Guid == service.GetScoped(),
        Note = "Scoped is the same within the same http context and different in a different context"
      },
      Singleton = new
      {
        AuthorsController = singletonService.Guid,
        ServiceA = service.GetSingleton(),
        IsEqual = singletonService.Guid == service.GetSingleton(),
        Note = "Singleton is always the same"
      },
    });
  }
}
