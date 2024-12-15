using System;

namespace WebApiAutores.DTOs;

public class AuthorDTO : Resource
{
  public int Id { get; set; }
  public required string Name { get; set; }
}
