using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Application.Features.Projects.Commands;
using TaskTracker.Application.Features.Projects.Commands.Handlers;
using TaskTracker.Application.Features.Projects.Dtos;
using TaskTracker.Application.Features.Projects.Queries;
using TaskTracker.Application.Features.Projects.Queries.Handler;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces.Repositories;
using Xunit;

namespace TaskTracker.Test.UnitTests.Application.Handlers.Projects
{
    public class ProjectHandlersUnitTests
    {
        private readonly Mock<IProjectRepository> _projectRepo = new();
        private readonly Mock<IPersonRepository> _personRepo = new();

        [Fact]
        public async Task CreateProject_ReturnsDto()
        {
            var command = new CreateProjectCommand
            {
                Name = "Project",
                Description = "Desc",
                StartDate = DateTime.UtcNow,
                ContributorIds = new List<Guid>()
            };
            var handler = new CreateProjectCommandHandler(_projectRepo.Object, _personRepo.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Name.Should().Be("Project");
            _projectRepo.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProject_WhenFound_ReturnsDto()
        {
            var project = new Project("Name", "Desc", DateTime.UtcNow);
            var projectId = Guid.NewGuid();
            typeof(Project).GetProperty("Id")!.SetValue(project, projectId);
            _projectRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(project);
            var command = new UpdateProjectCommand { Id = projectId, Name = "New", Description = "Desc", StartDate = DateTime.UtcNow };
            var handler = new UpdateProjectCommandHandler(_projectRepo.Object, _personRepo.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Name.Should().Be("New");
            _projectRepo.Verify(r => r.UpdateAsync(project), Times.Once);
        }

        [Fact]
        public async Task DeleteProject_ReturnsDto()
        {
            var project = new Project("Name", "Desc", DateTime.UtcNow);
            typeof(Project).GetProperty("Id")!.SetValue(project, Guid.NewGuid());
            _projectRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(project);
            var handler = new DeleteProjectCommandHandler(_projectRepo.Object);

            var result = await handler.Handle(new DeleteProjectCommand { Id = 1 }, CancellationToken.None);

            result.Id.Should().Be(project.Id);
            _projectRepo.Verify(r => r.DeleteAsync(project), Times.Once);
        }

        [Fact]
        public async Task GetProjects_ReturnsDtos()
        {
            var project = new Project("Name", "Desc", DateTime.UtcNow);
            var list = new List<Project> { project };
            _projectRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);
            var handler = new GetProjectsQueryHandler(_projectRepo.Object);

            var result = await handler.Handle(new GetProjectsQuery(), CancellationToken.None);

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetProjectById_ReturnsDto()
        {
            var project = new Project("Name", "Desc", DateTime.UtcNow);
            typeof(Project).GetProperty("Id")!.SetValue(project, Guid.NewGuid());
            _projectRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(project);
            var handler = new GetProjectByIdQueryHandler(_projectRepo.Object);

            var result = await handler.Handle(new GetProjectByIdQuery(1), CancellationToken.None);

            result!.Id.Should().Be(project.Id);
        }
    }
}
