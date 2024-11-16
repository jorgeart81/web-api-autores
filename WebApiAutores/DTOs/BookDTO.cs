using System;

namespace WebApiAutores.DTOs;

public class BookDTO
{
  public int Id { get; set; }
  public required string Title { get; set; }
  public  List<AuthorDTO>? Authors { get; set; }
  // public List<CommentDTO>? Comments { get; set; }
}
