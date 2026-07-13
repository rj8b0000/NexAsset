using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Branches.Commands.DeleteBranch;

public sealed class DeleteBranchCommandHandler
    : IRequestHandler<DeleteBranchCommand, Result>
{
    private readonly IBranchRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBranchCommandHandler(
        IBranchRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteBranchCommand request,
        CancellationToken cancellationToken)
    {
        var branch = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (branch is null)
        {
            return Result.Failure("Branch not found.");
        }

        branch.IsDeleted = true;
        branch.DeletedAtUtc = DateTime.UtcNow;

        _repository.Update(branch);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
