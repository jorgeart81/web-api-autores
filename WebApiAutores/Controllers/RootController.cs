using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers;

[ApiController]
[Route("api")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RootController(IAuthorizationService authorizationService) : ControllerBase
{
  [HttpGet(Name = "GetRoot")]
  [AllowAnonymous]
  public async Task<ActionResult<IEnumerable<DataHATEOAS>>> Get()
  {
    var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");

    var dataHateoas = new List<DataHATEOAS>
    {
      new(link: Url.Link("GetRoot", new { }), description: "self", method: "GET"),
      new(link: Url.Link("getAuthors", new { }), description: "authors", method: "GET"),
    };

    if (isAdmin.Succeeded)
    {
      dataHateoas.Add(new DataHATEOAS(link: Url.Link("createAuthor", new { }), description: "create-author", method: "POST"));
      dataHateoas.Add(new DataHATEOAS(link: Url.Link("createBook", new { }), description: "create-book", method: "POST"));
    }

    return dataHateoas;
  }
}
