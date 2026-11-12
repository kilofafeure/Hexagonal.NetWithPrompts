using MyExpenses.ApplicationLayer.Ports.SecundaryPort;
using MyExpenses.Domain.Entities;

namespace MyExpenses.FileRepository;

public sealed class ExpenseRepository : IRepository<Expense>
{
    private readonly List<Expense> _expenses = [];
    private readonly object _syncRoot = new();

    public Task AddAsync(Expense expense, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(expense);

        lock (_syncRoot)
        {
            _expenses.Add(expense);
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Expense>> GetAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        lock (_syncRoot)
        {
            IReadOnlyList<Expense> expenses = _expenses.ToArray();
            return Task.FromResult(expenses);
        }
    }

    public Task DeleteAsync(Expense expense, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(expense);

        lock (_syncRoot)
        {
            var index = _expenses.FindIndex(existingExpense => existingExpense.Id == expense.Id);

            if (index < 0)
            {
                throw new KeyNotFoundException($"Expense with ID '{expense.Id}' was not found.");
            }

            _expenses[index] = expense;
        }

        return Task.CompletedTask;
    }
}
