using System;
using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validations;

namespace WebApiAutores.Entities;

public class Author
{
  public int Id { get; set; }

  [Required]
  [FirstCapitalLetter]
  public required string Name { get; set; }
  public List<Book>? Books { get; set; }
}
