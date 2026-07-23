using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Projects.Queries.GetProjects;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Projects.Commands.DuplicateProject;

public sealed class DuplicateProjectCommandHandler : IRequestHandler<DuplicateProjectCommand, Result<ProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectParameterRepository _parameterRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DuplicateProjectCommandHandler(
        IProjectRepository projectRepository,
        IProjectParameterRepository parameterRepository,
        IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _parameterRepository = parameterRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectResponse>> Handle(DuplicateProjectCommand request, CancellationToken cancellationToken)
    {
        var sourceProject = await _projectRepository.GetByIdAsync(request.SourceProjectId, cancellationToken);
        if (sourceProject == null || sourceProject.OrganizationId != request.OrganizationId)
        {
            return Result<ProjectResponse>.Failure("Source project not found.");
        }

        var newCode = $"PRJ-{Random.Shared.Next(10000, 99999)}";
        var duplicateProject = new Project
        {
            OrganizationId = request.OrganizationId,
            ProjectCode = newCode,
            ProjectName = $"{sourceProject.ProjectName} (Copy)",
            Description = sourceProject.Description,
            Notes = sourceProject.Notes,
            InternalRemarks = sourceProject.InternalRemarks,
            CategoryId = sourceProject.CategoryId,
            BranchId = sourceProject.BranchId,
            DepartmentId = sourceProject.DepartmentId,
            Priority = sourceProject.Priority,
            Status = ProjectStatus.Draft,
            CustomerId = null,
            ProjectManagerEmployeeId = null,
            StartDate = null,
            EndDate = null,
            ExpectedCompletionDate = null
        };

        await _projectRepository.AddAsync(duplicateProject, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var sourceSections = await _parameterRepository.GetSectionsByProjectAsync(sourceProject.Id, cancellationToken);
        foreach (var sourceSec in sourceSections)
        {
            var newSec = new ProjectParameterSection
            {
                ProjectId = duplicateProject.Id,
                Name = sourceSec.Name,
                DisplayOrder = sourceSec.DisplayOrder
            };
            await _parameterRepository.AddSectionAsync(newSec, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var param in sourceSec.Parameters)
            {
                var newParam = new ProjectParameter
                {
                    SectionId = newSec.Id,
                    ParameterName = param.ParameterName,
                    InputType = param.InputType,
                    Unit = param.Unit,
                    IsRequired = param.IsRequired,
                    DisplayOrder = param.DisplayOrder,
                    DropdownOptionsJson = param.DropdownOptionsJson
                };
                await _parameterRepository.AddParameterAsync(newParam, cancellationToken);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var loaded = await _projectRepository.GetByIdWithDetailsAsync(duplicateProject.Id, cancellationToken);
        return Result<ProjectResponse>.Success(ProjectMapper.ToResponse(loaded ?? duplicateProject));
    }
}
