using MyExpenses.Domain.Entities;

namespace MyExpenses.ApplicationLayer.Ports.PrimaryPort.Expenses;

public interface IGetExpensesUseCase
{
    Task<IReadOnlyList<Expense>> GetExpensesAsync(CancellationToken cancellationToken = default);
}
