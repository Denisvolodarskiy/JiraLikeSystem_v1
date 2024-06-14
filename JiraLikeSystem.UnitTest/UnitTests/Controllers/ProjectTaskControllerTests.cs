
using JiraLikeSystem.Core.Interfaces;
using JiraLikeSystem.Core.Models;
using JiraLikeSystem.Models.Entities;
using JiraLikeSystem.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace JiraLikeSystem.Tests.UnitTests.Controllers
{
    public class ProjectTaskControllerTests
    {

        private readonly Mock<IProjectTaskService> _mockProjectTaskService;
        private readonly ProjectTaskController _controller;

        public ProjectTaskControllerTests()
        {
            _mockProjectTaskService = new Mock<IProjectTaskService>();
            _controller = new ProjectTaskController(_mockProjectTaskService.Object);
        }



        //=====================GetAllTasks====================================================

        [Fact]
        public async Task GetAllTasks_ReturnsOkResult_WithListOfTasks()
        {
            var tasks = new List<ProjectTask>
            {
                new ProjectTask { Title = "Test Task1" },
                new ProjectTask { Title = "Test Task2" },
                new ProjectTask { Title = "Test Task3" },
                new ProjectTask { Title = "Test Task4" }
            };

            _mockProjectTaskService.Setup(service => service.GetAllTasks()).ReturnsAsync(tasks);

            var result = await _controller.GetAllTasks();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProjectTask>>(okResult.Value);

            for (int i = 0; i < tasks.Count; i++)
            {
                Assert.Equal(tasks[i].Title, returnValue[i].Title);
            }
        }

        [Fact]
        public async Task GetAllTasks_ReturnsEmptyList_WhenNoTasksExist()
        {
            _mockProjectTaskService.Setup(service => service.GetAllTasks()).ReturnsAsync(new List<ProjectTask>());

            var result = await _controller.GetAllTasks();

            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnValue = Assert.IsType<List<ProjectTask>>(okResult.Value);

            Assert.Empty(returnValue);
        }

        [Fact]
        public async Task GetAllTasks_ReturnsBadRequest_OnArgumentException() 
        {
            _mockProjectTaskService.Setup(service => service.GetAllTasks()).ThrowsAsync(new ArgumentException());

            var result = await _controller.GetAllTasks();

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Value does not fall within the expected range.", badRequestResult.Value);
        }

        //=====================CreateTasks====================================================

        [Fact]
        public async Task GetTasks_ReturnsOkResult_WithTask_WhenTaskExists() 
        {

            var task = new ProjectTask { Title = "Test Task" };
            _mockProjectTaskService.Setup(service => service.GetTaskById(It.IsAny<int>())).ReturnsAsync(task);

            var result = await _controller.GetTasks(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProjectTask>(okResult.Value);

            Assert.Equal("Test Task", returnValue.Title);
        }

        [Fact]
        public async Task GetTasks_ReturnsNotFound_WhenTaskDoesNotExist() 
        {
            _mockProjectTaskService.Setup(service => service.GetTaskById(It.IsAny<int>())).ReturnsAsync((ProjectTask)null);

            var result = await _controller.GetTasks(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetTasks_ReturnsBadRequest_OnArgumentException() 
        {
            _mockProjectTaskService.Setup(service => service.GetTaskById(It.IsAny<int>())).ThrowsAsync(new ArgumentException());

            var result = await _controller.GetTasks(1);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Value does not fall within the expected range.", badRequestResult.Value);
        }


        //=====================CreateTask====================================================

        [Fact]
        public async Task CreateTask_ReturnsOkResult_WithCreatedTask()
        {
   
            var taskModel = new CreateTaskModel { Title = "New Task" };
            var createdTask = new ProjectTask { Title = taskModel.Title };

            _mockProjectTaskService.Setup(service => service.CreateTask(It.IsAny<int>(), taskModel))
                                   .ReturnsAsync(createdTask);

            var result = await _controller.CreateTask(1, taskModel);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProjectTask>(okResult.Value);
            Assert.Equal("New Task", returnValue.Title);
        }

        [Fact]
        public async Task CreateTask_ReturnsBadRequest_WhenTaskModelIsNull()
        {
            var result = await _controller.CreateTask(1, null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Task model is null.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateTask_ReturnsBadRequest_OnArgumentException()
        {
            var taskModel = new CreateTaskModel { Title = "New Task" };
            var errorMessage = "Error creating task";

            _mockProjectTaskService.Setup(service => service.CreateTask(It.IsAny<int>(), taskModel))
                                   .ThrowsAsync(new ArgumentException(errorMessage));

            var result = await _controller.CreateTask(1, taskModel);


            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }


        //=====================AssigneTaskToUser====================================================

        [Fact]
        public async Task AssigneTaskToUser_ReturnsOkResult_WithAssignedTask()
        {

            var taskModel = new AssignTaskModel { TaskId = 1, UserId = Guid.NewGuid() };

            _mockProjectTaskService.Setup(service => service.AssigneTaskToUser(taskModel)).ReturnsAsync(true);

            var result = await _controller.AssigneTaskToUser(taskModel);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<bool>(okResult.Value);
            Assert.True(returnValue);

            _mockProjectTaskService.Verify(service => service.AssigneTaskToUser(taskModel), Times.Once);
        }

        [Fact]
        public async Task AssigneTaskToUser_ReturnsBadRequest_OnArgumentException() 
        {
            var taskModel = new AssignTaskModel { TaskId = 1, UserId = Guid.NewGuid() };
            _mockProjectTaskService.Setup(service => service.AssigneTaskToUser(taskModel)).ThrowsAsync(new ArgumentException());

            var result = await _controller.AssigneTaskToUser(taskModel);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Value does not fall within the expected range.", badRequestResult.Value);
        }

        //=====================UpdateTask====================================================
        [Fact]
        public async Task UpdateTask_ReturnsOkResult_WithUpdatedTask()
        {
            var updateTaskModel = new UpdateTaskModel
            {
                Title = "New Title",
                Description = "New Description",
                Status = Models.Enums.TaskWorkStatus.InTesting,
                Priority = Models.Enums.TaskPriority.High,
                DueDate = DateTime.MinValue,
            };

            var newTask = new ProjectTask
            {
                Title = "New Title",
                Description = "New Description",
                Status = Models.Enums.TaskWorkStatus.InTesting,
                Priority = Models.Enums.TaskPriority.High,
                DueDate = DateTime.MinValue,
            };

            _mockProjectTaskService.Setup(service => service.UpdateTask(It.IsAny<int>(), It.IsAny<UpdateTaskModel>()))
                                   .ReturnsAsync(newTask);

            var result = await _controller.UpdateTask(1, updateTaskModel);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProjectTask>(okResult.Value);

            Assert.Equal(updateTaskModel.Title, returnValue.Title);
            Assert.Equal(updateTaskModel.Description, returnValue.Description);
            Assert.Equal(updateTaskModel.Status, returnValue.Status);
            Assert.Equal(updateTaskModel.Priority, returnValue.Priority);
            Assert.Equal(updateTaskModel.DueDate, returnValue.DueDate);
        }

        [Fact]
        public async Task UpdateTask_ReturnsBadRequest_OnArgumentException() 
        {
            var updateTaskModel = new UpdateTaskModel
            {
                Title = "New Title",
                Description = "New Description",
                Status = Models.Enums.TaskWorkStatus.InTesting,
                Priority = Models.Enums.TaskPriority.High,
                DueDate = DateTime.MinValue,
            };

            _mockProjectTaskService.Setup(service => service.UpdateTask(It.IsAny<int>(), It.IsAny<UpdateTaskModel>()))
                                   .ThrowsAsync(new ArgumentException());

            var result = await _controller.UpdateTask(1, updateTaskModel);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Value does not fall within the expected range.", badRequestResult.Value);
        }


        //=====================DeleteProjectTask====================================================

        [Fact]
        public async Task DeleteProjectTask_ReturnsOkResult_WhenTaskIsDeleted()
        {
            var taskToDelete = new ProjectTask { Id = 1, Title = "Test Task" };

            _mockProjectTaskService.Setup(service => service.GetTaskById(1)).ReturnsAsync(taskToDelete);
            _mockProjectTaskService.Setup(service => service.DeleteTask(1)).Returns(Task.CompletedTask);


            var result = await _controller.DeleteProjectTask(1);
            var okResult = Assert.IsType<OkResult>(result);

            _mockProjectTaskService.Verify(service => service.GetTaskById(1), Times.Once);
            _mockProjectTaskService.Verify(service => service.DeleteTask(1), Times.Once);

            _mockProjectTaskService.Setup(service => service.GetTaskById(1)).ReturnsAsync((ProjectTask)null);

            var checkResult = await _mockProjectTaskService.Object.GetTaskById(1);
            Assert.Null(checkResult);
        }

        [Fact]
        public async Task DeleteProjectTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            _mockProjectTaskService.Setup(service => service.GetTaskById(It.IsAny<int>())).ReturnsAsync((ProjectTask)null);

            var result = await _controller.DeleteProjectTask(1);
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            _mockProjectTaskService.Verify(service => service.GetTaskById(1), Times.Once);
            _mockProjectTaskService.Verify(service => service.DeleteTask(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteProjectTask_ReturnsBadRequest_OnException()
        {
            _mockProjectTaskService.Setup(service => service.GetTaskById(It.IsAny<int>())).ThrowsAsync(new ArgumentException("An error occurred"));

            var result = await _controller.DeleteProjectTask(1);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred", badRequestResult.Value);
            _mockProjectTaskService.Verify(service => service.GetTaskById(1), Times.Once);
            _mockProjectTaskService.Verify(service => service.DeleteTask(It.IsAny<int>()), Times.Never);
        }
    }
}
