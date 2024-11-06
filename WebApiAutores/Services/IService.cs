using System;

namespace WebApiAutores.Services;

public interface IService
{
  public Guid GetTransient();
  public Guid GetScoped();
  public Guid GetSingleton();
  void PerformTask();
}

public class ServiceA(ILogger<ServiceA> logger, TransientService transientService,
                      ScopedService scopedService, SingletonService singletonService) : IService
{
  public Guid GetTransient() => transientService.Guid;
  public Guid GetScoped() => scopedService.Guid;
  public Guid GetSingleton() => singletonService.Guid;

  public void PerformTask()
  {
    logger.LogInformation("Service_A perform task");
  }
}

public class ServiceB(ILogger<ServiceB> logger) : IService
{
  public Guid GetScoped()
  {
    throw new NotImplementedException();
  }

  public Guid GetSingleton()
  {
    throw new NotImplementedException();
  }

  public Guid GetTransient()
  {
    throw new NotImplementedException();
  }

  public void PerformTask()
  {
    logger.LogInformation("Service_B perform task");
  }
}

public class TransientService(ILogger<TransientService> logger)
{
  public Guid Guid = Guid.NewGuid();

  public void PerformTask()
  {
    logger.LogInformation("TransientService");
  }
}

public class ScopedService(ILogger<ScopedService> logger)
{
  public Guid Guid = Guid.NewGuid();

  public void PerformTask()
  {
    logger.LogInformation("ScopedService");
  }
}

public class SingletonService(ILogger<SingletonService> logger)
{
  public Guid Guid = Guid.NewGuid();

  public void PerformTask()
  {
    logger.LogInformation("SingletonService");
  }
}
