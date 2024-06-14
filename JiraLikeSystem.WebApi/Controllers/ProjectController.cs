using JiraLikeSystem.Core.Interfaces;
using JiraLikeSystem.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JiraLikeSystem.WebApi.Controllers;

[Authorize(Roles = "Admin")]
[Route("[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    //GET 
    [HttpGet("GetAllProjects")] 
    public async Task<IActionResult> GetProjects()
    {

        try
        {
            return Ok(await _projectService.GetProjects());
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetProjectBy/{id}")] 
    public async Task<IActionResult> GetProject([FromRoute]int id)
    {
        try
        {
            var project = await _projectService.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetAllProjectTasks/{id}")]
    public async Task<IActionResult> GetProjectTasks([FromRoute] int id)
    {
        try
        {
            var projectTasks = await _projectService.GetProjectTasks(id);
            if (projectTasks == null)
            {
                return NotFound("No tasks found for the specified project.");
            }
            return Ok(projectTasks);
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }
    }

    //POST
    [HttpPost("CreateProject")] 
    public async Task<IActionResult> CreateProject([FromBody] ProjectModel project)
    {
        try
        {
            var createdProject = await _projectService.CreateProject(project);
            return Ok(createdProject);
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }
    }

    //PUT 
    [HttpPut("UpdateProjectById/{id}")] 
    public async Task<IActionResult> UpdateProject([FromRoute] int id, [FromBody] ProjectModel project)
    {

        try
        {
            var existingProject = await _projectService.GetProjectById(id);

            if (existingProject == null)
            {
                return NotFound("Project not found");
            }

            var updatedProject = await _projectService.UpdateProject(id, project);
            return Ok(updatedProject);
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        } 
    }

    // DELETE
    [HttpDelete("DeleteProjectBy/{id}")] 
    public async Task<IActionResult> DeleteProject([FromRoute] int id)
    {

        try
        {
            var project = await _projectService.GetProjectById(id);

            if (project == null)
            {
                return NotFound();
            }

            await _projectService.DeleteProject(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }
    }
}

