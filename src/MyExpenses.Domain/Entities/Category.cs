using MyExpenses.Domain.Exceptions;

namespace MyExpenses.Domain.Entities;

public sealed class Category
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public Category(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Category name cannot be empty.");
        }

        if (name.Length > 100)
        {
            throw new DomainException("Category name cannot exceed 100 characters.");
        }

        Id = Guid.NewGuid();
        Name = name.Trim();
    }
}
