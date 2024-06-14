using JiraLikeSystem.Models.Enums;
using JiraLikeSystem.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace JiraLikeSystem.Models.Entities;

public class ProjectTask
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string? Description { get; set; }
    public TaskWorkStatus Status { get; set; } = TaskWorkStatus.Open;
    public TaskPriority Priority { get; set; } = TaskPriority.Low;
    public  DateTime DueDate { get; set; }


    [Required]
    public int ProjectId { get; set; }

    [Required]
    public Project Project { get; set; }
    public Guid? UserId { get; set; }
    public ApplicationUser AssignedTo { get; set; }
}