using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores;

public class ApplicationDBContext : IdentityDbContext
{
  public ApplicationDBContext(DbContextOptions options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<AuthorBook>().HasKey(ab => new { ab.AuthorId, ab.BookId });
  }

  public DbSet<Author> Authors { get; set; }
  public DbSet<Book> Books { get; set; }
  public DbSet<Comment> Comments { get; set; }
  public DbSet<AuthorBook> AuthorBook { get; set; }
}
