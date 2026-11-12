using MyExpenses.ApplicationLayer.Ports.PrimaryPort.Categories;
using MyExpenses.ApplicationLayer.Ports.SecundaryPort;
using MyExpenses.Domain.Entities;

namespace MyExpenses.ApplicationLayer.UseCases.Categories;

public sealed class DeleteCategoryUseCase : IDeleteCategoryUseCase
{
    private readonly IRepository<Category> _repository;

    public DeleteCategoryUseCase(IRepository<Category> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task DeleteCategoryAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        var categories = await _repository.GetAsync(cancellationToken);
        var category = categories.FirstOrDefault(candidate => candidate.Id == categoryId);

        if (category is null)
        {
            throw new KeyNotFoundException($"Category with ID '{categoryId}' was not found.");
        }

        cancellationToken.ThrowIfCancellationRequested();
        await _repository.DeleteAsync(category, cancellationToken);
    }
}
