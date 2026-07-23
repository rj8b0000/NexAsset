using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectParameters.Queries.GetParameterSections;

namespace NexAsset.Application.Features.ProjectParameters.Commands.UpdateParameter;

public sealed class UpdateParameterCommandHandler : IRequestHandler<UpdateParameterCommand, Result<ParameterResponse>>
{
    private readonly IProjectParameterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateParameterCommandHandler(IProjectParameterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ParameterResponse>> Handle(UpdateParameterCommand request, CancellationToken cancellationToken)
    {
        var param = await _repository.GetParameterByIdAsync(request.Id, cancellationToken);
        if (param == null)
        {
            return Result<ParameterResponse>.Failure("Parameter not found.");
        }

        param.ParameterName = request.ParameterName;
        param.InputType = request.InputType;
        param.Unit = request.Unit;
        param.IsRequired = request.IsRequired;
        param.DisplayOrder = request.DisplayOrder;
        param.DropdownOptionsJson = request.DropdownOptionsJson;
        param.UpdatedAtUtc = DateTime.UtcNow;

        _repository.UpdateParameter(param);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ParameterResponse>.Success(new ParameterResponse(
            param.Id, param.SectionId, param.ParameterName, param.InputType, param.Unit, param.IsRequired, param.DisplayOrder, param.DropdownOptionsJson));
    }
}
