using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectParameters.Queries.GetParameterSections;

public sealed class GetParameterSectionsQueryHandler : IRequestHandler<GetParameterSectionsQuery, Result<List<ParameterSectionResponse>>>
{
    private readonly IProjectParameterRepository _repository;

    public GetParameterSectionsQueryHandler(IProjectParameterRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<ParameterSectionResponse>>> Handle(GetParameterSectionsQuery request, CancellationToken cancellationToken)
    {
        var sections = await _repository.GetSectionsByProjectAsync(request.ProjectId, cancellationToken);
        var response = sections.Select(s => new ParameterSectionResponse(
            s.Id,
            s.ProjectId,
            s.Name,
            s.DisplayOrder,
            s.Parameters.OrderBy(p => p.DisplayOrder).Select(p => new ParameterResponse(
                p.Id,
                p.SectionId,
                p.ParameterName,
                p.InputType,
                p.Unit,
                p.IsRequired,
                p.DisplayOrder,
                p.DropdownOptionsJson)).ToList())).ToList();

        return Result<List<ParameterSectionResponse>>.Success(response);
    }
}
