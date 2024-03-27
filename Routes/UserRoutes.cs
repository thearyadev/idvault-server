using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using idvault_server.TokenValidator;
using Microsoft.IdentityModel.Tokens;

public static class UserRoutes
{
    public static void MapUserRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            "/",
            async context =>
            {
                Console.WriteLine(
                    JwtTokenValidator.ValidateTokenAndGetCurrentUser(context.Request.Headers)
                );
                return;
            }
        );

        endpoints.MapPost(
            "/api/users/login",
            async context =>
            {
                // parse formdata keys for email and password 
                var tokenString = JwtTokenValidator.GenerateTokenWithUserName("default");
                context.Response.Headers.Add("Authorization", "Bearer " + tokenString);
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(tokenString);
            }
        );
    }
}
