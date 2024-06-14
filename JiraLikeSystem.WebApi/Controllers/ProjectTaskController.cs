using JiraLikeSystem.Core.Interfaces;
using JiraLikeSystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JiraLikeSystem.WebApi.Controllers;

[Authorize(Roles = "Admin,Project Manager,Developer,Tester")]
[Route("[controller]")]
[ApiController]
public class ProjectTaskController : ControllerBase
{
    private readonly IProjectTaskService _projectTaskService;
    

    public ProjectTaskController(IProjectTaskService projectTaskService)
    {
        _projectTaskService = projectTaskService;
    }

    //GET 
    [HttpGet("GetAllTasks")]
    public async Task<IActionResult> GetAllTasks()
    {

        try
        {
            return Ok(await _projectTaskService.GetAllTasks());
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetTaskBy{id}")]
    public async Task<IActionResult> GetTasks([FromRoute] int id)
    {

        try
        {
            var task = await _projectTaskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //POST
    [HttpPost("CreateTask{projectId}")]
    public async Task<IActionResult> CreateTask([FromRoute] int projectId, [FromBody] CreateTaskModel taskModel)
    {

        if (taskModel == null)
        {
            return BadRequest("Task model is null.");
        }

        try
        {
            var createdTask = await _projectTaskService.CreateTask(projectId, taskModel);
            return Ok(createdTask);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("AssigneTaskToUser")]
    public async Task<IActionResult> AssigneTaskToUser([FromBody] AssignTaskModel model)
    {
        try
        {
            var usignedTask = await _projectTaskService.AssigneTaskToUser(model);
            return Ok(usignedTask);
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }
    }

    //PUT 
    [HttpPut("UpdateTaskBy{id}")]
    public async Task<IActionResult> UpdateTask([FromRoute]int Id, [FromBody]UpdateTaskModel taskModel)
    {
        try
        {
            var updatedTask = await _projectTaskService.UpdateTask(Id, taskModel);
            return Ok(updatedTask);
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }
    }

    // DELETE
    [HttpDelete("DeleteTaskBy{id}")]
    public async Task<IActionResult> DeleteProjectTask([FromRoute] int id)
    {
        try
        {
            var project = await _projectTaskService.GetTaskById(id);
            if (project != null)
            {
                await _projectTaskService.DeleteTask(id);
                return Ok();
            }
            else
            {
                return NotFound(); 
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
