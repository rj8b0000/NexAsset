using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectParameters.Commands.UpdateParameterSection;

public sealed class UpdateParameterSectionCommandHandler : IRequestHandler<UpdateParameterSectionCommand, Result>
{
    private readonly IProjectParameterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateParameterSectionCommandHandler(IProjectParameterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateParameterSectionCommand request, CancellationToken cancellationToken)
    {
        var section = await _repository.GetSectionByIdAsync(request.Id, cancellationToken);
        if (section == null)
        {
            return Result.Failure("Parameter section not found.");
        }

        section.Name = request.Name;
        section.DisplayOrder = request.DisplayOrder;
        section.UpdatedAtUtc = DateTime.UtcNow;

        _repository.UpdateSection(section);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
