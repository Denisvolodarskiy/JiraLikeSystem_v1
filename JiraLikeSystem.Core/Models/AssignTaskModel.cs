using System.ComponentModel.DataAnnotations;

namespace JiraLikeSystem.Core.Models
{
    public class AssignTaskModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int TaskId { get; set; }
    }
}
