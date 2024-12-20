using System;
using Microsoft.EntityFrameworkCore;

namespace WebApiAutores.Utilities;

public static class HttpContextExtensions
{
  public async static Task InsertPaginationParametersInHeader<T>(this HttpContext httpContext,
    IQueryable<T> queryable)
  {
    if (httpContext == null) throw new ArgumentException(nameof(httpContext));

    double quantity = await queryable.CountAsync();
    httpContext.Response.Headers.Append("total-number-records", quantity.ToString());
  }
}
