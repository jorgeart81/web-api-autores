using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiAutores.DTOs;
using WebApiAutores.Services;

namespace WebApiAutores.Utilities;

public class HATEOASAuthorFilterAttribute(LinksGenerator linkGenerator) : HATEOASFilterAttribute
{
  public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
  {
    var mustInclude = MustIncludeHATEOAS(context);
    var result = context.Result as ObjectResult;

    if (!mustInclude || result == null)
    {
      await next();
      return;
    }

    if (result.Value is not AuthorDTO authorDTO)
    {
      var authorsDTO = result.Value as List<AuthorDTO> ?? throw new ArgumentException("Expexted instance of AuthorDTO or List<AuthorDTO>");

      authorsDTO.ForEach(async author => await linkGenerator.GenerateLinks(author));
      result.Value = authorsDTO;
    }
    else
    {
      await linkGenerator.GenerateLinks(authorDTO);
    }

    await next();
  }
}
