using FluentValidation;

namespace NexAsset.Application.Features.ProjectWorkspaces;

public sealed class CreateProjectCategoryCommandValidator : AbstractValidator<CreateProjectCategoryCommand>
{
    public CreateProjectCategoryCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.BranchId).NotEmpty();
        RuleFor(x => x.DepartmentId).NotEmpty();
        RuleFor(x => x.ProjectManagerId).NotEmpty();
        RuleFor(x => x.ProjectName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue);
        RuleFor(x => x.ExpectedCompletion).GreaterThanOrEqualTo(x => x.StartDate).When(x => x.ExpectedCompletion.HasValue);
    }
}

public sealed class AddProjectMemberCommandValidator : AbstractValidator<AddProjectMemberCommand>
{
    public AddProjectMemberCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.RoleInProject).NotEmpty().MaximumLength(100);
        RuleFor(x => x.AllocationPercentage).InclusiveBetween(0, 100);
    }
}

public sealed class AllocateProjectAssetCommandValidator : AbstractValidator<AllocateProjectAssetCommand>
{
    public AllocateProjectAssetCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.AllocatedQuantity).GreaterThan(0);
    }
}

public sealed class CreateProjectParameterGroupCommandValidator : AbstractValidator<CreateProjectParameterGroupCommand>
{
    public CreateProjectParameterGroupCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.GroupName).NotEmpty().MaximumLength(100);
    }
}

public sealed class CreateProjectParameterCommandValidator : AbstractValidator<CreateProjectParameterCommand>
{
    public CreateProjectParameterCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.GroupId).NotEmpty();
        RuleFor(x => x.ParameterName).NotEmpty().MaximumLength(200);
    }
}

public sealed class AddProjectDocumentCommandValidator : AbstractValidator<AddProjectDocumentCommand>
{
    public AddProjectDocumentCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DocumentName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.FilePath).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.UploadedBy).NotEmpty();
    }
}

public sealed class UpsertProjectDraftCommandValidator : AbstractValidator<UpsertProjectDraftCommand>
{
    public UpsertProjectDraftCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.OwnerEmployeeId).NotEmpty();
        RuleFor(x => x.CurrentStep).InclusiveBetween(1, 7);
        RuleFor(x => x.DraftName).NotEmpty().MaximumLength(200);
    }
}
