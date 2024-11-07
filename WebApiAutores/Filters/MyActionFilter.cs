using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Filters;

public class MyActionFilter(ILogger<MyActionFilter> logger) : IActionFilter
{
  public void OnActionExecuting(ActionExecutingContext context)
  {
    logger.LogInformation("Before the action");
  }

  public void OnActionExecuted(ActionExecutedContext context)
  {
    logger.LogInformation("After the action");
  }
}
