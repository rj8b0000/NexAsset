using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IOrganizationRepository
{
    Task<bool> ExistsByCodeAsync(
        string code,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        string code,
        Guid excludeId,
        CancellationToken cancellationToken);

    Task AddAsync(
        Organization organization,
        CancellationToken cancellationToken);

    Task<Organization?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<List<Organization>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<PagedResponse<Organization>> GetPagedAsync(
        PagedRequest request,
        CancellationToken cancellationToken);

    void Update(Organization organization);

    void Delete(Organization organization);

    /// <summary>
    /// Soft-deletes every record that belongs to the organization (branches, departments,
    /// designations, employees, asset categories, assets and their lifecycle records,
    /// vendors, purchase requests/orders, inventory items with movements and consumables,
    /// maintenance records, customers, service tickets, and org-scoped settings), so a
    /// deleted organization leaves no orphaned data behind.
    /// </summary>
    Task CascadeSoftDeleteAsync(
        Guid organizationId,
        CancellationToken cancellationToken);
}