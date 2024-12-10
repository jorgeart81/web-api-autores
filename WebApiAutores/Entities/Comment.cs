using System;
using Microsoft.AspNetCore.Identity;

namespace WebApiAutores.Entities;

public class Comment
{
  public int Id { get; set; }
  public required string Content { get; set; }
  public int BookId { get; set; }
  public required Book Book { get; set; }
  public required string UserId { get; set; }
  public required IdentityUser User { get; set; }
}
