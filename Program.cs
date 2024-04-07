using System.Text;
using IdVaultServer.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key");
var authSymmetricKey = jwtKey != null ? Encoding.UTF8.GetBytes(jwtKey) : null;

string? ConnectionString =
    Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
    builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine("Using connection string: " + ConnectionString);
services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(ConnectionString));

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

using (var scope = app.Services.CreateScope()) {
  var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
  dbContext.Database.Migrate();
}

app.Run();
