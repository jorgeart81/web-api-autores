using System;
using Microsoft.AspNetCore.Mvc;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorsController : ControllerBase
{
  [HttpGet]
  public ActionResult<List<Author>> Get()
  {
    return Ok(new List<Author> {
      new() { Id = 1, Name= "Jorge" },
      new() { Id = 2, Name= "Andrés" },
    });
  }
}
