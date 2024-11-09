using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Filters;
using WebApiAutores.Middlewares;

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
    services.AddControllers(options =>
    {
      // Global filter
      options.Filters.Add(typeof(ExceptionFilter));
    }).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

    services.AddDbContext<ApplicationDBContext>(options =>
      options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
    );

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddAutoMapper(typeof(Startup));
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

    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}
