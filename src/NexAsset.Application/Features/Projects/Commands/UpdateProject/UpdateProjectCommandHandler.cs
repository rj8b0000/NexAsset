using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Projects.Queries.GetProjects;

namespace NexAsset.Application.Features.Projects.Commands.UpdateProject;

public sealed class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result<ProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectCategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProjectCommandHandler(
        IProjectRepository projectRepository,
        IProjectCategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectResponse>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken);
        if (project == null)
        {
            return Result<ProjectResponse>.Failure("Project not found.");
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null || category.OrganizationId != request.OrganizationId)
        {
            return Result<ProjectResponse>.Failure("Invalid Project Category for this Organization.");
        }

        var exists = await _projectRepository.ExistsByCodeAsync(request.OrganizationId, request.ProjectCode, request.Id, cancellationToken);
        if (exists)
        {
            return Result<ProjectResponse>.Failure($"Project Code '{request.ProjectCode}' already exists in this Organization.");
        }

        ProjectMapper.ApplyUpdate(request, project);
        project.UpdatedAtUtc = DateTime.UtcNow;

        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var loadedProject = await _projectRepository.GetByIdWithDetailsAsync(project.Id, cancellationToken);
        return Result<ProjectResponse>.Success(ProjectMapper.ToResponse(loadedProject ?? project));
    }
}
