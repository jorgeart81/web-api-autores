using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs;

public class CreateCommentDTO
{
  [Required]
  public required string Content { get; set; }
}
