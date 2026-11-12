using MyExpenses.ApplicationLayer.Ports.PrimaryPort.Categories;
using MyExpenses.ApplicationLayer.Ports.SecundaryPort;
using MyExpenses.Domain.Entities;

namespace MyExpenses.ApplicationLayer.UseCases.Categories;

public sealed class GetCategoriesUseCase : IGetCategoriesUseCase
{
    private readonly IRepository<Category> _repository;

    public GetCategoriesUseCase(IRepository<Category> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<IReadOnlyList<Category>> GetCategoriesAsync(
        CancellationToken cancellationToken = default) =>
        _repository.GetAsync(cancellationToken);
}
