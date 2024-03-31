using IdVaultServer.Data;
using idvault_server.TokenValidator;
using IdVaultServer.Models;

public static class UserRoutes
{
    public static void MapUserRoutes(this IEndpointRouteBuilder endpoints)
    {
        /* endpoints.MapGet( */
        /*     "/", */
        /*     async context => */
        /*     { */
        /*         string? user = JwtTokenValidator.ValidateTokenAndGetCurrentUser( */
        /*             context.Request.Headers */
        /*         ); */
        /*         if (user == null) */
        /*         { */
        /*             context.Response.StatusCode = 401; */
        /*             context.Response.ContentType = "text/json"; */
        /*             await context.Response.WriteAsJsonAsync( */
        /*                 new { error = new { message = "Unauthorized", } } */
        /*             ); */
        /*             return; */
        /*         } */

        /*         await context.Response.WriteAsJsonAsync( */
        /*             new */
        /*             { */
        /*                 user = JwtTokenValidator.ValidateTokenAndGetCurrentUser( */
        /*                     context.Request.Headers */
        /*                 ) */
        /*             } */
        /*         ); */
        /*         return; */
        /*     } */
        /* ); */

        endpoints.MapPost(
            "/users/login",
            async context =>
            {
                var form_data = await context.Request.ReadFormAsync();
                var username = form_data["username"];
                var password = form_data["password"];
                using var dbContext = context.RequestServices.GetService<ApplicationDbContext>();
                var user = dbContext!.Users.FirstOrDefault(u =>
                    u.Username == username.ToString() && u.Password == password.ToString()
                );
                if (user == null)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "Unauthorized", } }
                    );
                    return;
                }

                var tokenString = JwtTokenValidator.GenerateTokenWithUserName(user!.Username);
                context.Response.Headers.Add("Authorization", "Bearer " + tokenString);
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(tokenString);
                return;
            }
        );

        endpoints.MapPost(
            "/users/register",
            async context =>
            {
                var form_data = await context.Request.ReadFormAsync();

                var fullname = form_data["fullname"].FirstOrDefault();
                var email = form_data["email"].FirstOrDefault();
                var phone_number = form_data["phone_number"].FirstOrDefault();
                var username = form_data["username"].FirstOrDefault();
                var password = form_data["password"].FirstOrDefault();

                if (
                    fullname == null
                    || email == null
                    || phone_number == null
                    || username == null
                    || password == null
                )
                {
                    context.Response.StatusCode = 400; 
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "One or more required fields are missing", } }
                    );
                    return;
                }

                using var dbContext = context.RequestServices.GetService<ApplicationDbContext>();
                var user = dbContext!.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "text/json";
                    await context.Response.WriteAsJsonAsync(
                        new { error = new { message = "User already exists. Please try another username", } }
                    );
                    return;
                }

                var new_user = new User
                {
                    Name = fullname,
                    Email = email,
                    PhoneNumber = phone_number,
                    Username = username,
                    Password = password,
                };
                dbContext!.Users.Add(new_user);
                dbContext.SaveChanges();
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/json";
                await context.Response.WriteAsJsonAsync(new_user);
                return;
            }
        );
        endpoints.MapGet("/users/me", async context => { });
    }
}
