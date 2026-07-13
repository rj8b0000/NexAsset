using Microsoft.EntityFrameworkCore;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Models.Paging;
using NexAsset.Domain.Entities;
using NexAsset.Infrastructure.Persistence;

namespace NexAsset.Infrastructure.Persistence.Repositories;

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByCodeAsync(
        Guid organizationId,
        string employeeCode,
        CancellationToken cancellationToken)
    {
        return await _context.Employees.AnyAsync(
            x => x.OrganizationId == organizationId &&
                 x.EmployeeCode == employeeCode &&
                 !x.IsDeleted,
            cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(
        Guid organizationId,
        string employeeCode,
        Guid excludeId,
        CancellationToken cancellationToken)
    {
        return await _context.Employees.AnyAsync(
            x => x.OrganizationId == organizationId &&
                 x.EmployeeCode == employeeCode &&
                 x.Id != excludeId &&
                 !x.IsDeleted,
            cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        return await _context.Employees.AnyAsync(
            x => x.Email == email && !x.IsDeleted,
            cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(
        string email,
        Guid excludeId,
        CancellationToken cancellationToken)
    {
        return await _context.Employees.AnyAsync(
            x => x.Email == email &&
                 x.Id != excludeId &&
                 !x.IsDeleted,
            cancellationToken);
    }

    public async Task AddAsync(
        Employee employee,
        CancellationToken cancellationToken)
    {
        await _context.Employees.AddAsync(employee, cancellationToken);
    }

    public async Task<Employee?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<PagedResponse<Employee>> GetPagedAsync(
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        IQueryable<Employee> queryable = _context.Employees
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            queryable = queryable.Where(x =>
                x.EmployeeCode.ToLower().Contains(search) ||
                x.FirstName.ToLower().Contains(search) ||
                x.LastName.ToLower().Contains(search) ||
                x.Email.ToLower().Contains(search) ||
                (x.Phone != null && x.Phone.ToLower().Contains(search)));
        }

        queryable = request.SortBy?.ToLower() switch
        {
            "employeecode" or "code" => request.Descending
                ? queryable.OrderByDescending(x => x.EmployeeCode)
                : queryable.OrderBy(x => x.EmployeeCode),
            "firstname" => request.Descending
                ? queryable.OrderByDescending(x => x.FirstName)
                : queryable.OrderBy(x => x.FirstName),
            "lastname" => request.Descending
                ? queryable.OrderByDescending(x => x.LastName)
                : queryable.OrderBy(x => x.LastName),
            "joiningdate" => request.Descending
                ? queryable.OrderByDescending(x => x.JoiningDate)
                : queryable.OrderBy(x => x.JoiningDate),
            "createdat" or "createdatutc" => request.Descending
                ? queryable.OrderByDescending(x => x.CreatedAtUtc)
                : queryable.OrderBy(x => x.CreatedAtUtc),
            _ => request.Descending
                ? queryable.OrderByDescending(x => x.FirstName)
                : queryable.OrderBy(x => x.FirstName)
        };

        var totalCount = await queryable.CountAsync(cancellationToken);

        var items = await queryable
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResponse<Employee>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public void Update(Employee employee)
    {
        _context.Employees.Update(employee);
    }
}
