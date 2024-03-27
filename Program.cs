using Microsoft.EntityFrameworkCore;
using IdVaultServer.Data;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
  app.UseExceptionHandler("/error");
}

app.MapGet("/", async (ApplicationDbContext dbContext) => {
    var users = await dbContext.Users.ToListAsync();
    return users;
});

app.Run();
