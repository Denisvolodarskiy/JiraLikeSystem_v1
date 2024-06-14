
using JiraLikeSystem.Core.Models;
using JiraLikeSystem.Models.Entities;


namespace JiraLikeSystem.Core.Interfaces;

public interface IProjectService
{
    Task<Project> CreateProject(ProjectModel project);
    Task DeleteProject(int id);
    Task <Project> GetProjectById(int id);

    Task<IEnumerable<Project>> GetProjects();
    Task <IEnumerable<ProjectTask>> GetProjectTasks(int projectId);
    Task <Project> UpdateProject(int projectId, ProjectModel project);
}