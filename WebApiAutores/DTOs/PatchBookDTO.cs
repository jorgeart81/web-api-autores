using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WebApiAutores.Validations;

namespace WebApiAutores.DTOs;

public class PatchBookDTO
{
  [Required]
  [FirstCapitalLetter]
  [StringLength(maximumLength: 255)]
  public required string Title { get; set; }

  public DateTime? PublicationDate { get; set; }
}
