using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using WebApiAutores.DTOs;

namespace WebApiAutores.Services;

public class HashService
{
  public HashResult Hash(string plaintext)
  {
    var salt = new byte[16];
    using (var random = RandomNumberGenerator.Create())
    {
      random.GetBytes(salt);
    };

    return Hash(plaintext, salt);
  }

  public HashResult Hash(string plaintext, byte[] salt)
  {
    var derivedKey = KeyDerivation.Pbkdf2(password: plaintext,
      salt: salt, prf: KeyDerivationPrf.HMACSHA1,
      iterationCount: 10000,
      numBytesRequested: 32);

    var hash = Convert.ToBase64String(derivedKey);

    return new HashResult()
    {
      Hash = hash,
      Salt = salt
    };
  }
}
