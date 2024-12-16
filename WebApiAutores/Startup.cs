using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApiAutores.Filters;
using WebApiAutores.Middlewares;
using WebApiAutores.Services;
using WebApiAutores.Utilities;
using WebApiAutores.Values;

namespace WebApiAutores;

public class Startup
{
  public Startup(IConfiguration configuration)
  {
    // Clean up claims type mapping
    JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

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
    }).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();

    services.AddDbContext<ApplicationDBContext>(options =>
      options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
    );

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuer = false,
              ValidateAudience = false,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTKey"]!)),
              ClockSkew = TimeSpan.Zero
            });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
      c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
      });

      c.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
            {
              Type = ReferenceType.SecurityScheme,
              Id= "Bearer"
            }
        },
        new String[]{}}
      });
    });
    services.AddAutoMapper(typeof(Startup));

    services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDBContext>()
            .AddDefaultTokenProviders();

    services.AddAuthorization(options =>
    {
      options.AddPolicy(DefaultStrings.IsAdmin, policy => policy.RequireClaim("isAdmin"));
    });

    services.AddDataProtection();
    services.AddTransient<HashService>();

    services.AddCors(options =>
    {
      options.AddDefaultPolicy(builder =>
      {
        builder.WithOrigins("").AllowAnyMethod().AllowAnyHeader();
      });
    });

    services.AddTransient<LinksGenerator>();
    services.AddTransient<HATEOASAuthorFilterAttribute>();
    services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
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

    app.UseCors();

    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}
