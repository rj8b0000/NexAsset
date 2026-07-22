using FluentValidation;
using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Features.ProjectCategories;

public sealed record ProjectCategoryResponse(Guid Id, Guid OrganizationId, string Name, string? Description, bool IsActive, DateTime CreatedAtUtc);
public sealed record ProjectCategoryListItemResponse(Guid Id, Guid OrganizationId, string Name, string? Description, bool IsActive, DateTime CreatedAtUtc);
public sealed record CreateProjectCategoryCommand(Guid OrganizationId, string Name, string? Description) : IRequest<Result<ProjectCategoryResponse>>;
public sealed record UpdateProjectCategoryCommand(Guid Id, Guid OrganizationId, string Name, string? Description, bool IsActive) : IRequest<Result<ProjectCategoryResponse>>;
public sealed record DeleteProjectCategoryCommand(Guid Id) : IRequest<Result>;
public sealed record GetProjectCategoryQuery(Guid Id) : IRequest<Result<ProjectCategoryResponse>>;
public sealed class GetProjectCategoriesQuery : PagedRequest, IRequest<Result<PagedResponse<ProjectCategoryListItemResponse>>>
{
    public Guid? OrganizationId { get; init; }
    public bool? IsActive { get; init; }
}

public sealed class CreateProjectCategoryCommandValidator : AbstractValidator<CreateProjectCategoryCommand>
{
    public CreateProjectCategoryCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}

public sealed class UpdateProjectCategoryCommandValidator : AbstractValidator<UpdateProjectCategoryCommand>
{
    public UpdateProjectCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}

public sealed class CreateProjectCategoryCommandHandler(IProjectCategoryRepository categories, IOrganizationRepository organizations, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProjectCategoryCommand, Result<ProjectCategoryResponse>>
{
    public async Task<Result<ProjectCategoryResponse>> Handle(CreateProjectCategoryCommand request, CancellationToken cancellationToken)
    {
        if (await organizations.GetByIdAsync(request.OrganizationId, cancellationToken) is null)
            return Result<ProjectCategoryResponse>.Failure("Organization not found.");
        if (await categories.ExistsByNameAsync(request.OrganizationId, request.Name, cancellationToken))
            return Result<ProjectCategoryResponse>.Failure("A project category with this name already exists.");
        var category = ProjectWorkspaceMapper.ToEntity(request);
        await categories.AddAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<ProjectCategoryResponse>.Success(ProjectWorkspaceMapper.ToResponse(category));
    }
}

public sealed class UpdateProjectCategoryCommandHandler(IProjectCategoryRepository categories, IOrganizationRepository organizations, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProjectCategoryCommand, Result<ProjectCategoryResponse>>
{
    public async Task<Result<ProjectCategoryResponse>> Handle(UpdateProjectCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categories.GetByIdAsync(request.Id, cancellationToken);
        if (category is null || category.OrganizationId != request.OrganizationId)
            return Result<ProjectCategoryResponse>.Failure("Project category not found.");
        if (await organizations.GetByIdAsync(request.OrganizationId, cancellationToken) is null)
            return Result<ProjectCategoryResponse>.Failure("Organization not found.");
        if (await categories.ExistsByNameAsync(request.OrganizationId, request.Name, request.Id, cancellationToken))
            return Result<ProjectCategoryResponse>.Failure("A project category with this name already exists.");
        ProjectWorkspaceMapper.ApplyUpdate(request, category);
        category.UpdatedAtUtc = DateTime.UtcNow;
        categories.Update(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<ProjectCategoryResponse>.Success(ProjectWorkspaceMapper.ToResponse(category));
    }
}

public sealed class DeleteProjectCategoryCommandHandler(IProjectCategoryRepository categories, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProjectCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteProjectCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categories.GetByIdAsync(request.Id, cancellationToken);
        if (category is null) return Result.Failure("Project category not found.");
        category.IsDeleted = true;
        category.DeletedAtUtc = DateTime.UtcNow;
        category.UpdatedAtUtc = DateTime.UtcNow;
        categories.Update(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

public sealed class GetProjectCategoryQueryHandler(IProjectCategoryRepository categories)
    : IRequestHandler<GetProjectCategoryQuery, Result<ProjectCategoryResponse>>
{
    public async Task<Result<ProjectCategoryResponse>> Handle(GetProjectCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await categories.GetByIdAsync(request.Id, cancellationToken);
        return category is null
            ? Result<ProjectCategoryResponse>.Failure("Project category not found.")
            : Result<ProjectCategoryResponse>.Success(ProjectWorkspaceMapper.ToResponse(category));
    }
}

public sealed class GetProjectCategoriesQueryHandler(IProjectCategoryRepository categories)
    : IRequestHandler<GetProjectCategoriesQuery, Result<PagedResponse<ProjectCategoryListItemResponse>>>
{
    public async Task<Result<PagedResponse<ProjectCategoryListItemResponse>>> Handle(GetProjectCategoriesQuery request, CancellationToken cancellationToken)
    {
        var page = await categories.GetPagedAsync(request, request.OrganizationId, request.IsActive, cancellationToken);
        return Result<PagedResponse<ProjectCategoryListItemResponse>>.Success(page.Map(ProjectWorkspaceMapper.ToListItemResponse));
    }
}
