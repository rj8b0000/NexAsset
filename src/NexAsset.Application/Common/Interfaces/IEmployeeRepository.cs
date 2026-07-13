using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Common.Interfaces;

public interface IEmployeeRepository
{
    Task<bool> ExistsByCodeAsync(
        Guid organizationId,
        string employeeCode,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        Guid organizationId,
        string employeeCode,
        Guid excludeId,
        CancellationToken cancellationToken);

    Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task<bool> ExistsByEmailAsync(
        string email,
        Guid excludeId,
        CancellationToken cancellationToken);

    Task AddAsync(
        Employee employee,
        CancellationToken cancellationToken);

    Task<Employee?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResponse<Employee>> GetPagedAsync(
        PagedRequest request,
        CancellationToken cancellationToken);

    void Update(Employee employee);
}
