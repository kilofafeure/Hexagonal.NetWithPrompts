using MyExpenses.ApplicationLayer.Ports.PrimaryPort.Categories;
using MyExpenses.ApplicationLayer.Ports.SecundaryPort;
using MyExpenses.Domain.Entities;

namespace MyExpenses.ApplicationLayer.UseCases.Categories;

public sealed class CreateCategoryUseCase : ICreateCategoryUseCase
{
    private readonly IRepository<Category> _repository;

    public CreateCategoryUseCase(IRepository<Category> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Category> CreateCategoryAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var category = new Category(name);
        await _repository.AddAsync(category, cancellationToken);
        return category;
    }
}
