using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdVaultServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using idvault_server.TokenValidator;

public static class UserRoutes {
  public static void MapUserRoutes(this IEndpointRouteBuilder endpoints) {
    endpoints.MapGet("/", async context => {
      Console.WriteLine(JwtTokenValidator.ValidateToken(context.Request.Headers)); 
      return;
    });

    
    endpoints.MapGet("/login", async context => {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes("11111111111111111111111111111111"); // get from config
      var tokenDescriptor = new SecurityTokenDescriptor {
        Subject = new ClaimsIdentity(
            new Claim[] { new Claim(ClaimTypes.Name, "cooluser") }),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials =
            new SigningCredentials(new SymmetricSecurityKey(key),
                                   SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenString = tokenHandler.WriteToken(token);
      context.Response.Headers.Add("Authorization", "Bearer " + tokenString);
      context.Response.ContentType = "text/plain";
      await context.Response.WriteAsync(tokenString);
    });
  }
}
