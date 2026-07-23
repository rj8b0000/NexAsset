using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectParameters.Queries.GetParameterSections;
using NexAsset.Domain.Entities;

namespace NexAsset.Application.Features.ProjectParameters.Commands.CreateParameterSection;

public sealed class CreateParameterSectionCommandHandler : IRequestHandler<CreateParameterSectionCommand, Result<ParameterSectionResponse>>
{
    private readonly IProjectParameterRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateParameterSectionCommandHandler(IProjectParameterRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ParameterSectionResponse>> Handle(CreateParameterSectionCommand request, CancellationToken cancellationToken)
    {
        var section = new ProjectParameterSection
        {
            ProjectId = request.ProjectId,
            Name = request.Name,
            DisplayOrder = request.DisplayOrder
        };

        await _repository.AddSectionAsync(section, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ParameterSectionResponse>.Success(new ParameterSectionResponse(
            section.Id, section.ProjectId, section.Name, section.DisplayOrder, new List<ParameterResponse>()));
    }
}
