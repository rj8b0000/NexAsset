using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Projects.Queries.GetProjects;

namespace NexAsset.Application.Features.Projects.Queries.GetProject;

public sealed class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, Result<ProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<ProjectResponse>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (project == null)
        {
            return Result<ProjectResponse>.Failure("Project not found.");
        }

        return Result<ProjectResponse>.Success(ProjectMapper.ToResponse(project));
    }
}
