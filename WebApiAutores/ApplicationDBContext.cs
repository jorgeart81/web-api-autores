using System;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores;

public class ApplicationDBContext : DbContext
{
  public ApplicationDBContext(DbContextOptions options) : base(options)
  {
  }

  public DbSet<Author> Authors { get; set; }
}
