using System;

namespace WebApiAutores.Entities;

public class Author
{
  public int Id { get; set; }
  public required string Name { get; set; }
  public required List<AuthorBook> AuthorsBooks { get; set; }
}
