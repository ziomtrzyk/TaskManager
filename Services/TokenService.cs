using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using 	Microsoft.IdentityModel.JsonWebTokens;
using TaskManager.Models;
namespace TaskManager.Services;

public class TokenService(IConfiguration config): ITokenService
{
    public string Generate(User user)
    {
        var key = config["Jwt:Key"]
            ?? throw new InvalidOperationException("Brak Jwt:Key, ustaw go przez dotnet user-secrets set");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = config["Jwt:Issuer"]
                ?? throw new InvalidOperationException("Jwt:Issuer jest null"),
            Audience = config["Jwt:Audience"]
                ?? throw new InvalidOperationException("Jwt:Audience jest null"),
            Expires = DateTime.UtcNow.AddMinutes(
                config.GetValue<int?>("Jwt:ExpiresInMinutes")
                ?? throw new InvalidOperationException("Jwt:ExpiresInMinutes jest null")
            ),
            SigningCredentials = credentials
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }
}