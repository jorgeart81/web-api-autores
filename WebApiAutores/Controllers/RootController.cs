using System;
using Microsoft.AspNetCore.Mvc;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers;

[ApiController]
[Route("api")]
public class RootController : ControllerBase
{
  [HttpGet(Name = "GetRoot")]
  public ActionResult<IEnumerable<DataHATEOAS>> Get()
  {
    var dataHateoas = new List<DataHATEOAS>
    {
      new(link: Url.Link("GetRoot", new { }), description: "self", method: "GET")
    };

    return dataHateoas;
  }
}
