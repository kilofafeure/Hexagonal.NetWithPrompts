namespace MyExpenses.ApplicationLayer.Ports.PrimaryPort.Expenses;

public interface IDeleteExpenseUseCase
{
    Task DeleteExpenseAsync(Guid expenseId, CancellationToken cancellationToken = default);
}
