using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Common;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.ProjectWorkspaces;

public sealed record CreateProjectCategoryCommand(Guid OrganizationId, string Name, string? Description, bool IsSystemSuggested) : IRequest<Result<ProjectCategoryDto>>;
public sealed record UpdateProjectCategoryCommand(Guid Id, Guid OrganizationId, string Name, string? Description, bool IsActive) : IRequest<Result<ProjectCategoryDto>>;
public sealed record DeleteProjectCategoryCommand(Guid Id) : IRequest<Result>;
public sealed record GetProjectCategoryQuery(Guid Id) : IRequest<Result<ProjectCategoryDto>>;
public sealed class GetProjectCategoriesQuery : PagedRequest, IRequest<Result<PagedResponse<ProjectCategoryDto>>>;

public sealed record CreateProjectCommand(Guid OrganizationId, Guid CategoryId, Guid? ClientId, Guid BranchId, Guid DepartmentId, Guid ProjectManagerId, string ProjectName, string? Description, ProjectPriority Priority, ProjectStatus Status, DateOnly StartDate, DateOnly? EndDate, DateOnly? ExpectedCompletion, string? Notes) : IRequest<Result<ProjectDto>>;
public sealed record UpdateProjectCommand(Guid Id, Guid OrganizationId, Guid CategoryId, Guid? ClientId, Guid BranchId, Guid DepartmentId, Guid ProjectManagerId, string ProjectName, string? Description, ProjectPriority Priority, ProjectStatus Status, DateOnly StartDate, DateOnly? EndDate, DateOnly? ExpectedCompletion, string? Notes) : IRequest<Result<ProjectDto>>;
public sealed record DeleteProjectCommand(Guid Id) : IRequest<Result>;
public sealed record ArchiveProjectCommand(Guid Id) : IRequest<Result<ProjectDto>>;
public sealed record DuplicateProjectCommand(Guid Id, string ProjectName, DateOnly StartDate) : IRequest<Result<ProjectDto>>;
public sealed record GetProjectQuery(Guid Id) : IRequest<Result<ProjectDto>>;
public sealed class GetProjectsQuery : PagedRequest, IRequest<Result<PagedResponse<ProjectDto>>>;

public sealed record UpsertProjectDraftCommand(Guid? Id, Guid OrganizationId, Guid? ProjectId, Guid OwnerEmployeeId, int CurrentStep, string DraftName, string? DraftState) : IRequest<Result<ProjectDraftDto>>;
public sealed record GetProjectDraftQuery(Guid Id) : IRequest<Result<ProjectDraftDto>>;
public sealed class GetProjectDraftsQuery : PagedRequest, IRequest<Result<PagedResponse<ProjectDraftDto>>>;

public sealed record AddProjectMemberCommand(Guid ProjectId, Guid EmployeeId, string RoleInProject, decimal AllocationPercentage, DateOnly JoinedOn, string? Remarks) : IRequest<Result<ProjectMemberDto>>;
public sealed record UpdateProjectMemberCommand(Guid Id, string RoleInProject, decimal AllocationPercentage, DateOnly JoinedOn, DateOnly? ReleasedOn, ProjectMemberStatus Status, string? Remarks) : IRequest<Result<ProjectMemberDto>>;
public sealed record RemoveProjectMemberCommand(Guid Id) : IRequest<Result>;
public sealed record GetProjectMembersQuery(Guid ProjectId) : IRequest<Result<IReadOnlyCollection<ProjectMemberDto>>>;

public sealed record AllocateProjectAssetCommand(Guid ProjectId, Guid AssetId, int AllocatedQuantity, DateOnly AllocatedOn, string? Remarks) : IRequest<Result<ProjectAssetAllocationDto>>;
public sealed record ReturnProjectAssetCommand(Guid Id, int ReturnedQuantity, DateOnly ReturnedOn, string? Remarks) : IRequest<Result<ProjectAssetAllocationDto>>;
public sealed record GetProjectAssetAllocationsQuery(Guid ProjectId) : IRequest<Result<IReadOnlyCollection<ProjectAssetAllocationDto>>>;
public sealed record GetAssetProjectHistoryQuery(Guid AssetId) : IRequest<Result<IReadOnlyCollection<ProjectAssetAllocationDto>>>;

public sealed record CreateProjectParameterGroupCommand(Guid ProjectId, string GroupName, int DisplayOrder) : IRequest<Result<ProjectParameterGroupDto>>;
public sealed record UpdateProjectParameterGroupCommand(Guid Id, string GroupName, int DisplayOrder) : IRequest<Result<ProjectParameterGroupDto>>;
public sealed record DeleteProjectParameterGroupCommand(Guid Id) : IRequest<Result>;
public sealed record GetProjectParameterGroupsQuery(Guid ProjectId) : IRequest<Result<IReadOnlyCollection<ProjectParameterGroupDto>>>;

