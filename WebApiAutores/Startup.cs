using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Filters;
using WebApiAutores.Middlewares;
using WebApiAutores.Services;

namespace WebApiAutores;

public class Startup
{
  public Startup(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  public IConfiguration Configuration { get; }

  public void ConfigureServices(IServiceCollection services)
  {
    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
    services.AddDbContext<ApplicationDBContext>(options =>
      options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
    );

    // Custom services
    services.AddTransient<IService, ServiceA>();

    services.AddTransient<TransientService>();
    services.AddScoped<ScopedService>();
    services.AddSingleton<SingletonService>();
    services.AddTransient<MyActionFilter>();

    services.AddResponseCaching();
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
  }

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    // Configure the HTTP request pipeline.

    // app.UseMiddleware<LoggerHttpResponseMiddleware>();
    app.UseLoggerHttpResponse();

    if (env.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseResponseCaching();

    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}
