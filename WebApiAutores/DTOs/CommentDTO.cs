using System;

namespace WebApiAutores.DTOs;

public class CommentDTO
{
  public int Id { get; set; }
  public required string Content { get; set; }
}
