using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Mappings;
using NexAsset.Application.Common.Results;
using NexAsset.Application.Features.ProjectCategories.Queries.GetProjectCategories;

namespace NexAsset.Application.Features.ProjectCategories.Commands.CreateProjectCategory;

public sealed class CreateProjectCategoryCommandHandler : IRequestHandler<CreateProjectCategoryCommand, Result<ProjectCategoryResponse>>
{
    private readonly IProjectCategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectCategoryCommandHandler(IProjectCategoryRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProjectCategoryResponse>> Handle(CreateProjectCategoryCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsByNameAsync(request.OrganizationId, request.Name, cancellationToken);
        if (exists)
        {
            return Result<ProjectCategoryResponse>.Failure($"Project category name '{request.Name}' already exists.");
        }

        var category = ProjectCategoryMapper.ToEntity(request);
        await _repository.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ProjectCategoryResponse>.Success(ProjectCategoryMapper.ToResponse(category));
    }
}
