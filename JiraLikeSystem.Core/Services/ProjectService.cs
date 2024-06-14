
using JiraLikeSystem.Core.Interfaces;
using JiraLikeSystem.Core.Models;
using JiraLikeSystem.Data;
using JiraLikeSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JiraLikeSystem.Core.Services;

public class ProjectService : IProjectService
{
    private readonly JiraLikeDbContext _jiraLikeDbContext;

    public ProjectService(JiraLikeDbContext JiraLikeDbContext)
    {
        _jiraLikeDbContext = JiraLikeDbContext;
    }

    public async Task<IEnumerable<Project>> GetProjects()
    {

        return await _jiraLikeDbContext.Projects.ToListAsync();
    }

    public async Task<Project> GetProjectById(int id)
    {
        var project = await _jiraLikeDbContext.Projects.FirstOrDefaultAsync(p => p.Id == id) ?? throw new ArgumentException("Project with the given id doesn't exist."); ;
        return project;
    }

    public async Task<IEnumerable<ProjectTask>> GetProjectTasks(int projectId)
    {
  

        var projectTasks = await _jiraLikeDbContext.Tasks
                                      .Where(t => t.ProjectId == projectId)
                                      .ToListAsync();
        if (projectTasks != null)
        {
            return projectTasks;
        }
        else
        {
            return Enumerable.Empty<ProjectTask>();
        }
    }

    public async Task<Project> CreateProject(ProjectModel project)
    {
               var existingProject = await _jiraLikeDbContext.Projects.FirstOrDefaultAsync(p => p.Title == project.Title);

        if (existingProject != null)
        {
            throw new Exception("Project with the given Title already exists.");
        }

        var newProject = new Project()
        {
            Title = project.Title
        };

        _jiraLikeDbContext.Projects.Add(newProject);
        await _jiraLikeDbContext.SaveChangesAsync(); 

        return newProject;
    }

    public async Task<Project> UpdateProject(int projectId, ProjectModel project)
    {
        var existingProject =  await GetProjectById(projectId);

        if (existingProject == null)
        {
            throw new Exception("Project with the given id already exist.");
        }

        existingProject.Title = project.Title;

        _jiraLikeDbContext.Projects.Update(existingProject);
        _jiraLikeDbContext.SaveChangesAsync();
        return existingProject;
    }

    public async Task DeleteProject(int id)
    {
        var project = await GetProjectById(id);

        if (project != null)
        {
            _jiraLikeDbContext.Projects.Remove(project);
            await _jiraLikeDbContext.SaveChangesAsync();
        }
        else
        {
            throw new Exception("Project with the given id does not exist.");
        }
    }
}

