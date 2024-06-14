using JiraLikeSystem.Core.Models;
using JiraLikeSystem.Models.Entities;
using System.Threading.Tasks;

namespace JiraLikeSystem.Core.Interfaces;

public interface IProjectTaskService
{
    Task <bool> AssigneTaskToUser(AssignTaskModel assignTaskModel);
    Task<ProjectTask> CreateTask(int projectId, CreateTaskModel taskModel);
    Task DeleteTask(int id);
    Task <IEnumerable<ProjectTask>> GetAllTasks();
    Task<ProjectTask> GetTaskById(int id);
    Task <ProjectTask> UpdateTask(int taskId, UpdateTaskModel taskModel);
}
