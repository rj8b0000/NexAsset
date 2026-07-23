using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectParameters.Commands.DeleteParameter;

public sealed class DeleteParameterCommandHandler : IRequestHandler<DeleteParameterCommand, Result>
{
    private readonly IProjectParameterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteParameterCommandHandler(IProjectParameterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteParameterCommand request, CancellationToken cancellationToken)
    {
        var param = await _repository.GetParameterByIdAsync(request.Id, cancellationToken);
        if (param == null)
        {
            return Result.Failure("Parameter not found.");
        }

        _repository.RemoveParameter(param);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
