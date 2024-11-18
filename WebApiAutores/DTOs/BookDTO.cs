using System;

namespace WebApiAutores.DTOs;

public class BookDTO
{
  public int Id { get; set; }
  public required string Title { get; set; }
  public DateTime PublicationDate { get; set; }
  // public List<CommentDTO>? Comments { get; set; }
}
