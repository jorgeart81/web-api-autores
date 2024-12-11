using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController(UserManager<IdentityUser> userManager,
    IConfiguration configuration,
    SignInManager<IdentityUser> signInManager) : ControllerBase
{

  [HttpPost("register")]
  public async Task<ActionResult<AuthenticationResponse>> Register(UserCredentials userCredentials)
  {
    var newUser = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
    var result = await userManager.CreateAsync(newUser, userCredentials.Password);

    if (result.Succeeded) return BuildToken(userCredentials, 20);

    return BadRequest(result.Errors);
  }

  [HttpPost("login")]
  public async Task<ActionResult<AuthenticationResponse>> Login(UserCredentials userCredentials)
  {
    var result = await signInManager.PasswordSignInAsync(userCredentials.Email,
        userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

    if (result.Succeeded) return BuildToken(userCredentials);

    return BadRequest("Failed to login");
  }

  [HttpGet("refreshToken")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public ActionResult<AuthenticationResponse> RefreshToken()
  {
    var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
    var email = emailClaim?.Value;
    if (string.IsNullOrEmpty(email)) return Unauthorized();

    var userCredentials = new UserCredentials()
    {
      Email = email,
      Password = ""
    };

    return BuildToken(userCredentials);
  }

  private AuthenticationResponse BuildToken(UserCredentials userCredentials, double minutes = 10080)
  {
    var jwtKey = configuration["JWTKey"];
    if (jwtKey == null) return new AuthenticationResponse() { };


    var claims = new List<Claim>() {
      new Claim("email", userCredentials.Email)
    };


    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var expiration = DateTime.UtcNow.AddMinutes(minutes);

    var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

    return new AuthenticationResponse()
    {
      Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
      Expiration = expiration
    };
  }
}
