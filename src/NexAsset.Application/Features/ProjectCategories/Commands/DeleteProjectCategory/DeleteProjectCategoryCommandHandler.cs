using MediatR;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Common.Results;

namespace NexAsset.Application.Features.ProjectCategories.Commands.DeleteProjectCategory;

public sealed class DeleteProjectCategoryCommandHandler : IRequestHandler<DeleteProjectCategoryCommand, Result>
{
    private readonly IProjectCategoryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectCategoryCommandHandler(IProjectCategoryRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteProjectCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
        {
            return Result.Failure("Project category not found.");
        }

        category.IsDeleted = true;
        category.DeletedAtUtc = DateTime.UtcNow;

        _repository.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
