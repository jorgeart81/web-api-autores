using System;

namespace WebApiAutores.Entities;

public class Book
{
  public int Id { get; set; }
  public required string Title { get; set; }
}
