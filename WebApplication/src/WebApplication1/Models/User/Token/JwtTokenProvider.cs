using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Application;
using WebApplication1.Data;
using WebApplication1.Models.Options;
using WebApplication1.Models.Shared;

namespace WebApplication1.Models.User.Token;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly AppDbContext _accountsDbContext;

    public JwtTokenProvider(
        IOptions<JwtOptions> jwtOptions,
        AppDbContext accountsDbContext)
    {
        _jwtOptions = jwtOptions.Value;
        _accountsDbContext = accountsDbContext;
    }
    
    public string GenerateAccessToken(User user)
    {
        var jti = Guid.NewGuid();
        
        var claims = new[]
        {
            new Claim(CustomClaims.Sub, user.Id.ToString()),
            new Claim(CustomClaims.Jti, jti.ToString()),
            new Claim(CustomClaims.Email, user.Email ?? "")
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiredMinutesTime),
            claims: claims,
            signingCredentials: creds
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }
}