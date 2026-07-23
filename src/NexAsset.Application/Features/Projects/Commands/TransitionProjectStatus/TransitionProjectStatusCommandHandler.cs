using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Projects.Queries.GetProjects;
using NexAsset.Domain.Helpers;

namespace NexAsset.Application.Features.Projects.Commands.TransitionProjectStatus;

public sealed class TransitionProjectStatusCommandHandler : IRequestHandler<TransitionProjectStatusCommand, Result<ProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransitionProjectStatusCommandHandler(IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectResponse>> Handle(TransitionProjectStatusCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken);
        if (project == null)
        {
            return Result<ProjectResponse>.Failure("Project not found.");
        }

        if (!ProjectStatusTransition.IsAllowed(project.Status, request.NewStatus))
        {
            return Result<ProjectResponse>.Failure($"Transition from {project.Status} to {request.NewStatus} is not allowed.");
        }

        project.Status = request.NewStatus;
        project.UpdatedAtUtc = DateTime.UtcNow;

        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var loadedProject = await _projectRepository.GetByIdWithDetailsAsync(project.Id, cancellationToken);
        return Result<ProjectResponse>.Success(ProjectMapper.ToResponse(loadedProject ?? project));
    }
}