public sealed record CreateProjectParameterCommand(Guid ProjectId, Guid GroupId, string ParameterName, ProjectParameterInputType InputType, string? Value, string? Unit, bool Required, int DisplayOrder, bool IsVisible) : IRequest<Result<ProjectParameterDto>>;
public sealed record UpdateProjectParameterCommand(Guid Id, Guid GroupId, string ParameterName, ProjectParameterInputType InputType, string? Value, string? Unit, bool Required, int DisplayOrder, bool IsVisible) : IRequest<Result<ProjectParameterDto>>;
public sealed record DeleteProjectParameterCommand(Guid Id) : IRequest<Result>;
public sealed record GetProjectParametersQuery(Guid ProjectId) : IRequest<Result<IReadOnlyCollection<ProjectParameterDto>>>;

public sealed record AddProjectDocumentCommand(Guid ProjectId, string Category, string DocumentName, string FilePath, Guid UploadedBy, DateOnly? ExpiryDate, string? Remarks) : IRequest<Result<ProjectDocumentDto>>;
public sealed record ReplaceProjectDocumentCommand(Guid Id, string DocumentName, string FilePath, Guid UploadedBy, DateOnly? ExpiryDate, string? Remarks) : IRequest<Result<ProjectDocumentDto>>;
public sealed record DeleteProjectDocumentCommand(Guid Id) : IRequest<Result>;
public sealed record GetProjectDocumentsQuery(Guid ProjectId) : IRequest<Result<IReadOnlyCollection<ProjectDocumentDto>>>;
public sealed record GetProjectActivitiesQuery(Guid ProjectId) : IRequest<Result<IReadOnlyCollection<ProjectActivityDto>>>;

public sealed record UpsertProjectBudgetCommand(Guid ProjectId, decimal EstimatedBudget, decimal ActualCost, decimal ProcurementCost, decimal MaintenanceCost, decimal LabourCost, decimal MiscellaneousCost) : IRequest<Result<ProjectBudgetDto>>;
public sealed record GetProjectBudgetQuery(Guid ProjectId) : IRequest<Result<ProjectBudgetDto>>;

public sealed record CreateProjectRiskCommand(Guid ProjectId, string Title, string? Description, string Probability, string Impact, string Severity, string? MitigationPlan, Guid? OwnerEmployeeId) : IRequest<Result<ProjectRiskDto>>;
public sealed record UpdateProjectRiskCommand(Guid Id, string Title, string? Description, string Probability, string Impact, string Severity, string? MitigationPlan, Guid? OwnerEmployeeId, string Status) : IRequest<Result<ProjectRiskDto>>;
public sealed record DeleteProjectRiskCommand(Guid Id) : IRequest<Result>;
public sealed record GetProjectRisksQuery(Guid ProjectId) : IRequest<Result<IReadOnlyCollection<ProjectRiskDto>>>;

public sealed record UpsertProjectSettingCommand(Guid ProjectId, string Key, string Value, string? Description) : IRequest<Result<ProjectSettingDto>>;
public sealed record GetProjectSettingsQuery(Guid ProjectId) : IRequest<Result<IReadOnlyCollection<ProjectSettingDto>>>;

public sealed record GetProjectDashboardKpisQuery(Guid ProjectId) : IRequest<Result<ProjectDashboardKpiDto>>;

