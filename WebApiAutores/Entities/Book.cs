using System;

namespace WebApiAutores.Entities;

public class Book
{
  public int Id { get; set; }
  public required string Title { get; set; }
  public required DateTime? PublicationDate { get; set; }
  public List<Comment>? Comments { get; set; }
  public required List<AuthorBook> AuthorsBooks { get; set; }
}
