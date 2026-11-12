using MyExpenses.Domain.Entities;

namespace MyExpenses.ApplicationLayer.Ports.SecundaryPort;

public interface IRepository
{
    Task AddAsync(Expense expense, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Expense>> GetAsync(CancellationToken cancellationToken = default);

    Task DeleteAsync(Expense expense, CancellationToken cancellationToken = default);
}
