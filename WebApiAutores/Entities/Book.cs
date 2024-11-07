using System;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validations;

namespace WebApiAutores.Entities;

public class Book
{
  public int Id { get; set; }

  [Required]
  [FirstCapitalLetter]
  [StringLength(maximumLength: 120)]
  public required string Title { get; set; }
}
