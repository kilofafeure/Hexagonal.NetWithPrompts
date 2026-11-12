namespace MyExpenses.ExpenseAPI.Contracts;

/// <summary>Request body for registering an expense.</summary>
public sealed record CreateExpenseRequest
{
    public required string Concept { get; init; }

    public required decimal Amount { get; init; }

    public required DateOnly Date { get; init; }

    public string? Note { get; init; }
}
