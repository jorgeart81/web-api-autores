using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs;

public class UserCredentials
{
  [Required]
  [EmailAddress]
  public required string Email { get; set; }

  [Required]
  public required string Password { get; set; }
}
