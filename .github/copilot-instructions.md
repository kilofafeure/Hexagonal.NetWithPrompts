# Copilot instructions for MyExpenses

## Project context

This repository is a .NET 10 backend for tracking personal expenses. The canonical project description is in [README.md](../README.md). The solution is rooted at `MyExpenses.sln` and contains four projects under `src/`:

- `MyExpenses.Domain`: domain entities and validation rules.
- `MyExpenses.ApplicationLayer`: use cases and ports for the application layer.
- `MyExpenses.FileRepository`: implementation of the secondary port, currently backed by an in-memory list.
- `MyExpenses.ExpenseAPI`: Minimal API HTTP layer.

The application follows a hexagonal architecture with a clear separation between domain, application logic, infrastructure adapters, and HTTP endpoints.

## Build, test, and validation commands

From the repository root:

```powershell
dotnet build MyExpenses.sln
```

This is the main validation command for the project. There is no dedicated lint configuration in the repo and no test project is present at the moment, so there is no project-wide `dotnet test` target to run today.

If a test project is added later, prefer a single-test run pattern such as:

```powershell
dotnet test MyExpenses.sln --filter "FullyQualifiedName~Expense"
```

When debugging one test or one test class, keep the `--filter` narrow to the target name instead of running the full suite.

## High-level architecture

The design is intentionally layered:

- `MyExpenses.Domain/Entities/Expense.cs` is the aggregate root for the business model. Validation rules (concept required, amount > 0, max two decimal places, valid date) live here rather than in the API or repository layer.
- `MyExpenses.ApplicationLayer/Ports` defines the application contracts:
  - `PrimaryPort/Expenses` contains the use-case interfaces exposed to the API layer.
  - `SecundaryPort/IRepository.cs` defines persistence operations for the repository adapter.
- `MyExpenses.ApplicationLayer/UseCases/Expenses` contains the transactional application logic (`RegisterExpenseUseCase`, `GetExpensesUseCase`, `DeleteExpenseUseCase`). These use cases depend on `IRepository` and do not know about HTTP.
- `MyExpenses.FileRepository/ExpenseRepository.cs` is the current infrastructure implementation. It stores expenses in memory with a synchronized list and is intentionally not a durable database implementation.
- `MyExpenses.ExpenseAPI/Program.cs` wires the dependency injection container and registers the repository and use cases as scoped/singleton services.
- `MyExpenses.ExpenseAPI/Endpoints/ExpenseEndpoints.cs` defines the HTTP routes using Minimal API, with route grouping for `/api/expenses` and typed result objects.

The API does not expose domain objects directly; it maps them to response contracts in `MyExpenses.ExpenseAPI/Contracts`.

## Key conventions in this repo

- Keep domain validation in the domain model, not in endpoint handlers or use cases.
- Use soft delete semantics: `Expense.Delete()` sets `DeletedAt` instead of removing the record physically; the `GetExpensesUseCase` filters out deleted records.
- Keep the repository interface small and dependency-inverted: use cases depend only on `IRepository`, not concrete implementations.
- Prefer route-grouped Minimal API setup and `TypedResults.*` responses in the API layer instead of custom controller classes.
- Preserve the hexagonal boundaries: `MyExpenses.Domain` must not reference API or infrastructure projects; the application layer should reference the domain, and the API should reference the application and infrastructure projects only as needed for dependency injection.
- Data is ephemeral: the repository is in-memory and resets on process restart; treat persistence as a future increment, not a stable store.

## Notes for future changes

- If you add a new use case, follow the existing pattern under `src/MyExpenses.ApplicationLayer/UseCases/Expenses` and add the corresponding port under `Ports/PrimaryPort/Expenses`.
- If you add a new adapter, keep it behind the `IRepository` abstraction and register it in `Program.cs`.
- Keep the HTTP layer thin: endpoints should translate request/response contracts and delegate to the application use cases.
- Prefer existing naming patterns (`RegisterExpenseUseCase`, `GetExpensesUseCase`, `DeleteExpenseUseCase`) when extending the expense domain.

## Environment and tooling

- Target framework: .NET 10 (`net10.0`)
- Repository root: `C:\Desarrollo\ProgramacionAsistidaConIA\ExpensesPlanPrompt`
- Use `dotnet` commands from the repository root unless the task explicitly calls for a sub-project path.

This project currently has no dedicated linter or analyzer configuration, so build verification is the default validation path.
