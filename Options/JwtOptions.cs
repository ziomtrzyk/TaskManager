using System.ComponentModel.DataAnnotations;

namespace TaskManager.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required(ErrorMessage = "Jwt:Issuer is required.")]
    public string Issuer { get; set; } = string.Empty;

    [Required(ErrorMessage = "Jwt:Audience is required.")]
    public string Audience { get; set; } = string.Empty;

    [Required, MinLength(32, ErrorMessage = "Jwt:Key must be at least 32 characters (256 bits for HS256).")]
    public string Key { get; set; } = string.Empty;

    [Required(ErrorMessage = "Jwt:ExpiresInMinutes is required."), Range(1, 1440)]
    public int ExpiresInMinutes { get; set; }

}