using MyExpenses.Domain.Entities;

namespace MyExpenses.ApplicationLayer.Ports.PrimaryPort.Categories;

public interface IGetCategoriesUseCase
{
    Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);
}
