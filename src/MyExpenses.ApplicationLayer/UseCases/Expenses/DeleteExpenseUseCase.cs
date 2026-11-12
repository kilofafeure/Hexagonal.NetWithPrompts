using MyExpenses.ApplicationLayer.Ports.PrimaryPort.Expenses;
using MyExpenses.ApplicationLayer.Ports.SecundaryPort;

namespace MyExpenses.ApplicationLayer.UseCases.Expenses;

public sealed class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IRepository _repository;

    public DeleteExpenseUseCase(IRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task DeleteExpenseAsync(Guid expenseId, CancellationToken cancellationToken = default)
    {
        var expenses = await _repository.GetAsync(cancellationToken);
        var expense = expenses.FirstOrDefault(candidate => candidate.Id == expenseId);

        if (expense is null)
        {
            throw new KeyNotFoundException($"Expense with ID '{expenseId}' was not found.");
        }

        cancellationToken.ThrowIfCancellationRequested();
        expense.Delete();
        await _repository.DeleteAsync(expense, cancellationToken);
    }
}
