using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Entities;

public class Author : IValidatableObject
{
  public int Id { get; set; }

  [Required]
  [StringLength(maximumLength: 120, MinimumLength = 3)]
  public required string Name { get; set; }
  public List<Book>? Books { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    if (!string.IsNullOrEmpty(Name))
    {
      var firstLetter = Name.ToString()[0].ToString();
      if (firstLetter != firstLetter.ToUpper())
      {
        yield return new ValidationResult("The first letter must be capitalized", new string[] { nameof(Name) });
      }
    }

  }
}
