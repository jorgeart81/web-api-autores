using System;

namespace WebApiAutores.DTOs;

public class AuthorDTO
{
  public int Id { get; set; }
  public required string Name { get; set; }
}
