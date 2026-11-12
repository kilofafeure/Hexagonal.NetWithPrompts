namespace MyExpenses.ExpenseAPI.Contracts;

/// <summary>Request body for creating a category.</summary>
public sealed record CreateCategoryRequest
{
    public required string Name { get; init; }
}
