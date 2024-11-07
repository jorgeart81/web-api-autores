using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Filters;

public class ExceptionFilter(ILogger<ExceptionFilter> logger) : ExceptionFilterAttribute
{
  public override void OnException(ExceptionContext context)
  {
    logger.LogError(context.Exception, context.Exception.Message);
    base.OnException(context);
  }
}
