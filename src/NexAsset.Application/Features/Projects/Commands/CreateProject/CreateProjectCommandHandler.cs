using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.Projects.Queries.GetProjects;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Projects.Commands.CreateProject;

public sealed class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<ProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectCategoryRepository _categoryRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IProjectCategoryRepository categoryRepository,
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _categoryRepository = categoryRepository;
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectResponse>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Auto-resolve OrganizationId
            var orgId = request.OrganizationId;
            if (orgId == Guid.Empty)
            {
                var orgs = await _organizationRepository.GetAllAsync(cancellationToken);
                if (orgs != null && orgs.Count > 0)
                {
                    orgId = orgs[0].Id;
                }
                else
                {
                    return Result<ProjectResponse>.Failure("No valid Organization found in database.");
                }
            }

            // 2. Auto-resolve CategoryId
            ProjectCategory? category = null;
            if (request.CategoryId != Guid.Empty)
            {
                category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            }

            if (category == null)
            {
                var pagedCategories = await _categoryRepository.GetPagedAsync(new PagedRequest { PageNumber = 1, PageSize = 10 }, true, cancellationToken);
                if (pagedCategories.Items.Count > 0)
                {
                    category = pagedCategories.Items.First();
                }
                else
                {
                    category = new ProjectCategory
                    {
                        OrganizationId = orgId,
                        Name = "General Engineering",
                        Description = "Default auto-created project category",
                        IsActive = true
                    };
                    await _categoryRepository.AddAsync(category, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }

            // 3. Auto-resolve ProjectCode
            var code = !string.IsNullOrWhiteSpace(request.ProjectCode)
                ? request.ProjectCode.Trim()
                : $"PRJ-{Random.Shared.Next(100000, 999999)}";

            var exists = await _projectRepository.ExistsByCodeAsync(orgId, code, cancellationToken);
            if (exists)
            {
                code = $"PRJ-{Random.Shared.Next(100000, 999999)}";
            }

            // 4. Map & Persist Project
            var updatedCmd = request with { OrganizationId = orgId, CategoryId = category.Id, ProjectCode = code };
            var project = ProjectMapper.ToEntity(updatedCmd);
            project.OrganizationId = orgId;
            project.CategoryId = category.Id;
            project.ProjectCode = code;
            project.Status = ProjectStatus.Draft;

            await _projectRepository.AddAsync(project, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var loadedProject = await _projectRepository.GetByIdWithDetailsAsync(project.Id, cancellationToken);
            return Result<ProjectResponse>.Success(ProjectMapper.ToResponse(loadedProject ?? project));
        }
        catch (Exception ex)
        {
            return Result<ProjectResponse>.Failure($"Failed to create project: {ex.Message}");
        }
    }
}
