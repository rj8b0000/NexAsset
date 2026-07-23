using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IProjectParameterRepository
{
    Task AddSectionAsync(ProjectParameterSection section, CancellationToken cancellationToken);
    Task<ProjectParameterSection?> GetSectionByIdAsync(Guid sectionId, CancellationToken cancellationToken);
    Task<List<ProjectParameterSection>> GetSectionsByProjectAsync(Guid projectId, CancellationToken cancellationToken);
    Task AddParameterAsync(ProjectParameter parameter, CancellationToken cancellationToken);
    Task<ProjectParameter?> GetParameterByIdAsync(Guid parameterId, CancellationToken cancellationToken);
    Task<List<ProjectParameterValue>> GetValuesByProjectAsync(Guid projectId, CancellationToken cancellationToken);
    Task UpsertValueAsync(ProjectParameterValue value, CancellationToken cancellationToken);
    void UpdateSection(ProjectParameterSection section);
    void UpdateParameter(ProjectParameter parameter);
    void RemoveSection(ProjectParameterSection section);
    void RemoveParameter(ProjectParameter parameter);
}
