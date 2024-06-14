using JiraLikeSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace JiraLikeSystem.Core.Models
{
    public class UpdateTaskModel
    {
        [Required]
        public string Title { get; set; }
        public Guid? UserId { get; set; }

        [Required]
        public string Description { get; set; }
        public TaskWorkStatus Status { get; set; } = TaskWorkStatus.Open;
        public TaskPriority Priority { get; set; } = TaskPriority.Low;
        public DateTime DueDate { get; set; }
    }
}
