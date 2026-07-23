using NexAsset.Domain.Enums;

namespace NexAsset.Application.Features.Projects.Queries.GetProjects;

public sealed record ProjectResponse(
    Guid Id,
    Guid OrganizationId,
    string ProjectCode,
    string ProjectName,
    string? Description,
    string? Notes,
    string? InternalRemarks,
    ProjectStatus Status,
    ProjectPriority Priority,
    DateOnly? StartDate,
    DateOnly? EndDate,
    DateOnly? ExpectedCompletionDate,
    Guid CategoryId,
    string CategoryName,
    Guid? CustomerId,
    string? CustomerName,
    Guid? BranchId,
    string? BranchName,
    Guid? DepartmentId,
    string? DepartmentName,
    Guid? ProjectManagerEmployeeId,
    string? ProjectManagerName,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public sealed record ProjectListItemResponse(
    Guid Id,
    string ProjectCode,
    string ProjectName,
    ProjectStatus Status,
    ProjectPriority Priority,
    string CategoryName,
    string? ProjectManagerName,
    DateOnly? StartDate,
    DateOnly? EndDate,
    DateOnly? ExpectedCompletionDate,
    DateTime CreatedAtUtc);
