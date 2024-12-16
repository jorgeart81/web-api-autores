using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WebApiAutores.DTOs;

namespace WebApiAutores.Services;

public class LinksGenerator(IAuthorizationService authorizationService,
  IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor)
{

  private async Task<bool> IsAdmin()
  {
    var httpContext = httpContextAccessor.HttpContext;
    if (httpContext == null) return false;

    var result = await authorizationService.AuthorizeAsync(httpContext.User, "isAdmin");
    return result.Succeeded;
  }

  private IUrlHelper BuildURLHelper()
  {
    var factory = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContextAccessor.ActionContext);
  }

  public async Task GenerateLinks(AuthorDTO authorDTO)
  {
    var isAdmin = await IsAdmin();
    var Url = BuildURLHelper();

    authorDTO.Links.Add(new DataHATEOAS(link: Url.Link("getAuthorById", new { id = authorDTO.Id }), description: "self", method: "GET"));
    authorDTO.Links.Add(new DataHATEOAS(link: Url.Link("getAuthorByName", new { name = authorDTO.Name }), description: "self", method: "GET"));

    if (isAdmin)
    {
      authorDTO.Links.Add(new DataHATEOAS(link: Url.Link("updateAuthor", new { id = authorDTO.Id }), description: "self", method: "PUT"));
      authorDTO.Links.Add(new DataHATEOAS(link: Url.Link("deleteAuthor", new { id = authorDTO.Id }), description: "self", method: "DELETE"));
    }
  }
}
