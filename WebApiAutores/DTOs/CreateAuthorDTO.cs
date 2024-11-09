using System;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validations;

namespace WebApiAutores.DTOs;

public class CreateAuthorDTO
{
  [Required]
  [FirstCapitalLetter]
  [StringLength(maximumLength: 120, MinimumLength = 3)]
  public required string Name { get; set; }
}
