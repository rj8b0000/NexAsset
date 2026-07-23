using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectParameters.Queries.GetParameterSections;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Features.ProjectParameters.Commands.AddParameter;

public sealed class AddParameterCommandHandler : IRequestHandler<AddParameterCommand, Result<ParameterResponse>>
{
    private readonly IProjectParameterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddParameterCommandHandler(IProjectParameterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ParameterResponse>> Handle(AddParameterCommand request, CancellationToken cancellationToken)
    {
        var section = await _repository.GetSectionByIdAsync(request.SectionId, cancellationToken);
        if (section == null)
        {
            return Result<ParameterResponse>.Failure("Parameter section not found.");
        }

        var param = new ProjectParameter
        {
            SectionId = request.SectionId,
            ParameterName = request.ParameterName,
            InputType = request.InputType,
            Unit = request.Unit,
            IsRequired = request.IsRequired,
            DisplayOrder = request.DisplayOrder,
            DropdownOptionsJson = request.DropdownOptionsJson
        };

        await _repository.AddParameterAsync(param, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ParameterResponse>.Success(new ParameterResponse(
            param.Id, param.SectionId, param.ParameterName, param.InputType, param.Unit, param.IsRequired, param.DisplayOrder, param.DropdownOptionsJson));
    }
}
