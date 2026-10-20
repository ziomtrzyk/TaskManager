using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using 	Microsoft.IdentityModel.JsonWebTokens;
using TaskManager.Models;
using Microsoft.Extensions.Options;
using TaskManager.Options;
namespace TaskManager.Services;

public class TokenService(IOptions<JwtOptions> options): ITokenService
{
    public string Generate(User user)
    {
        var jwt = options.Value;

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = jwt.Issuer,
            Audience = jwt.Audience,
            Expires = DateTime.UtcNow.AddMinutes(jwt.ExpiresInMinutes),
            SigningCredentials = credentials
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }
}