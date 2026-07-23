using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Projects.Queries.GetProjects;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Projects.Commands.CreateProject;

public sealed class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<ProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IProjectCategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectResponse>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null || category.OrganizationId != request.OrganizationId)
        {
            return Result<ProjectResponse>.Failure("Invalid Project Category for this Organization.");
        }

        if (!category.IsActive)
        {
            return Result<ProjectResponse>.Failure("Cannot create project under an inactive category.");
        }

        var exists = await _projectRepository.ExistsByCodeAsync(request.OrganizationId, request.ProjectCode, cancellationToken);
        if (exists)
        {
            return Result<ProjectResponse>.Failure($"Project Code '{request.ProjectCode}' already exists in this Organization.");
        }

        var project = ProjectMapper.ToEntity(request);
        project.Status = ProjectStatus.Draft;

        await _projectRepository.AddAsync(project, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var loadedProject = await _projectRepository.GetByIdWithDetailsAsync(project.Id, cancellationToken);
        return Result<ProjectResponse>.Success(ProjectMapper.ToResponse(loadedProject ?? project));
    }
}
