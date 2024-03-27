using System.Text;
using IdVaultServer.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key");
var authSymmetricKey = jwtKey != null ? Encoding.UTF8.GetBytes(jwtKey) : null;

services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

services
    .AddAuthentication(options => {
      options.DefaultAuthenticateScheme =
          JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => {
      options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = false, ValidateAudience = false,
        ValidateLifetime = true, ValidateIssuerSigningKey = true,
        IssuerSigningKey = authSymmetricKey != null
                               ? new SymmetricSecurityKey(authSymmetricKey)
                               : null
      };
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
  app.UseExceptionHandler("/error");
}
app.UseAuthentication();
app.MapUserRoutes();
app.MapDocumentRoutes();

app.Run();
