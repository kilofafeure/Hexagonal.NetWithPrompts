using MyExpenses.Domain.Entities;

namespace MyExpenses.ApplicationLayer.Ports.PrimaryPort.Expenses;

public interface IRegisterExpenseUseCase
{
    Task<Expense> RegisterExpenseAsync(
        string concept,
        decimal amount,
        DateOnly date,
        Guid idCategory,
        string? note = null,
        CancellationToken cancellationToken = default);
}
