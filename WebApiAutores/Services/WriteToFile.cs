using System;

namespace WebApiAutores.Services;

public class WriteToFile(IWebHostEnvironment env) : IHostedService
{
  private readonly string fileName = "file_1.txt";
  private Timer? timer;

  public Task StartAsync(CancellationToken cancellationToken)
  {
    Write($"{DateTime.Now} 🚀 Process start");
    timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    return Task.CompletedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    timer?.Dispose();
    Write($"{DateTime.Now} 🛑 Process finished");
    return Task.CompletedTask;

  }

  private void DoWork(object? state)
  {
    Write($"{DateTime.Now} 🟢 Process in progress");
  }

  private void Write(string message)
  {
    // var root = $@"{env.ContentRootPath}/wwwroot";
    var path = "wwwroot";
    var route = $@"{path}/{fileName}";

    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

    using (StreamWriter writer = new StreamWriter(route, append: true))
    {
      writer.WriteLine(message);
    }
  }
}
