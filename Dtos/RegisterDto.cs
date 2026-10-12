using System.ComponentModel.DataAnnotations;

namespace TaskManager.Dtos;

public class RegisterDto
{
    [Required, MinLength(3), MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    [Required, MinLength(5), MaxLength(100)]
    public string Password { get; set;} = string.Empty;
}