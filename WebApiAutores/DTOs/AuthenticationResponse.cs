using System;

namespace WebApiAutores.DTOs;

public class AuthenticationResponse
{
  public string? Token { get; set; }
  public DateTime Expiration { get; set; }
}
