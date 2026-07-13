using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Employees.Commands.DeleteEmployee;

public sealed class DeleteEmployeeCommandHandler
    : IRequestHandler<DeleteEmployeeCommand, Result>
{
    private readonly IEmployeeRepository _repository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEmployeeCommandHandler(
        IEmployeeRepository repository,
        IIdentityService identityService,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _identityService = identityService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employee is null)
            return Result.Failure("Employee not found.");

        employee.IsDeleted = true;
        employee.IsActive = false;
        employee.DeletedAtUtc = DateTime.UtcNow;

        if (employee.IdentityUserId.HasValue)
        {
            var identityResult = await _identityService.SetUserActiveAsync(
                employee.IdentityUserId.Value,
                false,
                cancellationToken);

            if (identityResult.IsFailure)
                return identityResult;
        }

        _repository.Update(employee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
