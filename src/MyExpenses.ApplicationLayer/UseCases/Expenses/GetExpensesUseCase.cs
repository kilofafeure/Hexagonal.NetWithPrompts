using MyExpenses.ApplicationLayer.Ports.PrimaryPort.Expenses;
using MyExpenses.ApplicationLayer.Ports.SecundaryPort;
using MyExpenses.Domain.Entities;

namespace MyExpenses.ApplicationLayer.UseCases.Expenses;

public sealed class GetExpensesUseCase : IGetExpensesUseCase
{
    private readonly IRepository _repository;

    public GetExpensesUseCase(IRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IReadOnlyList<Expense>> GetExpensesAsync(CancellationToken cancellationToken = default)
    {
        var expenses = await _repository.GetAsync(cancellationToken);
        return expenses.Where(expense => expense.DeletedAt is null).ToArray();
    }
}
