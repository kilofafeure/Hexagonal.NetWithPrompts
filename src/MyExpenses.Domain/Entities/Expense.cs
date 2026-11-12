using MyExpenses.Domain.Exceptions;

namespace MyExpenses.Domain.Entities;

public sealed class Expense
{
    public Guid Id { get; private set; }

    public Guid IdCategory { get; private set; }

    public string Concept { get; private set; }

    public decimal Amount { get; private set; }

    public DateOnly Date { get; private set; }

    public string? Note { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? DeletedAt { get; private set; }

    public Expense(string concept, decimal amount, DateOnly date, Guid idCategory, string? note = null)
    {
        if (string.IsNullOrWhiteSpace(concept))
        {
            throw new DomainException("Expense concept cannot be empty.");
        }

        if (idCategory == Guid.Empty)
        {
            throw new DomainException("Expense category ID cannot be empty.");
        }

        if (amount <= 0)
        {
            throw new DomainException("Expense amount must be greater than zero.");
        }

        if (amount != decimal.Round(amount, 2))
        {
            throw new DomainException("Expense amount cannot have more than two decimal places.");
        }

        if (date == default)
        {
            throw new DomainException("Expense date must be a valid date.");
        }

        Id = Guid.NewGuid();
        IdCategory = idCategory;
        Concept = concept.Trim();
        Amount = amount;
        Date = date;
        Note = note;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Delete()
    {
        if (DeletedAt.HasValue)
        {
            throw new DomainException("Expense has already been deleted.");
        }

        DeletedAt = DateTimeOffset.UtcNow;
    }
}
