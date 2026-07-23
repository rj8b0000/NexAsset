using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Repository;

public sealed class ProjectParameterRepository : IProjectParameterRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectParameterRepository(ApplicationDbContext context) => _context = context;

    public async Task AddSectionAsync(ProjectParameterSection section, CancellationToken cancellationToken) =>
        await _context.ProjectParameterSections.AddAsync(section, cancellationToken);

    public Task<ProjectParameterSection?> GetSectionByIdAsync(Guid sectionId, CancellationToken cancellationToken) =>
        _context.ProjectParameterSections
            .Include(x => x.Parameters)
            .FirstOrDefaultAsync(x => x.Id == sectionId && !x.IsDeleted, cancellationToken);

    public Task<List<ProjectParameterSection>> GetSectionsByProjectAsync(Guid projectId, CancellationToken cancellationToken) =>
        _context.ProjectParameterSections
            .Include(x => x.Parameters.Where(p => !p.IsDeleted))
            .Where(x => x.ProjectId == projectId && !x.IsDeleted)
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);

    public async Task AddParameterAsync(ProjectParameter parameter, CancellationToken cancellationToken) =>
        await _context.ProjectParameters.AddAsync(parameter, cancellationToken);

    public Task<ProjectParameter?> GetParameterByIdAsync(Guid parameterId, CancellationToken cancellationToken) =>
        _context.ProjectParameters.FirstOrDefaultAsync(x => x.Id == parameterId && !x.IsDeleted, cancellationToken);

    public Task<List<ProjectParameterValue>> GetValuesByProjectAsync(Guid projectId, CancellationToken cancellationToken) =>
        _context.ProjectParameterValues
            .Include(x => x.Parameter)
            .Where(x => x.ProjectId == projectId && !x.IsDeleted)
            .ToListAsync(cancellationToken);

    public async Task UpsertValueAsync(ProjectParameterValue value, CancellationToken cancellationToken)
    {
        var existing = await _context.ProjectParameterValues
            .FirstOrDefaultAsync(x => x.ProjectId == value.ProjectId && x.ParameterId == value.ParameterId && !x.IsDeleted, cancellationToken);

        if (existing != null)
        {
            existing.Value = value.Value;
            existing.UpdatedAtUtc = DateTime.UtcNow;
            _context.ProjectParameterValues.Update(existing);
        }
        else
        {
            await _context.ProjectParameterValues.AddAsync(value, cancellationToken);
        }
    }

    public void UpdateSection(ProjectParameterSection section) => _context.ProjectParameterSections.Update(section);

    public void UpdateParameter(ProjectParameter parameter) => _context.ProjectParameters.Update(parameter);

    public void RemoveSection(ProjectParameterSection section)
    {
        section.IsDeleted = true;
        section.DeletedAtUtc = DateTime.UtcNow;
        _context.ProjectParameterSections.Update(section);
    }

    public void RemoveParameter(ProjectParameter parameter)
    {
        parameter.IsDeleted = true;
        parameter.DeletedAtUtc = DateTime.UtcNow;
        _context.ProjectParameters.Update(parameter);
    }
}
