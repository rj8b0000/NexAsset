using FluentAssertions;
using Moq;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Features.ProjectWorkspaces;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.UnitTests.Features.ProjectWorkspaces;

public class ProjectWorkspaceHandlerTests
{
    private readonly Mock<IProjectWorkspaceRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ProjectWorkspaceHandler _handler;

    public ProjectWorkspaceHandlerTests()
    {
        _repositoryMock = new Mock<IProjectWorkspaceRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new ProjectWorkspaceHandler(_repositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_CreateProjectCommand_Should_ReturnSuccess()
    {
        // Arrange
        var command = new CreateProjectCommand(
            Guid.NewGuid(), Guid.NewGuid(), null, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            "New Project", "Description", Domain.Enums.ProjectPriority.High, Domain.Enums.ProjectStatus.Draft,
            new DateOnly(2025, 1, 1), null, null, null);

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.ProjectName.Should().Be("New Project");
        
        _repositoryMock.Verify(r => r.AddAsync(It.Is<Project>(p => p.ProjectName == "New Project"), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeleteProjectCommand_Should_ReturnSuccess_When_ProjectExists()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var project = new Project { Id = projectId, ProjectName = "To Delete" };
        var command = new DeleteProjectCommand(projectId);

        _repositoryMock.Setup(r => r.GetByIdAsync<Project>(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);
        _repositoryMock.Setup(r => r.Update(project));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        _repositoryMock.Verify(r => r.Update(project), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeleteProjectCommand_Should_ReturnFailure_When_ProjectDoesNotExist()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var command = new DeleteProjectCommand(projectId);

        _repositoryMock.Setup(r => r.GetByIdAsync<Project>(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        
        _repositoryMock.Verify(r => r.Update(It.IsAny<Project>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpsertProjectBudgetCommand_Should_Add_When_NotExists()
    {
        // Arrange
        var command = new UpsertProjectBudgetCommand(Guid.NewGuid(), 1000, 500, 100, 100, 200, 100);
        _repositoryMock.Setup(r => r.GetBudgetAsync(command.ProjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProjectBudget?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.AddAsync(It.Is<ProjectBudget>(b => b.EstimatedBudget == 1000), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CreateProjectRiskCommand_Should_ReturnSuccess()
    {
        // Arrange
        var command = new CreateProjectRiskCommand(Guid.NewGuid(), "Risk 1", null, "Low", "Low", "Low", null, null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.AddAsync(It.Is<ProjectRisk>(x => x.Title == "Risk 1"), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetProjectDashboardKpisQuery_Should_CalculateKpis()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var query = new GetProjectDashboardKpisQuery(projectId);
        
        _repositoryMock.Setup(r => r.GetByIdAsync<Project>(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Project { Id = projectId });
            
        _repositoryMock.Setup(r => r.GetBudgetAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProjectBudget { EstimatedBudget = 1000, ActualCost = 250 });
            
        _repositoryMock.Setup(r => r.GetRisksAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProjectRisk> { new ProjectRisk { Severity = "High", Status = "Open" } });
            
        _repositoryMock.Setup(r => r.GetMembersAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProjectMember> { new ProjectMember { Status = Domain.Enums.ProjectMemberStatus.Active } });

        _repositoryMock.Setup(r => r.GetAssetAllocationsAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProjectAssetAllocation>());

        _repositoryMock.Setup(r => r.GetDocumentsAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProjectDocument>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.BudgetUtilizationPercentage.Should().Be(25);
        result.Value.HealthStatus.Should().Be("At Risk");
        result.Value.EmployeesAssigned.Should().Be(1);
    }
}
