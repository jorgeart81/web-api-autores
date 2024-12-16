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

    if (!mustInclude)
    {
      await next();
      return;
    }

    var result = context.Result as ObjectResult;
    var model = result?.Value as AuthorDTO ?? throw new ArgumentNullException("An AuthorDTO instance was expected");

    await linkGenerator.GenerateLinks(model);
    await next();
  }
}
