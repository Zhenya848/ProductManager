using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Models.Options;

namespace WebApplication1.Models.User.Token;

public class TokenValidationParametersFactory
{
    public static TokenValidationParameters CreateWithLifeTime(JwtOptions jwtOptions)
    {
        return new TokenValidationParameters
        {
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ClockSkew = TimeSpan.Zero
        };
    }
}