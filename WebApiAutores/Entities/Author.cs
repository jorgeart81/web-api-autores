using System;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validations;

namespace WebApiAutores.Entities;

public class Author
{
  public int Id { get; set; }

  [Required]
  [FirstCapitalLetter]
  [StringLength(maximumLength: 120, MinimumLength = 3)]
  public required string Name { get; set; }
}
