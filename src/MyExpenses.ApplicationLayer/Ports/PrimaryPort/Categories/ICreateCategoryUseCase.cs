using MyExpenses.Domain.Entities;

namespace MyExpenses.ApplicationLayer.Ports.PrimaryPort.Categories;

public interface ICreateCategoryUseCase
{
    Task<Category> CreateCategoryAsync(string name, CancellationToken cancellationToken = default);
}
