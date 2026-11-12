namespace MyExpenses.ExpenseAPI.Contracts;

/// <summary>Expense data returned by the API.</summary>
public sealed record ExpenseResponse(
    Guid Id,
    string Concept,
    decimal Amount,
    DateOnly Date,
    Guid IdCategory,
    string? Note,
    DateTimeOffset CreatedAt,
    DateTimeOffset? DeletedAt);
