using FluentAssertions;
using FluentValidation.TestHelper;
using NexAsset.Application.Features.ProjectWorkspaces;

namespace NexAsset.Application.UnitTests.Features.ProjectWorkspaces;

public class ProjectWorkspaceValidatorTests
{
    [Fact]
    public void CreateProjectCategoryCommandValidator_Should_HaveError_When_NameIsEmpty()
    {
        var validator = new CreateProjectCategoryCommandValidator();
        var command = new CreateProjectCategoryCommand(Guid.NewGuid(), "", null, false);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateProjectCategoryCommandValidator_Should_NotHaveError_When_NameIsProvided()
    {
        var validator = new CreateProjectCategoryCommandValidator();
        var command = new CreateProjectCategoryCommand(Guid.NewGuid(), "Valid Name", null, false);

        var result = validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateProjectCommandValidator_Should_HaveError_When_EndDateIsBeforeStartDate()
    {
        var validator = new CreateProjectCommandValidator();
        var command = new CreateProjectCommand(
            Guid.NewGuid(), Guid.NewGuid(), null, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            "Project", null, Domain.Enums.ProjectPriority.Medium, Domain.Enums.ProjectStatus.Draft,
            new DateOnly(2025, 1, 10), new DateOnly(2025, 1, 9), null, null);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Fact]
    public void CreateProjectCommandValidator_Should_NotHaveError_When_DatesAreValid()
    {
        var validator = new CreateProjectCommandValidator();
        var command = new CreateProjectCommand(
            Guid.NewGuid(), Guid.NewGuid(), null, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            "Project", null, Domain.Enums.ProjectPriority.Medium, Domain.Enums.ProjectStatus.Draft,
            new DateOnly(2025, 1, 10), new DateOnly(2025, 1, 11), new DateOnly(2025, 1, 12), null);

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void AddProjectMemberCommandValidator_Should_HaveError_When_AllocationIsInvalid(decimal allocation)
    {
        var validator = new AddProjectMemberCommandValidator();
        var command = new AddProjectMemberCommand(Guid.NewGuid(), Guid.NewGuid(), "Developer", allocation, new DateOnly(2025, 1, 1), null);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AllocationPercentage);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public void AddProjectMemberCommandValidator_Should_NotHaveError_When_AllocationIsValid(decimal allocation)
    {
        var validator = new AddProjectMemberCommandValidator();
        var command = new AddProjectMemberCommand(Guid.NewGuid(), Guid.NewGuid(), "Developer", allocation, new DateOnly(2025, 1, 1), null);

        var result = validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.AllocationPercentage);
    }

    [Fact]
    public void AllocateProjectAssetCommandValidator_Should_HaveError_When_QuantityIsZero()
    {
        var validator = new AllocateProjectAssetCommandValidator();
        var command = new AllocateProjectAssetCommand(Guid.NewGuid(), Guid.NewGuid(), 0, new DateOnly(2025, 1, 1), null);

        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AllocatedQuantity);
    }
}
