using System.ComponentModel.DataAnnotations;

namespace TaskManager.Dtos;

public class CreateProjectDto
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(300)]
    public string? Description { get; set; }
}