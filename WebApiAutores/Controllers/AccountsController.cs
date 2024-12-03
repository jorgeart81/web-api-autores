using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController(UserManager<IdentityUser> userManager, IConfiguration configuration) : ControllerBase
{

  [HttpPost("register")]
  public async Task<ActionResult<AuthenticationResponse>> Register(UserCredentials userCredentials)
  {
    var newUser = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
    var result = await userManager.CreateAsync(newUser, userCredentials.Password);

    if (!result.Succeeded) return BadRequest(result.Errors);

    return BuildToken(userCredentials);
  }

  private AuthenticationResponse BuildToken(UserCredentials userCredentials)
  {
    var jwtKey = configuration["JWTKey"];
    if (jwtKey == null) return new AuthenticationResponse() { };


    var claims = new List<Claim>() {
      new Claim("email", userCredentials.Email)
    };


    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var expiration = DateTime.UtcNow.AddDays(30);

    var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

    return new AuthenticationResponse()
    {
      Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
      Expiration = expiration
    };
  }
}
