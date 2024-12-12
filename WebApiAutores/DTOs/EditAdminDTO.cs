using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs;

public class EditAdminDTO
{
  [Required]
  [EmailAddress]
  public required string Email { get; set; }
}
