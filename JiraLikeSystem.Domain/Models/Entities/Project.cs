using System.ComponentModel.DataAnnotations;

namespace JiraLikeSystem.Models.Entities;

public class Project
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public ICollection<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
}
