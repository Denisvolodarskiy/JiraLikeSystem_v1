using JiraLikeSystem.Core.Interfaces;
using JiraLikeSystem.Core.Models;
using JiraLikeSystem.Models.Entities;
using JiraLikeSystem.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace JiraLikeSystem.Tests.UnitTests.Controllers
{
    public class ProjectControllerTests
    {
        private readonly Mock<IProjectService> _mockProjectService;
        private readonly ProjectController _controller;

        public ProjectControllerTests()
        {
            _mockProjectService = new Mock<IProjectService>();
            _controller = new ProjectController(_mockProjectService.Object);
        }

        [Fact]
        public async Task GetProjects_ReturnsOkResult_WithListOfProjects()
        {

            var projects = new List<Project> { new Project { Title = "Test Project" } };
            _mockProjectService.Setup(service => service.GetProjects()).ReturnsAsync(projects);


            var result = await _controller.GetProjects();


            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Project>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetProjects_ReturnsBadRequest_OnArgumentException()
        {

            _mockProjectService.Setup(service => service.GetProjects()).ThrowsAsync(new ArgumentException("Error"));


            var result = await _controller.GetProjects();

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal("Error", badRequestResult.Value);
        }

        [Fact]
        public async Task GetProject_ReturnsOkResult_WithProject()
        {
  
            var project = new Project { Title = "Test Project" };
            _mockProjectService.Setup(service => service.GetProjectById(It.IsAny<int>())).ReturnsAsync(project);

     
            var result = await _controller.GetProject(1);

       
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Project>(okResult.Value);
            Assert.Equal("Test Project", returnValue.Title);
        }

        [Fact]
        public async Task GetProject_ReturnsNotFound_WhenProjectNotFound()
        {
           
            _mockProjectService.Setup(service => service.GetProjectById(It.IsAny<int>())).ReturnsAsync((Project)null);

         
            var result = await _controller.GetProject(1);

            
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetProject_ReturnsBadRequest_OnArgumentException()
        {
           
            _mockProjectService.Setup(service => service.GetProjectById(It.IsAny<int>())).ThrowsAsync(new ArgumentException("Error"));

          
            var result = await _controller.GetProject(1);

            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateProject_ReturnsOkResult_WithCreatedProject()
        {
            
            var projectModel = new ProjectModel { Title = "Test Project" };
            var createdProject = new Project { Title = "Test Project" };
            _mockProjectService.Setup(service => service.CreateProject(projectModel)).ReturnsAsync(createdProject);

           
            var result = await _controller.CreateProject(projectModel);

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Project>(okResult.Value);
            Assert.Equal("Test Project", returnValue.Title);
        }

        [Fact]
        public async Task CreateProject_ReturnsBadRequest_OnArgumentException()
        {
            
            var projectModel = new ProjectModel { Title = "Test Project" };
            _mockProjectService.Setup(service => service.CreateProject(projectModel)).ThrowsAsync(new ArgumentException("Error"));

            
            var result = await _controller.CreateProject(projectModel);

            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProject_ReturnsOkResult_WithUpdatedProject()
        {
            
            var projectModel = new ProjectModel { Title = "Updated Project" };
            var existingProject = new Project { Title = "Existing Project" };
            var updatedProject = new Project { Title = "Updated Project" };

            _mockProjectService.Setup(service => service.GetProjectById(It.IsAny<int>())).ReturnsAsync(existingProject);
            _mockProjectService.Setup(service => service.UpdateProject(It.IsAny<int>(), projectModel)).ReturnsAsync(updatedProject);

            
            var result = await _controller.UpdateProject(1, projectModel);

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Project>(okResult.Value);
            Assert.Equal("Updated Project", returnValue.Title);
        }

        [Fact]
        public async Task UpdateProject_ReturnsNotFound_WhenProjectNotFound()
        {
            
            var projectModel = new ProjectModel { Title = "Updated Project" };
            _mockProjectService.Setup(service => service.GetProjectById(It.IsAny<int>())).ReturnsAsync((Project)null);
 
            var result = await _controller.UpdateProject(1, projectModel);
            
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProject_ReturnsNotFound_OnArgumentException()
        {
            
            var projectModel = new ProjectModel { Title = "Updated Project" };
            _mockProjectService.Setup(service => service.UpdateProject(It.IsAny<int>(), projectModel)).ThrowsAsync(new ArgumentException());
            
            var result = await _controller.UpdateProject(1, projectModel);

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Project not found", notFoundObjectResult.Value);
        }

        [Fact]
        public async Task DeleteProject_ReturnsNoContent_WhenProjectDeleted()
        {
            
            var existingProject = new Project { Title = "Existing Project" };
            _mockProjectService.Setup(service => service.GetProjectById(It.IsAny<int>())).ReturnsAsync(existingProject);

           
            var result = await _controller.DeleteProject(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProject_ReturnsNotFound_WhenProjectNotFound()
        {
            
            _mockProjectService.Setup(service => service.GetProjectById(It.IsAny<int>())).ReturnsAsync((Project)null);

            
            var result = await _controller.DeleteProject(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProject_ReturnsNotFound_OnArgumentException()
        {
            
            var errorMessage = "Error";
            _mockProjectService.Setup(service => service.DeleteProject(It.IsAny<int>())).ThrowsAsync(new ArgumentException(errorMessage));

            var result = await _controller.DeleteProject(1);
            
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}
