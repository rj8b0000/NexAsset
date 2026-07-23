using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Features.ProjectParameters.Commands.SaveParameterValues;

public sealed class SaveParameterValuesCommandHandler : IRequestHandler<SaveParameterValuesCommand, Result>
{
    private readonly IProjectParameterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SaveParameterValuesCommandHandler(IProjectParameterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SaveParameterValuesCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.Values)
        {
            var valueEntity = new ProjectParameterValue
            {
                ProjectId = request.ProjectId,
                ParameterId = item.ParameterId,
                Value = item.Value
            };
            await _repository.UpsertValueAsync(valueEntity, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
