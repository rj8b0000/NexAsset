using System.Text;
using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Entities;
using NexAsset.Domain.Enums;
using NexAsset.Domain.Helpers;

namespace NexAsset.Application.Features.Projects;

public sealed class ProjectWorkspaceCoreHandlers(
    IProjectRepository projects,
    IProjectCategoryRepository categories,
    IOrganizationRepository organizations,
    IBranchRepository branches,
    IDepartmentRepository departments,
    IEmployeeRepository employees,
    IAssetRepository assets,
    IDraftSessionRepository drafts,
    IProjectTeamRepository teams,
    IProjectAssetRepository allocations,
    IProjectDocumentRepository documents,
    IProjectParameterRepository parameters,
    IProjectTimelineRepository timeline,
    IProjectActivityRepository activities,
    IEnterpriseOperationsRepository operations,
    IFileStorageService fileStorage,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProjectCommand, Result<ProjectResponse>>,
      IRequestHandler<UpdateProjectCommand, Result<ProjectResponse>>,
      IRequestHandler<DeleteProjectCommand, Result>,
      IRequestHandler<TransitionProjectStatusCommand, Result<ProjectResponse>>,
      IRequestHandler<DuplicateProjectCommand, Result<ProjectResponse>>,
      IRequestHandler<GetProjectQuery, Result<ProjectResponse>>,
      IRequestHandler<GetProjectsQuery, Result<PagedResponse<ProjectListItemResponse>>>,
      IRequestHandler<UpsertDraftSessionCommand, Result<DraftSessionResponse>>,
      IRequestHandler<GetDraftSessionQuery, Result<DraftSessionResponse>>,
      IRequestHandler<DeleteDraftSessionCommand, Result>,
      IRequestHandler<AddTeamMemberCommand, Result<TeamMemberResponse>>,
      IRequestHandler<UpdateTeamMemberCommand, Result<TeamMemberResponse>>,
      IRequestHandler<ReleaseTeamMemberCommand, Result<TeamMemberResponse>>,
      IRequestHandler<RemoveTeamMemberCommand, Result>,
      IRequestHandler<GetTeamMembersQuery, Result<PagedResponse<TeamMemberResponse>>>,
      IRequestHandler<AllocateAssetCommand, Result<AssetAllocationResponse>>,
      IRequestHandler<ReturnAssetCommand, Result<AssetAllocationResponse>>,
      IRequestHandler<GetAssetAllocationsQuery, Result<PagedResponse<AssetAllocationResponse>>>,
      IRequestHandler<UploadDocumentCommand, Result<DocumentResponse>>,
      IRequestHandler<ReplaceDocumentCommand, Result<DocumentResponse>>,
      IRequestHandler<DeleteDocumentCommand, Result>,
      IRequestHandler<GetDocumentsQuery, Result<PagedResponse<DocumentResponse>>>,
      IRequestHandler<GetDocumentVersionHistoryQuery, Result<IReadOnlyCollection<DocumentResponse>>>,
      IRequestHandler<CreateParameterSectionCommand, Result<ParameterSectionResponse>>,
      IRequestHandler<UpdateParameterSectionCommand, Result<ParameterSectionResponse>>,
      IRequestHandler<DeleteParameterSectionCommand, Result>,
      IRequestHandler<AddParameterCommand, Result<ParameterResponse>>,
      IRequestHandler<UpdateParameterCommand, Result<ParameterResponse>>,
      IRequestHandler<DeleteParameterCommand, Result>,
      IRequestHandler<SaveParameterValuesCommand, Result>,
      IRequestHandler<GetParameterSectionsQuery, Result<IReadOnlyCollection<ParameterSectionResponse>>>
{
    public async Task<Result<ProjectResponse>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var referenceError = await ValidateProjectReferencesAsync(request.OrganizationId, request.CategoryId, request.CustomerId, request.BranchId, request.DepartmentId, request.ProjectManagerEmployeeId, cancellationToken);
        if (referenceError is not null) return Result<ProjectResponse>.Failure(referenceError);
        if (await projects.ExistsByCodeAsync(request.OrganizationId, request.ProjectCode, cancellationToken))
            return Result<ProjectResponse>.Failure("A project with this code already exists.");

        var project = ProjectWorkspaceMapper.ToEntity(request);
        project.Status = ProjectStatus.Draft;
        await projects.AddAsync(project, cancellationToken);
        await RecordAsync(project.Id, TimelineEventType.ProjectCreated, ActivityType.ProjectCreated, "Project created.", "Project", project.Id, cancellationToken);
        await NotifyManagerAsync(project.ProjectManagerEmployeeId, "Project assigned", $"You have been assigned to project {project.ProjectName}.", cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<ProjectResponse>.Success(ToResponse(project));
    }

    public async Task<Result<ProjectResponse>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projects.GetByIdAsync(request.Id, cancellationToken);
        if (project is null || project.OrganizationId != request.OrganizationId)
            return Result<ProjectResponse>.Failure("Project not found.");
        var referenceError = await ValidateProjectReferencesAsync(request.OrganizationId, request.CategoryId, request.CustomerId, request.BranchId, request.DepartmentId, request.ProjectManagerEmployeeId, cancellationToken);
        if (referenceError is not null) return Result<ProjectResponse>.Failure(referenceError);
        if (await projects.ExistsByCodeAsync(request.OrganizationId, request.ProjectCode, request.Id, cancellationToken))
            return Result<ProjectResponse>.Failure("A project with this code already exists.");

        ProjectWorkspaceMapper.ApplyUpdate(request, project);
        project.UpdatedAtUtc = DateTime.UtcNow;
        projects.Update(project);
        await RecordAsync(project.Id, null, ActivityType.ProjectUpdated, "Project updated.", "Project", project.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var details = await projects.GetByIdWithDetailsAsync(project.Id, cancellationToken) ?? project;
        return Result<ProjectResponse>.Success(ToResponse(details));
    }

    public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projects.GetByIdAsync(request.Id, cancellationToken);
        if (project is null) return Result.Failure("Project not found.");
        project.IsDeleted = true;
        project.DeletedAtUtc = DateTime.UtcNow;
        project.UpdatedAtUtc = DateTime.UtcNow;
        projects.Update(project);
        await operations.AddAsync(new AuditLog { UserId = currentUser.UserId, EntityName = "Project", EntityId = project.Id, Action = "Deleted" }, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<ProjectResponse>> Handle(TransitionProjectStatusCommand request, CancellationToken cancellationToken)
    {
        var project = await projects.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (project is null) return Result<ProjectResponse>.Failure("Project not found.");
        if (!ProjectStatusTransition.IsAllowed(project.Status, request.NewStatus))
            return Result<ProjectResponse>.Failure($"Cannot transition project from {project.Status} to {request.NewStatus}.");

        var previous = project.Status;
        project.Status = request.NewStatus;
        project.UpdatedAtUtc = DateTime.UtcNow;
        projects.Update(project);
        var timelineType = request.NewStatus switch
        {
            ProjectStatus.Approved => TimelineEventType.Approved,
            ProjectStatus.Completed => TimelineEventType.ProjectCompleted,
            ProjectStatus.Archived => TimelineEventType.ProjectArchived,
            ProjectStatus.AwaitingApproval => TimelineEventType.ApprovalRequested,
            _ => TimelineEventType.PlanningStarted
        };
        await RecordAsync(project.Id, timelineType, ActivityType.StatusChanged, $"Project status changed from {previous} to {request.NewStatus}.", "Project", project.Id, cancellationToken, previous.ToString(), request.NewStatus.ToString());
        if (request.NewStatus is ProjectStatus.Approved or ProjectStatus.Completed)
            await NotifyManagerAsync(project.ProjectManagerEmployeeId, "Project status updated", $"Project {project.ProjectName} is now {request.NewStatus}.", cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<ProjectResponse>.Success(ToResponse(project));
    }

    public async Task<Result<ProjectResponse>> Handle(DuplicateProjectCommand request, CancellationToken cancellationToken)
    {
        var source = await projects.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (source is null) return Result<ProjectResponse>.Failure("Project not found.");
        var code = string.IsNullOrWhiteSpace(request.ProjectCode) ? $"{source.ProjectCode}-COPY" : request.ProjectCode.Trim();
        var suffix = 2;
        while (await projects.ExistsByCodeAsync(source.OrganizationId, code, cancellationToken))
        {
            var prefix = source.ProjectCode[..Math.Min(source.ProjectCode.Length, 43)];
            code = $"{prefix}-COPY{suffix++}";
        }

        var copy = new Project
        {
            OrganizationId = source.OrganizationId,
            CategoryId = source.CategoryId,
            BranchId = source.BranchId,
            DepartmentId = source.DepartmentId,
            ProjectCode = code,
            ProjectName = $"{source.ProjectName} (Copy)",
            Description = source.Description,
            Notes = source.Notes,
            InternalRemarks = source.InternalRemarks,
            Priority = source.Priority,
            Status = ProjectStatus.Draft
        };
        await projects.AddAsync(copy, cancellationToken);
        var sections = await parameters.GetSectionsAsync(source.Id, cancellationToken);
        foreach (var section in sections)
        {
            var copiedSection = new ProjectParameterSection { ProjectId = copy.Id, Name = section.Name, DisplayOrder = section.DisplayOrder };
            await parameters.AddSectionAsync(copiedSection, cancellationToken);
            foreach (var parameter in section.Parameters)
                await parameters.AddParameterAsync(new ProjectParameter { SectionId = copiedSection.Id, ParameterName = parameter.ParameterName, InputType = parameter.InputType, Unit = parameter.Unit, IsRequired = parameter.IsRequired, DisplayOrder = parameter.DisplayOrder, DropdownOptionsJson = parameter.DropdownOptionsJson }, cancellationToken);
        }
        await RecordAsync(copy.Id, TimelineEventType.ProjectCreated, ActivityType.ProjectCreated, $"Project duplicated from {source.ProjectCode}.", "Project", copy.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<ProjectResponse>.Success(ToResponse(copy));
    }

    public async Task<Result<ProjectResponse>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await projects.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        return project is null ? Result<ProjectResponse>.Failure("Project not found.") : Result<ProjectResponse>.Success(ToResponse(project));
    }

    public async Task<Result<PagedResponse<ProjectListItemResponse>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var page = await projects.GetPagedAsync(request, cancellationToken);
        return Result<PagedResponse<ProjectListItemResponse>>.Success(page.Map(ToListItemResponse));
    }

    public async Task<Result<DraftSessionResponse>> Handle(UpsertDraftSessionCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId) return Result<DraftSessionResponse>.Failure("Authenticated user is required.");
        var session = await drafts.GetByUserAsync(userId, request.OrganizationId, cancellationToken);
        if (session is null)
        {
            session = ProjectWorkspaceMapper.ToEntity(request);
            session.UserId = userId;
            session.LastSavedAtUtc = DateTime.UtcNow;
            await drafts.AddAsync(session, cancellationToken);
        }
        else
        {
            session.WizardStateJson = request.WizardStateJson;
            session.CurrentStep = request.CurrentStep;
            session.LastSavedAtUtc = DateTime.UtcNow;
            session.UpdatedAtUtc = DateTime.UtcNow;
            drafts.Update(session);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<DraftSessionResponse>.Success(ProjectWorkspaceMapper.ToResponse(session));
    }

    public async Task<Result<DraftSessionResponse>> Handle(GetDraftSessionQuery request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId) return Result<DraftSessionResponse>.Failure("Authenticated user is required.");
        var session = await drafts.GetByUserAsync(userId, request.OrganizationId, cancellationToken);
        return session is null ? Result<DraftSessionResponse>.Failure("Draft session not found.") : Result<DraftSessionResponse>.Success(ProjectWorkspaceMapper.ToResponse(session));
    }

    public async Task<Result> Handle(DeleteDraftSessionCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId) return Result.Failure("Authenticated user is required.");
        var session = await drafts.GetByUserAsync(userId, request.OrganizationId, cancellationToken);
        if (session is null) return Result.Success();
        drafts.Remove(session);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<TeamMemberResponse>> Handle(AddTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var project = await projects.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project is null) return Result<TeamMemberResponse>.Failure("Project not found.");
        var employee = await employees.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee is null || employee.OrganizationId != project.OrganizationId) return Result<TeamMemberResponse>.Failure("Employee not found for this project organization.");
        if (await teams.HasActiveAsync(request.ProjectId, request.EmployeeId, cancellationToken)) return Result<TeamMemberResponse>.Failure("Employee is already an active team member.");
        var member = ProjectWorkspaceMapper.ToEntity(request);
        member.SnapshotBranchId = employee.BranchId;
        member.SnapshotDepartmentId = employee.DepartmentId;
        await teams.AddAsync(member, cancellationToken);
        await RecordAsync(project.Id, TimelineEventType.TeamMemberAdded, ActivityType.EmployeeAdded, $"{employee.FirstName} {employee.LastName} joined the project team.", "ProjectTeamMember", member.Id, cancellationToken);
        await NotifyEmployeeAsync(employee, "Project team assignment", $"You were added to project {project.ProjectName}.", cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<TeamMemberResponse>.Success(ToResponse(member, employee));
    }

    public async Task<Result<TeamMemberResponse>> Handle(UpdateTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await teams.GetByIdAsync(request.Id, cancellationToken);
        if (member is null || member.ProjectId != request.ProjectId) return Result<TeamMemberResponse>.Failure("Project team member not found.");
        if (member.Status == TeamMemberStatus.Released)
        {
            member.Remarks = request.Remarks;
        }
        else
        {
            member.ProjectRole = request.ProjectRole;
            member.AllocationPercentage = request.AllocationPercentage;
            member.Remarks = request.Remarks;
        }
        member.UpdatedAtUtc = DateTime.UtcNow;
        teams.Update(member);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<TeamMemberResponse>.Success(ToResponse(member, member.Employee));
    }

    public async Task<Result<TeamMemberResponse>> Handle(ReleaseTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await teams.GetByIdAsync(request.Id, cancellationToken);
        if (member is null || member.ProjectId != request.ProjectId) return Result<TeamMemberResponse>.Failure("Project team member not found.");
        if (member.Status == TeamMemberStatus.Released) return Result<TeamMemberResponse>.Failure("Team member has already been released.");
        member.Status = TeamMemberStatus.Released;
        member.ReleasedDate = request.ReleasedDate;
        member.Remarks = request.Remarks ?? member.Remarks;
        member.UpdatedAtUtc = DateTime.UtcNow;
        teams.Update(member);
        await RecordAsync(request.ProjectId, TimelineEventType.TeamMemberReleased, ActivityType.EmployeeReleased, "Project team member released.", "ProjectTeamMember", member.Id, cancellationToken);
        if (member.Employee is not null) await NotifyEmployeeAsync(member.Employee, "Project team release", "You were released from a project team.", cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<TeamMemberResponse>.Success(ToResponse(member, member.Employee));
    }

    public async Task<Result> Handle(RemoveTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await teams.GetByIdAsync(request.Id, cancellationToken);
        if (member is null || member.ProjectId != request.ProjectId) return Result.Failure("Project team member not found.");
        member.IsDeleted = true;
        member.DeletedAtUtc = DateTime.UtcNow;
        teams.Remove(member);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<PagedResponse<TeamMemberResponse>>> Handle(GetTeamMembersQuery request, CancellationToken cancellationToken)
    {
        var page = await teams.GetPagedAsync(request.ProjectId, request.Status, request, cancellationToken);
        return Result<PagedResponse<TeamMemberResponse>>.Success(page.Map(x => ToResponse(x, x.Employee)));
    }

    public async Task<Result<AssetAllocationResponse>> Handle(AllocateAssetCommand request, CancellationToken cancellationToken)
    {
        var project = await projects.GetByIdWithDetailsAsync(request.ProjectId, cancellationToken);
        if (project is null) return Result<AssetAllocationResponse>.Failure("Project not found.");
        var asset = await assets.GetByIdAsync(request.AssetId, cancellationToken);
        if (asset is null || asset.OrganizationId != project.OrganizationId) return Result<AssetAllocationResponse>.Failure("Asset not found for this project organization.");
        if (asset.AssetStatus != AssetStatus.Available) return Result<AssetAllocationResponse>.Failure("Asset must be available before it can be allocated.");
        if (await allocations.HasActiveAsync(request.ProjectId, request.AssetId, cancellationToken)) return Result<AssetAllocationResponse>.Failure("Asset is already allocated to this project.");

        var allocation = ProjectWorkspaceMapper.ToEntity(request);
        allocation.Status = AllocationStatus.Active;
        asset.ProjectId = project.Id;
        asset.AssetStatus = AssetStatus.Assigned;
        asset.UpdatedAtUtc = DateTime.UtcNow;
        await allocations.AddAsync(allocation, cancellationToken);
        assets.Update(asset);
        await RecordAsync(project.Id, TimelineEventType.AssetAllocated, ActivityType.AssetAllocated, $"Asset {asset.AssetCode} allocated to project.", "ProjectAssetAllocation", allocation.Id, cancellationToken);
        await NotifyManagerAsync(project.ProjectManagerEmployeeId, "Asset allocated", $"Asset {asset.AssetCode} was allocated to {project.ProjectName}.", cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<AssetAllocationResponse>.Success(ToResponse(allocation, asset));
    }

    public async Task<Result<AssetAllocationResponse>> Handle(ReturnAssetCommand request, CancellationToken cancellationToken)
    {
        var allocation = await allocations.GetByIdAsync(request.Id, cancellationToken);
        if (allocation is null || allocation.ProjectId != request.ProjectId) return Result<AssetAllocationResponse>.Failure("Asset allocation not found.");
        if (allocation.Status == AllocationStatus.Returned) return Result<AssetAllocationResponse>.Failure("Asset allocation has already been fully returned.");
        if (request.ReturnedQuantity <= 0 || allocation.ReturnedQuantity + request.ReturnedQuantity > allocation.AllocatedQuantity)
            return Result<AssetAllocationResponse>.Failure("Returned quantity must not exceed allocated quantity.");
        var asset = allocation.Asset ?? await assets.GetByIdAsync(allocation.AssetId, cancellationToken);
        if (asset is null) return Result<AssetAllocationResponse>.Failure("Asset not found.");

        allocation.ReturnedQuantity += request.ReturnedQuantity;
        allocation.ReturnDate = request.ReturnDate;
        allocation.Remarks = request.Remarks ?? allocation.Remarks;
        allocation.Status = allocation.ReturnedQuantity == allocation.AllocatedQuantity ? AllocationStatus.Returned : AllocationStatus.PartiallyReturned;
        allocation.UpdatedAtUtc = DateTime.UtcNow;
        if (allocation.Status == AllocationStatus.Returned)
        {
            asset.ProjectId = null;
            asset.AssetStatus = request.IsAssetUsable ? AssetStatus.Available : AssetStatus.Damaged;
            asset.UpdatedAtUtc = DateTime.UtcNow;
            assets.Update(asset);
        }
        allocations.Update(allocation);
        await RecordAsync(request.ProjectId, TimelineEventType.AssetReturned, ActivityType.AssetReturned, $"Asset {asset.AssetCode} returned from project.", "ProjectAssetAllocation", allocation.Id, cancellationToken);
        var project = await projects.GetByIdWithDetailsAsync(request.ProjectId, cancellationToken);
        if (project is not null) await NotifyManagerAsync(project.ProjectManagerEmployeeId, "Asset returned", $"Asset {asset.AssetCode} was returned from {project.ProjectName}.", cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<AssetAllocationResponse>.Success(ToResponse(allocation, asset));
    }

    public async Task<Result<PagedResponse<AssetAllocationResponse>>> Handle(GetAssetAllocationsQuery request, CancellationToken cancellationToken)
    {
        var page = await allocations.GetPagedAsync(request.ProjectId, request.Status, request, cancellationToken);
        return Result<PagedResponse<AssetAllocationResponse>>.Success(page.Map(x => ToResponse(x, x.Asset)));
    }

    public async Task<Result<DocumentResponse>> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        var project = await projects.GetByIdWithDetailsAsync(request.ProjectId, cancellationToken);
        if (project is null) return Result<DocumentResponse>.Failure("Project not found.");
        if (!IsAllowedDocument(request.FileName)) return Result<DocumentResponse>.Failure("This document type is not allowed.");
        if (!TryReadBase64(request.FileContentBase64, out var content)) return Result<DocumentResponse>.Failure("Document content is not valid base64.");
        if (content.Length > 50 * 1024 * 1024) return Result<DocumentResponse>.Failure("Document size must not exceed 50 MB.");

        await using var stream = new MemoryStream(content);
        var reference = await fileStorage.SaveAsync(stream, request.FileName, request.ContentType, cancellationToken);
        var document = ProjectWorkspaceMapper.ToEntity(request);
        document.FileReference = reference;
        document.UploadedAtUtc = DateTime.UtcNow;
        document.Version = 1;
        await documents.AddAsync(document, cancellationToken);
        await RecordAsync(project.Id, TimelineEventType.DocumentUploaded, ActivityType.DocumentUploaded, $"Document {document.DocumentName} uploaded.", "ProjectDocument", document.Id, cancellationToken);
        await NotifyManagerAsync(project.ProjectManagerEmployeeId, "Project document uploaded", $"A document was uploaded to {project.ProjectName}.", cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<DocumentResponse>.Success(ToResponse(document, null));
    }

    public async Task<Result<DocumentResponse>> Handle(ReplaceDocumentCommand request, CancellationToken cancellationToken)
    {
        var previous = await documents.GetByIdAsync(request.Id, cancellationToken);
        if (previous is null || previous.ProjectId != request.ProjectId) return Result<DocumentResponse>.Failure("Project document not found.");
        if (!IsAllowedDocument(request.FileName)) return Result<DocumentResponse>.Failure("This document type is not allowed.");
        if (!TryReadBase64(request.FileContentBase64, out var content)) return Result<DocumentResponse>.Failure("Document content is not valid base64.");
        if (content.Length > 50 * 1024 * 1024) return Result<DocumentResponse>.Failure("Document size must not exceed 50 MB.");

        await using var stream = new MemoryStream(content);
        var reference = await fileStorage.SaveAsync(stream, request.FileName, request.ContentType, cancellationToken);
        var replacement = new ProjectDocument
        {
            ProjectId = previous.ProjectId,
            DocumentName = previous.DocumentName,
            Category = previous.Category,
            Description = previous.Description,
            FileReference = reference,
            UploadedAtUtc = DateTime.UtcNow,
            Version = previous.Version + 1,
            Remarks = request.Remarks ?? previous.Remarks,
            ExpiryDate = previous.ExpiryDate,
            UploadedByEmployeeId = previous.UploadedByEmployeeId
        };
        await documents.AddAsync(replacement, cancellationToken);
        await RecordAsync(request.ProjectId, TimelineEventType.DocumentReplaced, ActivityType.DocumentReplaced, $"Document {previous.DocumentName} replaced with version {replacement.Version}.", "ProjectDocument", replacement.Id, cancellationToken, previous.Version.ToString(), replacement.Version.ToString());
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<DocumentResponse>.Success(ToResponse(replacement, null));
    }

    public async Task<Result> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await documents.GetByIdAsync(request.Id, cancellationToken);
        if (document is null || document.ProjectId != request.ProjectId) return Result.Failure("Project document not found.");
        document.IsDeleted = true;
        document.DeletedAtUtc = DateTime.UtcNow;
        document.UpdatedAtUtc = DateTime.UtcNow;
        documents.Update(document);
        await RecordAsync(request.ProjectId, TimelineEventType.DocumentDeleted, ActivityType.DocumentDeleted, $"Document {document.DocumentName} deleted.", "ProjectDocument", document.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<PagedResponse<DocumentResponse>>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        var page = await documents.GetPagedAsync(request.ProjectId, request.Category, request.Search, request, cancellationToken);
        return Result<PagedResponse<DocumentResponse>>.Success(page.Map(x => ToResponse(x, x.UploadedByEmployee)));
    }

    public async Task<Result<IReadOnlyCollection<DocumentResponse>>> Handle(GetDocumentVersionHistoryQuery request, CancellationToken cancellationToken)
    {
        var values = await documents.GetVersionHistoryAsync(request.ProjectId, request.DocumentName, cancellationToken);
        return Result<IReadOnlyCollection<DocumentResponse>>.Success(values.Select(x => ToResponse(x, x.UploadedByEmployee)).ToList());
    }

    public async Task<Result<ParameterSectionResponse>> Handle(CreateParameterSectionCommand request, CancellationToken cancellationToken)
    {
        if (await projects.GetByIdAsync(request.ProjectId, cancellationToken) is null) return Result<ParameterSectionResponse>.Failure("Project not found.");
        var section = ProjectWorkspaceMapper.ToEntity(request);
        await parameters.AddSectionAsync(section, cancellationToken);
        await RecordAsync(request.ProjectId, null, ActivityType.SectionCreated, $"Parameter section {section.Name} created.", "ProjectParameterSection", section.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<ParameterSectionResponse>.Success(ToResponse(section, []));
    }

    public async Task<Result<ParameterSectionResponse>> Handle(UpdateParameterSectionCommand request, CancellationToken cancellationToken)
    {
        var section = await parameters.GetSectionByIdAsync(request.Id, cancellationToken);
        if (section is null || section.ProjectId != request.ProjectId) return Result<ParameterSectionResponse>.Failure("Parameter section not found.");
        section.Name = request.Name;
        section.DisplayOrder = request.DisplayOrder;
        section.UpdatedAtUtc = DateTime.UtcNow;
        parameters.UpdateSection(section);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<ParameterSectionResponse>.Success(ToResponse(section, section.Parameters.Select(ToResponse).ToList()));
    }

    public async Task<Result> Handle(DeleteParameterSectionCommand request, CancellationToken cancellationToken)
    {
        var section = await parameters.GetSectionByIdAsync(request.Id, cancellationToken);
        if (section is null || section.ProjectId != request.ProjectId) return Result.Failure("Parameter section not found.");
        section.IsDeleted = true;
        section.DeletedAtUtc = DateTime.UtcNow;
        foreach (var parameter in section.Parameters)
        {
            parameter.IsDeleted = true;
            parameter.DeletedAtUtc = DateTime.UtcNow;
            parameters.RemoveParameter(parameter);
        }
        parameters.RemoveSection(section);
        await RecordAsync(request.ProjectId, null, ActivityType.SectionDeleted, $"Parameter section {section.Name} deleted.", "ProjectParameterSection", section.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<ParameterResponse>> Handle(AddParameterCommand request, CancellationToken cancellationToken)
    {
        var section = await parameters.GetSectionByIdAsync(request.SectionId, cancellationToken);
        if (section is null || section.ProjectId != request.ProjectId) return Result<ParameterResponse>.Failure("Parameter section not found.");
        var parameter = ProjectWorkspaceMapper.ToEntity(request);
        await parameters.AddParameterAsync(parameter, cancellationToken);
        await RecordAsync(request.ProjectId, TimelineEventType.ParameterCreated, ActivityType.ParameterCreated, $"Parameter {parameter.ParameterName} added.", "ProjectParameter", parameter.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<ParameterResponse>.Success(ToResponse(parameter));
    }

    public async Task<Result<ParameterResponse>> Handle(UpdateParameterCommand request, CancellationToken cancellationToken)
    {
        var parameter = await parameters.GetParameterByIdAsync(request.Id, cancellationToken);
        if (parameter is null || parameter.SectionId != request.SectionId || parameter.Section.ProjectId != request.ProjectId) return Result<ParameterResponse>.Failure("Project parameter not found.");
        parameter.ParameterName = request.ParameterName;
        parameter.InputType = request.InputType;
        parameter.Unit = request.Unit;
        parameter.IsRequired = request.IsRequired;
        parameter.DisplayOrder = request.DisplayOrder;
        parameter.DropdownOptionsJson = request.DropdownOptionsJson;
        parameter.UpdatedAtUtc = DateTime.UtcNow;
        parameters.UpdateParameter(parameter);
        await RecordAsync(request.ProjectId, TimelineEventType.ParameterUpdated, ActivityType.ParameterUpdated, $"Parameter {parameter.ParameterName} updated.", "ProjectParameter", parameter.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<ParameterResponse>.Success(ToResponse(parameter));
    }

    public async Task<Result> Handle(DeleteParameterCommand request, CancellationToken cancellationToken)
    {
        var parameter = await parameters.GetParameterByIdAsync(request.Id, cancellationToken);
        if (parameter is null || parameter.SectionId != request.SectionId || parameter.Section.ProjectId != request.ProjectId) return Result.Failure("Project parameter not found.");
        parameter.IsDeleted = true;
        parameter.DeletedAtUtc = DateTime.UtcNow;
        parameter.UpdatedAtUtc = DateTime.UtcNow;
        parameters.RemoveParameter(parameter);
        await RecordAsync(request.ProjectId, TimelineEventType.ParameterDeleted, ActivityType.ParameterDeleted, $"Parameter {parameter.ParameterName} deleted.", "ProjectParameter", parameter.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> Handle(SaveParameterValuesCommand request, CancellationToken cancellationToken)
    {
        if (await projects.GetByIdAsync(request.ProjectId, cancellationToken) is null) return Result.Failure("Project not found.");
        foreach (var input in request.Values)
        {
            var parameter = await parameters.GetParameterByIdAsync(input.ParameterId, cancellationToken);
            if (parameter is null || parameter.Section.ProjectId != request.ProjectId) return Result.Failure("One or more parameters do not belong to this project.");
            if (parameter.IsRequired && string.IsNullOrWhiteSpace(input.Value)) return Result.Failure($"{parameter.ParameterName} is required.");
            await parameters.UpsertValueAsync(new ProjectParameterValue { ProjectId = request.ProjectId, ParameterId = input.ParameterId, Value = input.Value }, cancellationToken);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<IReadOnlyCollection<ParameterSectionResponse>>> Handle(GetParameterSectionsQuery request, CancellationToken cancellationToken)
    {
        var sections = await parameters.GetSectionsAsync(request.ProjectId, cancellationToken);
        var values = await parameters.GetValuesByProjectAsync(request.ProjectId, cancellationToken);
        var byParameter = values.ToDictionary(x => x.ParameterId, x => x.Value);
        var result = sections.Select(section => ToResponse(section, section.Parameters.OrderBy(x => x.DisplayOrder).Select(parameter => ToResponse(parameter) with { Value = byParameter.GetValueOrDefault(parameter.Id) }).ToList())).ToList();
        return Result<IReadOnlyCollection<ParameterSectionResponse>>.Success(result);
    }

    private async Task<string?> ValidateProjectReferencesAsync(Guid organizationId, Guid categoryId, Guid? customerId, Guid? branchId, Guid? departmentId, Guid? managerId, CancellationToken cancellationToken)
    {
        if (await organizations.GetByIdAsync(organizationId, cancellationToken) is null) return "Organization not found.";
        var category = await categories.GetByIdAsync(categoryId, cancellationToken);
        if (category is null || category.OrganizationId != organizationId) return "Project category not found for this organization.";
        if (customerId.HasValue)
        {
            var customer = await operations.GetByIdAsync<Customer>(customerId.Value, cancellationToken);
            if (customer is null || customer.OrganizationId != organizationId) return "Customer not found for this organization.";
        }
        if (branchId.HasValue)
        {
            var branch = await branches.GetByIdAsync(branchId.Value, cancellationToken);
            if (branch is null || branch.OrganizationId != organizationId) return "Branch not found for this organization.";
        }
        if (departmentId.HasValue)
        {
            var department = await departments.GetByIdAsync(departmentId.Value, cancellationToken);
            if (department is null || department.OrganizationId != organizationId) return "Department not found for this organization.";
        }
        if (managerId.HasValue)
        {
            var manager = await employees.GetByIdAsync(managerId.Value, cancellationToken);
            if (manager is null || manager.OrganizationId != organizationId) return "Project manager not found for this organization.";
        }
        return null;
    }

    private async Task RecordAsync(Guid projectId, TimelineEventType? timelineType, ActivityType activityType, string action, string entityType, Guid? entityId, CancellationToken cancellationToken, string? oldValue = null, string? newValue = null)
    {
        if (timelineType.HasValue)
            await timeline.AddAsync(new ProjectTimelineEvent { ProjectId = projectId, EventType = timelineType.Value, EntityType = entityType, EntityId = entityId, Description = action, Timestamp = DateTime.UtcNow, UserId = currentUser.UserId, IconType = timelineType.Value.ToString() }, cancellationToken);
        await activities.AddAsync(new ProjectActivityRecord { ProjectId = projectId, ActivityType = activityType, Action = action, TargetEntity = entityType, TargetEntityId = entityId, Timestamp = DateTime.UtcNow, UserId = currentUser.UserId }, cancellationToken);
        await operations.AddAsync(new AuditLog { UserId = currentUser.UserId, EntityName = entityType, EntityId = entityId, Action = action, OldValues = oldValue, NewValues = newValue }, cancellationToken);
    }

    private async Task NotifyManagerAsync(Guid? employeeId, string title, string message, CancellationToken cancellationToken)
    {
        if (!employeeId.HasValue) return;
        var employee = await employees.GetByIdAsync(employeeId.Value, cancellationToken);
        if (employee is not null) await NotifyEmployeeAsync(employee, title, message, cancellationToken);
    }

    private Task NotifyEmployeeAsync(Employee employee, string title, string message, CancellationToken cancellationToken) =>
        employee.IdentityUserId.HasValue
            ? operations.AddAsync(new Notification { UserId = employee.IdentityUserId, Title = title, Message = message, NotificationType = NotificationType.Info }, cancellationToken)
            : Task.CompletedTask;

    private static bool TryReadBase64(string value, out byte[] content)
    {
        try { content = Convert.FromBase64String(value); return true; }
        catch (FormatException) { content = []; return false; }
    }

    private static bool IsAllowedDocument(string fileName) => new[] { ".pdf", ".png", ".jpg", ".jpeg", ".doc", ".docx", ".xls", ".xlsx", ".txt" }.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase);

    private static ProjectResponse ToResponse(Project project) => ProjectWorkspaceMapper.ToResponse(project) with
    {
        CustomerName = project.Customer?.Name,
        CategoryName = project.Category?.Name,
        BranchName = project.Branch?.Name,
        DepartmentName = project.Department?.Name,
        ProjectManagerName = project.ProjectManager is null ? null : $"{project.ProjectManager.FirstName} {project.ProjectManager.LastName}"
    };

    private static ProjectListItemResponse ToListItemResponse(Project project) => ProjectWorkspaceMapper.ToListItemResponse(project) with
    {
        ProjectManagerName = project.ProjectManager is null ? null : $"{project.ProjectManager.FirstName} {project.ProjectManager.LastName}",
        CategoryName = project.Category?.Name
    };

    private static TeamMemberResponse ToResponse(ProjectTeamMember member, Employee? employee) => ProjectWorkspaceMapper.ToResponse(member) with { EmployeeName = employee is null ? null : $"{employee.FirstName} {employee.LastName}" };
    private static AssetAllocationResponse ToResponse(ProjectAssetAllocation allocation, Asset? asset) => ProjectWorkspaceMapper.ToResponse(allocation) with { AssetCode = asset?.AssetCode, AssetName = asset?.AssetName };
    private static DocumentResponse ToResponse(ProjectDocument document, Employee? employee) => ProjectWorkspaceMapper.ToResponse(document) with { UploadedByEmployeeName = employee is null ? null : $"{employee.FirstName} {employee.LastName}" };
    private static ParameterResponse ToResponse(ProjectParameter parameter) => ProjectWorkspaceMapper.ToResponse(parameter);
    private static ParameterSectionResponse ToResponse(ProjectParameterSection section, IReadOnlyList<ParameterResponse> values) => ProjectWorkspaceMapper.ToResponse(section) with { Parameters = values };
}
