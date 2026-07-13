using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Designations.Commands.DeleteDesignation;

public sealed class DeleteDesignationCommandHandler
    : IRequestHandler<DeleteDesignationCommand, Result>
{
    private readonly IDesignationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDesignationCommandHandler(
        IDesignationRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteDesignationCommand request,
        CancellationToken cancellationToken)
    {
        var designation = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (designation is null)
        {
            return Result.Failure("Designation not found.");
        }

        designation.IsDeleted = true;
        designation.DeletedAtUtc = DateTime.UtcNow;

        _repository.Update(designation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
