using JiraLikeSystem.Core.Interfaces;
using JiraLikeSystem.Core.Models;
using JiraLikeSystem.Data;
using JiraLikeSystem.Models.Entities;

using Microsoft.EntityFrameworkCore;

namespace JiraLikeSystem.Core.Services;

public class ProjectTaskService : IProjectTaskService
{
    private readonly JiraLikeDbContext _jiraLikeDbContext;

    public ProjectTaskService(JiraLikeDbContext jiraLikeDbContext)
    {
        _jiraLikeDbContext = jiraLikeDbContext;
    }

    public async Task<IEnumerable<ProjectTask>> GetAllTasks() 
    {
        var tasks = await _jiraLikeDbContext.Tasks.ToListAsync();
        
        if (tasks == null)
        {
            throw new Exception("No existed tasks");
        }
        return tasks;
    }

    public async Task<ProjectTask> CreateTask(int projectId, CreateTaskModel task) 
    {
 
        var existingProject = await _jiraLikeDbContext.Projects
        .Include(p => p.ProjectTasks)
        .FirstOrDefaultAsync(p => p.Id == projectId)
        ?? throw new ArgumentException("The specified project doesn't exist.");

        if (task.UserId.HasValue)
        {
            var existingUser = await _jiraLikeDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == task.UserId)
                ?? throw new ArgumentException("User doesn't exist.");
        }

        if (existingProject.ProjectTasks.Any(pt => pt.Title == task.Title))
        {
            throw new ArgumentException("Task with the given name already exists.");
        }

        var newTask = new ProjectTask()
        {
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            ProjectId = projectId,
            UserId = task.UserId,
        };

        existingProject.ProjectTasks.Add(newTask);
        await _jiraLikeDbContext.Tasks.AddAsync(newTask);
        await _jiraLikeDbContext.SaveChangesAsync();
        return newTask;
    }


    public async Task<bool> AssigneTaskToUser(AssignTaskModel assignTaskModel)
    {

        var existingProjectTask = await _jiraLikeDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == assignTaskModel.TaskId);

        if (existingProjectTask == null)
        {
            throw new ArgumentException("The specified project task doesn't exist.");

        }

        var existingUser = await _jiraLikeDbContext.Users.FirstOrDefaultAsync(u => u.Id == assignTaskModel.UserId);

        if (existingUser == null)
        {
            throw new ArgumentException("The specified user doesn't exist.");
        }

        existingProjectTask.UserId = assignTaskModel.UserId;
        _jiraLikeDbContext.Update(existingProjectTask);
        await _jiraLikeDbContext.SaveChangesAsync();

        return true;
    }


    public async Task <ProjectTask> UpdateTask(int taskId, UpdateTaskModel taskModel)
    {
        var existingTask = await GetTaskById(taskId);

        if (existingTask == null)
        {
            throw new ArgumentException("The specified task doesn't exist.");
        }

        existingTask.Title = taskModel.Title;
        existingTask.Description = taskModel.Description;
        existingTask.Status = taskModel.Status;
        existingTask.Priority = taskModel.Priority;
        existingTask.DueDate = taskModel.DueDate;

        if (taskModel.UserId.HasValue)
        {
            existingTask.UserId = taskModel.UserId;
        }

        _jiraLikeDbContext.Tasks.Update(existingTask);
        await _jiraLikeDbContext.SaveChangesAsync();
        return existingTask;
    }

    public async Task <ProjectTask> GetTaskById(int id)
    {
        var task = await _jiraLikeDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id) ?? throw new ArgumentException("Task with the given id doesn't exist.");
        return task;
    }

    public async Task DeleteTask(int id)
    {
        var task = await _jiraLikeDbContext.Tasks.FirstOrDefaultAsync(p => p.Id == id) ?? throw new ArgumentException("Task with the given id doesn't exist.");

        if (task != null)
        {
            _jiraLikeDbContext.Tasks.Remove(task);
            await _jiraLikeDbContext.SaveChangesAsync();
        }
    }
}
