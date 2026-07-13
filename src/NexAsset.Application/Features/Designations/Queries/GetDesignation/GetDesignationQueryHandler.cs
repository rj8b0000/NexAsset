using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.Designations.Queries.GetDesignation;

public sealed class GetDesignationQueryHandler
    : IRequestHandler<GetDesignationQuery, Result<GetDesignationResponse>>
{
    private readonly IDesignationRepository _repository;

    public GetDesignationQueryHandler(
        IDesignationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetDesignationResponse>> Handle(
        GetDesignationQuery request,
        CancellationToken cancellationToken)
    {
        var designation = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (designation is null)
        {
            return Result<GetDesignationResponse>
                .Failure("Designation not found.");
        }

        return Result<GetDesignationResponse>.Success(
            DesignationMapper.ToGetResponse(designation));
    }
}
