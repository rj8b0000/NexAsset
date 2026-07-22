using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Projects;

public sealed class ProjectWorkspaceSearchHandlers(
    IProjectRepository projects,
    IProjectParameterRepository parameters,
    IProjectDocumentRepository documents,
    IProjectTeamRepository teams,
    IProjectAssetRepository allocations,
    IProjectRiskRepository risks,
    IProjectActivityRepository activities,
    ISavedFilterRepository filters,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork)
    : IRequestHandler<GlobalSearchQuery, Result<GlobalSearchResponse>>,
      IRequestHandler<SaveFilterCommand, Result<SavedFilterResponse>>,
      IRequestHandler<GetSavedFiltersQuery, Result<IReadOnlyCollection<SavedFilterResponse>>>,
      IRequestHandler<DeleteSavedFilterCommand, Result>
{
    public async Task<Result<GlobalSearchResponse>> Handle(GlobalSearchQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Keyword))
            return Result<GlobalSearchResponse>.Success(new GlobalSearchResponse([], 0, Math.Max(1, request.PageNumber), Math.Clamp(request.PageSize, 1, 200)));

        var keyword = request.Keyword.Trim();
        var projectPage = await projects.GetPagedAsync(new GetProjectsQuery { OrganizationId = request.OrganizationId, Search = keyword, PageNumber = 1, PageSize = 200 }, cancellationToken);
        var allProjects = await projects.GetPagedAsync(new GetProjectsQuery { OrganizationId = request.OrganizationId, PageNumber = 1, PageSize = 200 }, cancellationToken);
        var groups = new Dictionary<string, List<SearchResultItem>>(StringComparer.Ordinal);

        foreach (var project in projectPage.Items)
            Add(groups, "Projects", new SearchResultItem(project.Id, $"{project.ProjectCode} - {project.ProjectName}", project.Description ?? project.ProjectName, $"/projects/{project.Id}"));

        foreach (var project in allProjects.Items)
        {
            var sections = await parameters.GetSectionsAsync(project.Id, cancellationToken);
            var values = await parameters.GetValuesByProjectAsync(project.Id, cancellationToken);
            foreach (var parameter in sections.SelectMany(x => x.Parameters).Where(x => Contains(x.ParameterName, keyword)))
                Add(groups, "Project Parameters", new SearchResultItem(parameter.Id, parameter.ParameterName, $"Parameter in {project.ProjectName}", $"/projects/{project.Id}?tab=parameters"));
            foreach (var value in values.Where(x => Contains(x.Value, keyword)))
                Add(groups, "Parameter Values", new SearchResultItem(value.Id, value.Value ?? string.Empty, $"Value in {project.ProjectName}", $"/projects/{project.Id}?tab=parameters"));
            foreach (var document in (await documents.GetAllAsync(project.Id, cancellationToken)).Where(x => Contains(x.DocumentName, keyword) || Contains(x.Description, keyword) || Contains(x.Remarks, keyword)))
                Add(groups, "Documents", new SearchResultItem(document.Id, document.DocumentName, document.Description ?? document.Remarks ?? project.ProjectName, $"/projects/{project.Id}?tab=documents"));
            foreach (var member in (await teams.GetAllAsync(project.Id, cancellationToken)).Where(x => Contains($"{x.Employee.FirstName} {x.Employee.LastName}", keyword) || Contains(x.ProjectRole, keyword)))
                Add(groups, "Team Members", new SearchResultItem(member.Id, $"{member.Employee.FirstName} {member.Employee.LastName}", member.ProjectRole, $"/projects/{project.Id}?tab=team"));
            foreach (var allocation in (await allocations.GetAllAsync(project.Id, cancellationToken)).Where(x => Contains(x.Asset.AssetCode, keyword) || Contains(x.Asset.AssetName, keyword)))
                Add(groups, "Asset Allocations", new SearchResultItem(allocation.Id, $"{allocation.Asset.AssetCode} - {allocation.Asset.AssetName}", allocation.Status.ToString(), $"/projects/{project.Id}?tab=assets"));
            foreach (var risk in (await risks.GetAllAsync(project.Id, cancellationToken)).Where(x => Contains(x.Title, keyword) || Contains(x.Description, keyword) || Contains(x.MitigationPlan, keyword)))
                Add(groups, "Risks", new SearchResultItem(risk.Id, risk.Title, risk.Description ?? risk.Severity.ToString(), $"/projects/{project.Id}?tab=risks"));
            foreach (var activity in (await activities.GetRecentAsync(project.Id, 200, cancellationToken)).Where(x => Contains(x.Action, keyword) || Contains(x.Remarks, keyword)))
                Add(groups, "Activities", new SearchResultItem(activity.Id, activity.Action, activity.Timestamp.ToString("u"), $"/projects/{project.Id}?tab=activity"));
        }

        var items = groups.Select(x => new SearchResultGroup(x.Key, x.Value.Count, x.Value)).ToList();
        var total = items.Sum(x => x.MatchCount);
        return Result<GlobalSearchResponse>.Success(new GlobalSearchResponse(items, total, Math.Max(1, request.PageNumber), Math.Clamp(request.PageSize, 1, 200)));
    }

    public async Task<Result<SavedFilterResponse>> Handle(SaveFilterCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId) return Result<SavedFilterResponse>.Failure("Authenticated user is required.");
        var filter = ProjectWorkspaceMapper.ToEntity(request);
        filter.UserId = userId;
        await filters.AddAsync(filter, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<SavedFilterResponse>.Success(ProjectWorkspaceMapper.ToResponse(filter));
    }

    public async Task<Result<IReadOnlyCollection<SavedFilterResponse>>> Handle(GetSavedFiltersQuery request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId) return Result<IReadOnlyCollection<SavedFilterResponse>>.Failure("Authenticated user is required.");
        var values = await filters.GetByUserAsync(userId, request.OrganizationId, request.EntityType, cancellationToken);
        return Result<IReadOnlyCollection<SavedFilterResponse>>.Success(values.Select(ProjectWorkspaceMapper.ToResponse).ToList());
    }

    public async Task<Result> Handle(DeleteSavedFilterCommand request, CancellationToken cancellationToken)
    {
        var filter = await filters.GetByIdAsync(request.Id, cancellationToken);
        if (filter is null) return Result.Failure("Saved filter not found.");
        if (currentUser.UserId != filter.UserId) return Result.Failure("Saved filter not found.");
        filters.Remove(filter);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private static bool Contains(string? value, string keyword) => value?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true;
    private static void Add(IDictionary<string, List<SearchResultItem>> groups, string type, SearchResultItem item)
    {
        if (!groups.TryGetValue(type, out var list)) groups[type] = list = [];
        list.Add(item);
    }
}
