using MyExpenses.ApplicationLayer.Ports.SecundaryPort;
using MyExpenses.Domain.Entities;

namespace MyExpenses.FileRepository;

public sealed class CategoryRepository : IRepository<Category>
{
    private readonly List<Category> _categories = [];
    private readonly object _syncRoot = new();

    public Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(category);

        lock (_syncRoot)
        {
            _categories.Add(category);
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Category>> GetAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        lock (_syncRoot)
        {
            IReadOnlyList<Category> categories = _categories.ToArray();
            return Task.FromResult(categories);
        }
    }

    public Task DeleteAsync(Category category, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(category);

        lock (_syncRoot)
        {
            var index = _categories.FindIndex(existingCategory => existingCategory.Id == category.Id);

            if (index < 0)
            {
                throw new KeyNotFoundException($"Category with ID '{category.Id}' was not found.");
            }

            _categories.RemoveAt(index);
        }

        return Task.CompletedTask;
    }
}
