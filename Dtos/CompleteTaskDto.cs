using System.ComponentModel.DataAnnotations;

namespace TaskManager.Dtos;

public class CompleteTaskDto
{
    [Required, MaxLength(100)]
    public string FollowUpTitle { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int FollowUpProjectId { get; set; }
}