public sealed class ProjectWorkspaceHandler :
    IRequestHandler<CreateProjectCategoryCommand, Result<ProjectCategoryDto>>,
    IRequestHandler<UpdateProjectCategoryCommand, Result<ProjectCategoryDto>>,
    IRequestHandler<DeleteProjectCategoryCommand, Result>,
    IRequestHandler<GetProjectCategoryQuery, Result<ProjectCategoryDto>>,
    IRequestHandler<GetProjectCategoriesQuery, Result<PagedResponse<ProjectCategoryDto>>>,
    IRequestHandler<CreateProjectCommand, Result<ProjectDto>>,
    IRequestHandler<UpdateProjectCommand, Result<ProjectDto>>,
    IRequestHandler<DeleteProjectCommand, Result>,
    IRequestHandler<ArchiveProjectCommand, Result<ProjectDto>>,
    IRequestHandler<DuplicateProjectCommand, Result<ProjectDto>>,
    IRequestHandler<GetProjectQuery, Result<ProjectDto>>,
    IRequestHandler<GetProjectsQuery, Result<PagedResponse<ProjectDto>>>,
    IRequestHandler<UpsertProjectDraftCommand, Result<ProjectDraftDto>>,
    IRequestHandler<GetProjectDraftQuery, Result<ProjectDraftDto>>,
    IRequestHandler<GetProjectDraftsQuery, Result<PagedResponse<ProjectDraftDto>>>,
    IRequestHandler<AddProjectMemberCommand, Result<ProjectMemberDto>>,
    IRequestHandler<UpdateProjectMemberCommand, Result<ProjectMemberDto>>,
    IRequestHandler<RemoveProjectMemberCommand, Result>,
    IRequestHandler<GetProjectMembersQuery, Result<IReadOnlyCollection<ProjectMemberDto>>>,
    IRequestHandler<AllocateProjectAssetCommand, Result<ProjectAssetAllocationDto>>,
    IRequestHandler<ReturnProjectAssetCommand, Result<ProjectAssetAllocationDto>>,
    IRequestHandler<GetProjectAssetAllocationsQuery, Result<IReadOnlyCollection<ProjectAssetAllocationDto>>>,
    IRequestHandler<GetAssetProjectHistoryQuery, Result<IReadOnlyCollection<ProjectAssetAllocationDto>>>,
    IRequestHandler<CreateProjectParameterGroupCommand, Result<ProjectParameterGroupDto>>,
    IRequestHandler<UpdateProjectParameterGroupCommand, Result<ProjectParameterGroupDto>>,
    IRequestHandler<DeleteProjectParameterGroupCommand, Result>,
    IRequestHandler<GetProjectParameterGroupsQuery, Result<IReadOnlyCollection<ProjectParameterGroupDto>>>,
    IRequestHandler<CreateProjectParameterCommand, Result<ProjectParameterDto>>,
    IRequestHandler<UpdateProjectParameterCommand, Result<ProjectParameterDto>>,
    IRequestHandler<DeleteProjectParameterCommand, Result>,
    IRequestHandler<GetProjectParametersQuery, Result<IReadOnlyCollection<ProjectParameterDto>>>,
    IRequestHandler<AddProjectDocumentCommand, Result<ProjectDocumentDto>>,
    IRequestHandler<ReplaceProjectDocumentCommand, Result<ProjectDocumentDto>>,
    IRequestHandler<DeleteProjectDocumentCommand, Result>,
    IRequestHandler<GetProjectDocumentsQuery, Result<IReadOnlyCollection<ProjectDocumentDto>>>,
    IRequestHandler<GetProjectActivitiesQuery, Result<IReadOnlyCollection<ProjectActivityDto>>>,
    IRequestHandler<UpsertProjectBudgetCommand, Result<ProjectBudgetDto>>,
    IRequestHandler<GetProjectBudgetQuery, Result<ProjectBudgetDto>>,
    IRequestHandler<CreateProjectRiskCommand, Result<ProjectRiskDto>>,
    IRequestHandler<UpdateProjectRiskCommand, Result<ProjectRiskDto>>,
    IRequestHandler<DeleteProjectRiskCommand, Result>,
    IRequestHandler<GetProjectRisksQuery, Result<IReadOnlyCollection<ProjectRiskDto>>>,
    IRequestHandler<UpsertProjectSettingCommand, Result<ProjectSettingDto>>,
    IRequestHandler<GetProjectSettingsQuery, Result<IReadOnlyCollection<ProjectSettingDto>>>,
    IRequestHandler<GetProjectDashboardKpisQuery, Result<ProjectDashboardKpiDto>>
{
    private readonly IProjectWorkspaceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProjectWorkspaceHandler(IProjectWorkspaceRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectCategoryDto>> Handle(CreateProjectCategoryCommand r, CancellationToken ct)
    {
        var entity = new ProjectCategory { OrganizationId = r.OrganizationId, Name = r.Name, Description = r.Description, IsSystemSuggested = r.IsSystemSuggested, IsActive = true };
        await _repository.AddAsync(entity, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectCategoryDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<ProjectCategoryDto>> Handle(UpdateProjectCategoryCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<ProjectCategory>(r.Id, ct);
        if (entity is null) return Result<ProjectCategoryDto>.Failure("Project category not found.");
        entity.OrganizationId = r.OrganizationId; entity.Name = r.Name; entity.Description = r.Description; entity.IsActive = r.IsActive; Touch(entity);
        _repository.Update(entity); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectCategoryDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result> Handle(DeleteProjectCategoryCommand r, CancellationToken ct) => await SoftDelete<ProjectCategory>(r.Id, ct, "Project category not found.");
    public async Task<Result<ProjectCategoryDto>> Handle(GetProjectCategoryQuery r, CancellationToken ct) => await Get<ProjectCategory, ProjectCategoryDto>(r.Id, ProjectWorkspaceMapper.ToDto, ct, "Project category not found.");
    public async Task<Result<PagedResponse<ProjectCategoryDto>>> Handle(GetProjectCategoriesQuery r, CancellationToken ct) => Result<PagedResponse<ProjectCategoryDto>>.Success((await _repository.GetCategoriesAsync(r, ct)).Map(ProjectWorkspaceMapper.ToDto));

    public async Task<Result<ProjectDto>> Handle(CreateProjectCommand r, CancellationToken ct)
    {
        var entity = new Project { OrganizationId = r.OrganizationId, CategoryId = r.CategoryId, ClientId = r.ClientId, BranchId = r.BranchId, DepartmentId = r.DepartmentId, ProjectManagerId = r.ProjectManagerId, ProjectName = r.ProjectName, Description = r.Description, Priority = r.Priority, Status = r.Status, StartDate = r.StartDate, EndDate = r.EndDate, ExpectedCompletion = r.ExpectedCompletion, Notes = r.Notes };
        await _repository.AddAsync(entity, ct);
        await AddActivity(entity.Id, "Project Created", $"Project '{entity.ProjectName}' was created.", null, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<ProjectDto>> Handle(UpdateProjectCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<Project>(r.Id, ct);
        if (entity is null) return Result<ProjectDto>.Failure("Project not found.");
        var statusChanged = entity.Status != r.Status;
        entity.OrganizationId = r.OrganizationId; entity.CategoryId = r.CategoryId; entity.ClientId = r.ClientId; entity.BranchId = r.BranchId; entity.DepartmentId = r.DepartmentId; entity.ProjectManagerId = r.ProjectManagerId; entity.ProjectName = r.ProjectName; entity.Description = r.Description; entity.Priority = r.Priority; entity.Status = r.Status; entity.StartDate = r.StartDate; entity.EndDate = r.EndDate; entity.ExpectedCompletion = r.ExpectedCompletion; entity.Notes = r.Notes; Touch(entity);
        _repository.Update(entity);
        if (statusChanged) await AddActivity(entity.Id, "Status Changed", $"Project status changed to {entity.Status}.", null, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result> Handle(DeleteProjectCommand r, CancellationToken ct) => await SoftDelete<Project>(r.Id, ct, "Project not found.");

    public async Task<Result<ProjectDto>> Handle(ArchiveProjectCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<Project>(r.Id, ct);
        if (entity is null) return Result<ProjectDto>.Failure("Project not found.");
        entity.IsArchived = true; entity.ArchivedAtUtc = DateTime.UtcNow; entity.Status = ProjectStatus.Archived; Touch(entity);
        _repository.Update(entity);
        await AddActivity(entity.Id, "Project Archived", $"Project '{entity.ProjectName}' was archived.", null, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<ProjectDto>> Handle(DuplicateProjectCommand r, CancellationToken ct)
    {
        var source = await _repository.GetByIdAsync<Project>(r.Id, ct);
        if (source is null) return Result<ProjectDto>.Failure("Project not found.");
        var copy = new Project { OrganizationId = source.OrganizationId, CategoryId = source.CategoryId, ClientId = source.ClientId, BranchId = source.BranchId, DepartmentId = source.DepartmentId, ProjectManagerId = source.ProjectManagerId, ProjectName = r.ProjectName, Description = source.Description, Priority = source.Priority, Status = ProjectStatus.Draft, StartDate = r.StartDate, ExpectedCompletion = source.ExpectedCompletion, Notes = source.Notes };
        await _repository.AddAsync(copy, ct);
        await AddActivity(copy.Id, "Project Duplicated", $"Project was duplicated from '{source.ProjectName}'.", null, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectDto>.Success(ProjectWorkspaceMapper.ToDto(copy));
    }

    public async Task<Result<ProjectDto>> Handle(GetProjectQuery r, CancellationToken ct) => await Get<Project, ProjectDto>(r.Id, ProjectWorkspaceMapper.ToDto, ct, "Project not found.");
    public async Task<Result<PagedResponse<ProjectDto>>> Handle(GetProjectsQuery r, CancellationToken ct) => Result<PagedResponse<ProjectDto>>.Success((await _repository.GetProjectsAsync(r, ct)).Map(ProjectWorkspaceMapper.ToDto));

    public async Task<Result<ProjectDraftDto>> Handle(UpsertProjectDraftCommand r, CancellationToken ct)
    {
        ProjectDraft? entity = r.Id.HasValue ? await _repository.GetDraftAsync(r.Id.Value, ct) : null;
        if (entity is null)
        {
            entity = new ProjectDraft { OrganizationId = r.OrganizationId, ProjectId = r.ProjectId, OwnerEmployeeId = r.OwnerEmployeeId, CurrentStep = r.CurrentStep, DraftName = r.DraftName, DraftState = r.DraftState };
            await _repository.AddAsync(entity, ct);
        }
        else
        {
            entity.ProjectId = r.ProjectId; entity.CurrentStep = r.CurrentStep; entity.DraftName = r.DraftName; entity.DraftState = r.DraftState; entity.LastSavedAtUtc = DateTime.UtcNow; Touch(entity); _repository.Update(entity);
        }
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectDraftDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<ProjectDraftDto>> Handle(GetProjectDraftQuery r, CancellationToken ct) => await Get<ProjectDraft, ProjectDraftDto>(r.Id, ProjectWorkspaceMapper.ToDto, ct, "Project draft not found.");
    public async Task<Result<PagedResponse<ProjectDraftDto>>> Handle(GetProjectDraftsQuery r, CancellationToken ct) => Result<PagedResponse<ProjectDraftDto>>.Success((await _repository.GetDraftsAsync(r, ct)).Map(ProjectWorkspaceMapper.ToDto));

    public async Task<Result<ProjectMemberDto>> Handle(AddProjectMemberCommand r, CancellationToken ct)
    {
        var entity = new ProjectMember { ProjectId = r.ProjectId, EmployeeId = r.EmployeeId, RoleInProject = r.RoleInProject, AllocationPercentage = r.AllocationPercentage, JoinedOn = r.JoinedOn, Remarks = r.Remarks, Status = ProjectMemberStatus.Active };
        await _repository.AddAsync(entity, ct); await AddActivity(r.ProjectId, "Employee Added", $"Employee was added as {r.RoleInProject}.", r.EmployeeId, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectMemberDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<ProjectMemberDto>> Handle(UpdateProjectMemberCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<ProjectMember>(r.Id, ct);
        if (entity is null) return Result<ProjectMemberDto>.Failure("Project member not found.");
        entity.RoleInProject = r.RoleInProject; entity.AllocationPercentage = r.AllocationPercentage; entity.JoinedOn = r.JoinedOn; entity.ReleasedOn = r.ReleasedOn; entity.Status = r.Status; entity.Remarks = r.Remarks; Touch(entity);
        _repository.Update(entity); await AddActivity(entity.ProjectId, "Employee Updated", $"Project member role updated to {entity.RoleInProject}.", entity.EmployeeId, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectMemberDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result> Handle(RemoveProjectMemberCommand r, CancellationToken ct) => await SoftDelete<ProjectMember>(r.Id, ct, "Project member not found.");
    public async Task<Result<IReadOnlyCollection<ProjectMemberDto>>> Handle(GetProjectMembersQuery r, CancellationToken ct) => Result<IReadOnlyCollection<ProjectMemberDto>>.Success((await _repository.GetMembersAsync(r.ProjectId, ct)).Select(ProjectWorkspaceMapper.ToDto).ToList());

    public async Task<Result<ProjectAssetAllocationDto>> Handle(AllocateProjectAssetCommand r, CancellationToken ct)
    {
        var entity = new ProjectAssetAllocation { ProjectId = r.ProjectId, AssetId = r.AssetId, AllocatedQuantity = r.AllocatedQuantity, AllocatedOn = r.AllocatedOn, Remarks = r.Remarks, Status = ProjectAssetAllocationStatus.Allocated };
        await _repository.AddAsync(entity, ct); await AddActivity(r.ProjectId, "Asset Allocated", $"Asset allocation created for quantity {r.AllocatedQuantity}.", null, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectAssetAllocationDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<ProjectAssetAllocationDto>> Handle(ReturnProjectAssetCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<ProjectAssetAllocation>(r.Id, ct);
        if (entity is null) return Result<ProjectAssetAllocationDto>.Failure("Project asset allocation not found.");
        if (r.ReturnedQuantity > entity.AllocatedQuantity) return Result<ProjectAssetAllocationDto>.Failure("Returned quantity cannot exceed allocated quantity.");
        entity.ReturnedQuantity = r.ReturnedQuantity; entity.ReturnedOn = r.ReturnedOn; entity.Remarks = r.Remarks ?? entity.Remarks; entity.Status = r.ReturnedQuantity >= entity.AllocatedQuantity ? ProjectAssetAllocationStatus.Returned : ProjectAssetAllocationStatus.PartiallyReturned; Touch(entity);
        _repository.Update(entity); await AddActivity(entity.ProjectId, "Asset Returned", $"Asset return recorded for quantity {r.ReturnedQuantity}.", null, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectAssetAllocationDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<IReadOnlyCollection<ProjectAssetAllocationDto>>> Handle(GetProjectAssetAllocationsQuery r, CancellationToken ct) => Result<IReadOnlyCollection<ProjectAssetAllocationDto>>.Success((await _repository.GetAssetAllocationsAsync(r.ProjectId, ct)).Select(ProjectWorkspaceMapper.ToDto).ToList());
    public async Task<Result<IReadOnlyCollection<ProjectAssetAllocationDto>>> Handle(GetAssetProjectHistoryQuery r, CancellationToken ct) => Result<IReadOnlyCollection<ProjectAssetAllocationDto>>.Success((await _repository.GetAssetHistoryAsync(r.AssetId, ct)).Select(ProjectWorkspaceMapper.ToDto).ToList());

    public async Task<Result<ProjectParameterGroupDto>> Handle(CreateProjectParameterGroupCommand r, CancellationToken ct)
    {
        var entity = new ProjectParameterGroup { ProjectId = r.ProjectId, GroupName = r.GroupName, DisplayOrder = r.DisplayOrder };
        await _repository.AddAsync(entity, ct); await AddActivity(r.ProjectId, "Parameter Group Created", $"Parameter group '{r.GroupName}' was created.", null, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectParameterGroupDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<ProjectParameterGroupDto>> Handle(UpdateProjectParameterGroupCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<ProjectParameterGroup>(r.Id, ct);
        if (entity is null) return Result<ProjectParameterGroupDto>.Failure("Project parameter group not found.");
        entity.GroupName = r.GroupName; entity.DisplayOrder = r.DisplayOrder; Touch(entity);
        _repository.Update(entity); await AddActivity(entity.ProjectId, "Parameter Group Updated", $"Parameter group '{r.GroupName}' was updated.", null, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectParameterGroupDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result> Handle(DeleteProjectParameterGroupCommand r, CancellationToken ct) => await SoftDelete<ProjectParameterGroup>(r.Id, ct, "Project parameter group not found.");
    public async Task<Result<IReadOnlyCollection<ProjectParameterGroupDto>>> Handle(GetProjectParameterGroupsQuery r, CancellationToken ct) => Result<IReadOnlyCollection<ProjectParameterGroupDto>>.Success((await _repository.GetParameterGroupsAsync(r.ProjectId, ct)).Select(ProjectWorkspaceMapper.ToDto).ToList());

    public async Task<Result<ProjectParameterDto>> Handle(CreateProjectParameterCommand r, CancellationToken ct)
    {
        var entity = new ProjectParameter { ProjectId = r.ProjectId, GroupId = r.GroupId, ParameterName = r.ParameterName, InputType = r.InputType, Value = r.Value, Unit = r.Unit, Required = r.Required, DisplayOrder = r.DisplayOrder, IsVisible = r.IsVisible };
        await _repository.AddAsync(entity, ct); await AddActivity(r.ProjectId, "Parameter Updated", $"Parameter '{r.ParameterName}' was created.", null, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectParameterDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<ProjectParameterDto>> Handle(UpdateProjectParameterCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<ProjectParameter>(r.Id, ct);
        if (entity is null) return Result<ProjectParameterDto>.Failure("Project parameter not found.");
        entity.GroupId = r.GroupId; entity.ParameterName = r.ParameterName; entity.InputType = r.InputType; entity.Value = r.Value; entity.Unit = r.Unit; entity.Required = r.Required; entity.DisplayOrder = r.DisplayOrder; entity.IsVisible = r.IsVisible; Touch(entity);
        _repository.Update(entity); await AddActivity(entity.ProjectId, "Parameter Updated", $"Parameter '{r.ParameterName}' was updated.", null, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectParameterDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result> Handle(DeleteProjectParameterCommand r, CancellationToken ct) => await SoftDelete<ProjectParameter>(r.Id, ct, "Project parameter not found.");
    public async Task<Result<IReadOnlyCollection<ProjectParameterDto>>> Handle(GetProjectParametersQuery r, CancellationToken ct) => Result<IReadOnlyCollection<ProjectParameterDto>>.Success((await _repository.GetParametersAsync(r.ProjectId, ct)).Select(ProjectWorkspaceMapper.ToDto).ToList());

    public async Task<Result<ProjectDocumentDto>> Handle(AddProjectDocumentCommand r, CancellationToken ct)
    {
        var entity = new ProjectDocument { ProjectId = r.ProjectId, Category = r.Category, DocumentName = r.DocumentName, FilePath = r.FilePath, UploadedBy = r.UploadedBy, ExpiryDate = r.ExpiryDate, Remarks = r.Remarks };
        await _repository.AddAsync(entity, ct); await AddActivity(r.ProjectId, "Document Uploaded", $"Document '{r.DocumentName}' was uploaded.", r.UploadedBy, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectDocumentDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<ProjectDocumentDto>> Handle(ReplaceProjectDocumentCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<ProjectDocument>(r.Id, ct);
        if (entity is null) return Result<ProjectDocumentDto>.Failure("Project document not found.");
        entity.DocumentName = r.DocumentName; entity.FilePath = r.FilePath; entity.UploadedBy = r.UploadedBy; entity.UploadedOn = DateTime.UtcNow; entity.Version += 1; entity.ExpiryDate = r.ExpiryDate; entity.Remarks = r.Remarks; Touch(entity);
        _repository.Update(entity); await AddActivity(entity.ProjectId, "Document Uploaded", $"Document '{r.DocumentName}' was replaced.", r.UploadedBy, ct); await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectDocumentDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result> Handle(DeleteProjectDocumentCommand r, CancellationToken ct) => await SoftDelete<ProjectDocument>(r.Id, ct, "Project document not found.");
    public async Task<Result<IReadOnlyCollection<ProjectDocumentDto>>> Handle(GetProjectDocumentsQuery r, CancellationToken ct) => Result<IReadOnlyCollection<ProjectDocumentDto>>.Success((await _repository.GetDocumentsAsync(r.ProjectId, ct)).Select(ProjectWorkspaceMapper.ToDto).ToList());
    public async Task<Result<IReadOnlyCollection<ProjectActivityDto>>> Handle(GetProjectActivitiesQuery r, CancellationToken ct) => Result<IReadOnlyCollection<ProjectActivityDto>>.Success((await _repository.GetActivitiesAsync(r.ProjectId, ct)).Select(ProjectWorkspaceMapper.ToDto).ToList());

    public async Task<Result<ProjectBudgetDto>> Handle(UpsertProjectBudgetCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetBudgetAsync(r.ProjectId, ct);
        if (entity is null)
        {
            entity = new ProjectBudget { ProjectId = r.ProjectId, EstimatedBudget = r.EstimatedBudget, ActualCost = r.ActualCost, ProcurementCost = r.ProcurementCost, MaintenanceCost = r.MaintenanceCost, LabourCost = r.LabourCost, MiscellaneousCost = r.MiscellaneousCost };
            await _repository.AddAsync(entity, ct);
        }
        else
        {
            entity.EstimatedBudget = r.EstimatedBudget; entity.ActualCost = r.ActualCost; entity.ProcurementCost = r.ProcurementCost; entity.MaintenanceCost = r.MaintenanceCost; entity.LabourCost = r.LabourCost; entity.MiscellaneousCost = r.MiscellaneousCost; Touch(entity);
            _repository.Update(entity);
        }
        await AddActivity(r.ProjectId, "Budget Updated", "Project budget was updated.", null, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectBudgetDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<ProjectBudgetDto>> Handle(GetProjectBudgetQuery r, CancellationToken ct)
    {
        var entity = await _repository.GetBudgetAsync(r.ProjectId, ct);
        return entity is null ? Result<ProjectBudgetDto>.Failure("Budget not found.") : Result<ProjectBudgetDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<ProjectRiskDto>> Handle(CreateProjectRiskCommand r, CancellationToken ct)
    {
        var entity = new ProjectRisk { ProjectId = r.ProjectId, Title = r.Title, Description = r.Description, Probability = r.Probability, Impact = r.Impact, Severity = r.Severity, MitigationPlan = r.MitigationPlan, OwnerEmployeeId = r.OwnerEmployeeId };
        await _repository.AddAsync(entity, ct);
        await AddActivity(r.ProjectId, "Risk Logged", $"Risk '{r.Title}' was logged.", r.OwnerEmployeeId, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectRiskDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<ProjectRiskDto>> Handle(UpdateProjectRiskCommand r, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync<ProjectRisk>(r.Id, ct);
        if (entity is null) return Result<ProjectRiskDto>.Failure("Risk not found.");
        entity.Title = r.Title; entity.Description = r.Description; entity.Probability = r.Probability; entity.Impact = r.Impact; entity.Severity = r.Severity; entity.MitigationPlan = r.MitigationPlan; entity.OwnerEmployeeId = r.OwnerEmployeeId; entity.Status = r.Status;
        if (r.Status == "Closed" && entity.ClosedDate == null) entity.ClosedDate = DateTime.UtcNow;
        else if (r.Status != "Closed") entity.ClosedDate = null;
        Touch(entity);
        _repository.Update(entity);
        await AddActivity(entity.ProjectId, "Risk Updated", $"Risk '{r.Title}' was updated to {r.Status}.", r.OwnerEmployeeId, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectRiskDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result> Handle(DeleteProjectRiskCommand r, CancellationToken ct) => await SoftDelete<ProjectRisk>(r.Id, ct, "Risk not found.");
    public async Task<Result<IReadOnlyCollection<ProjectRiskDto>>> Handle(GetProjectRisksQuery r, CancellationToken ct) => Result<IReadOnlyCollection<ProjectRiskDto>>.Success((await _repository.GetRisksAsync(r.ProjectId, ct)).Select(ProjectWorkspaceMapper.ToDto).ToList());

    public async Task<Result<ProjectSettingDto>> Handle(UpsertProjectSettingCommand r, CancellationToken ct)
    {
        var settings = await _repository.GetSettingsAsync(r.ProjectId, ct);
        var entity = settings.FirstOrDefault(x => x.Key == r.Key);
        if (entity is null)
        {
            entity = new ProjectSetting { ProjectId = r.ProjectId, Key = r.Key, Value = r.Value, Description = r.Description };
            await _repository.AddAsync(entity, ct);
        }
        else
        {
            entity.Value = r.Value; entity.Description = r.Description; Touch(entity);
            _repository.Update(entity);
        }
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<ProjectSettingDto>.Success(ProjectWorkspaceMapper.ToDto(entity));
    }

    public async Task<Result<IReadOnlyCollection<ProjectSettingDto>>> Handle(GetProjectSettingsQuery r, CancellationToken ct) => Result<IReadOnlyCollection<ProjectSettingDto>>.Success((await _repository.GetSettingsAsync(r.ProjectId, ct)).Select(ProjectWorkspaceMapper.ToDto).ToList());

    public async Task<Result<ProjectDashboardKpiDto>> Handle(GetProjectDashboardKpisQuery r, CancellationToken ct)
    {
        var project = await _repository.GetByIdAsync<Project>(r.ProjectId, ct);
        if (project is null) return Result<ProjectDashboardKpiDto>.Failure("Project not found.");
        
        var budget = await _repository.GetBudgetAsync(r.ProjectId, ct);
        var risks = await _repository.GetRisksAsync(r.ProjectId, ct);
        var members = await _repository.GetMembersAsync(r.ProjectId, ct);
        var allocations = await _repository.GetAssetAllocationsAsync(r.ProjectId, ct);
        var docs = await _repository.GetDocumentsAsync(r.ProjectId, ct);
        
        decimal budgetUtilization = budget is not null && budget.EstimatedBudget > 0 ? (budget.ActualCost / budget.EstimatedBudget) * 100 : 0;
        
        var dto = new ProjectDashboardKpiDto(
            CompletionPercentage: 0,
            HealthStatus: risks.Any(x => x.Severity == "High" && x.Status == "Open") ? "At Risk" : "On Track",
            AssetsAllocated: allocations.Sum(x => x.AllocatedQuantity - x.ReturnedQuantity),
            EmployeesAssigned: members.Count(x => x.Status == ProjectMemberStatus.Active),
            ConsumablesUsed: 0,
            DocumentsUploaded: docs.Count,
            OpenRisks: risks.Count(x => x.Status == "Open"),
            PendingApprovals: 0,
            UpcomingDeadlines: 0,
            MaintenanceRequests: 0,
            BudgetUtilizationPercentage: budgetUtilization
        );
        return Result<ProjectDashboardKpiDto>.Success(dto);
    }

    private async Task<Result<TEntityDto>> Get<TEntity, TEntityDto>(Guid id, Func<TEntity, TEntityDto> mapper, CancellationToken ct, string notFound)
        where TEntity : BaseEntity
    {
        var entity = await _repository.GetByIdAsync<TEntity>(id, ct);
        return entity is null ? Result<TEntityDto>.Failure(notFound) : Result<TEntityDto>.Success(mapper(entity));
    }

    private async Task<Result> SoftDelete<TEntity>(Guid id, CancellationToken ct, string notFound)
        where TEntity : BaseEntity
    {
        var entity = await _repository.GetByIdAsync<TEntity>(id, ct);
        if (entity is null) return Result.Failure(notFound);
        entity.IsDeleted = true; entity.DeletedAtUtc = DateTime.UtcNow; Touch(entity);
        _repository.Update(entity); await _unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }

    private async Task AddActivity(Guid projectId, string type, string message, Guid? actorEmployeeId, CancellationToken ct)
    {
        await _repository.AddAsync(new ProjectActivity { ProjectId = projectId, ActivityType = type, Message = message, ActorEmployeeId = actorEmployeeId }, ct);
    }

    private static void Touch(BaseEntity entity) => entity.UpdatedAtUtc = DateTime.UtcNow;
}
