using MyExpenses.ApplicationLayer.Ports.PrimaryPort.Expenses;
using MyExpenses.ApplicationLayer.Ports.SecundaryPort;
using MyExpenses.Domain.Entities;

namespace MyExpenses.ApplicationLayer.UseCases.Expenses;

public sealed class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IRepository _repository;

    public RegisterExpenseUseCase(IRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Expense> RegisterExpenseAsync(
        string concept,
        decimal amount,
        DateOnly date,
        Guid idCategory,
        string? note = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var expense = new Expense(concept, amount, date, idCategory, note);
        await _repository.AddAsync(expense, cancellationToken);
        return expense;
    }
}
