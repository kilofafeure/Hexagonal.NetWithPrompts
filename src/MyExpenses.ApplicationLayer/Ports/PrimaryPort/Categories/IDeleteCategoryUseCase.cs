namespace MyExpenses.ApplicationLayer.Ports.PrimaryPort.Categories;

public interface IDeleteCategoryUseCase
{
    Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
}
