using System;

namespace WebApiAutores.DTOs;

public class HashResult
{
  public string? Hash { get; set; }
  public byte[]? Salt { get; set; }
}
