using System;

namespace WebApiAutores.DTOs;

public class AuthorDTOWithBooks : AuthorDTO
{
  public List<BookDTO>? Books { get; set; }
}
