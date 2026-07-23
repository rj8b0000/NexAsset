using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectParameters.Commands.DeleteParameterSection;

public sealed class DeleteParameterSectionCommandHandler : IRequestHandler<DeleteParameterSectionCommand, Result>
{
    private readonly IProjectParameterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteParameterSectionCommandHandler(IProjectParameterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteParameterSectionCommand request, CancellationToken cancellationToken)
    {
        var section = await _repository.GetSectionByIdAsync(request.Id, cancellationToken);
        if (section == null)
        {
            return Result.Failure("Parameter section not found.");
        }

        _repository.RemoveSection(section);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
