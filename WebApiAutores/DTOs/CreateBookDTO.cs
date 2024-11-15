using System;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validations;

namespace WebApiAutores.DTOs;

public class CreateBookDTO
{
  [Required]
  [FirstCapitalLetter]
  [StringLength(maximumLength: 255)]
  public required string Title { get; set; }

  [Required]
  public required List<int> AuthorsId { get; set; }
}
