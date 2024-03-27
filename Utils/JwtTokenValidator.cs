using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace idvault_server.TokenValidator
{
    public class JwtTokenValidator
    {
        public static string? ValidateToken(IHeaderDictionary headers)
        {
            if (!headers.ContainsKey("Authorization"))
            {
                return null;
            }
            var token = headers["Authorization"].ToString();

            if (token == null || !token.StartsWith("Bearer "))
                return null;

            token = token.Replace("Bearer ", "");

            var key = Encoding.ASCII.GetBytes("11111111111111111111111111111111"); // load from config
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    },
                    out SecurityToken validatedToken
                );

                var unique_name_claim = tokenHandler.ReadJwtToken(token).Claims.ElementAt(0);
                // get and return username
                return unique_name_claim.Value;
            }
            catch (Exception) // exception on failed validation
            {
                return null;
            }
        }
    }
}
