using System;

namespace WebApiAutores.Entities;

public class Comment
{
  public int Id { get; set; }
  public required string Content { get; set; }
  public int BookId { get; set; }
  public required Book Book { get; set; }
}
