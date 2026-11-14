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

This is the main validation command for the project. There is currently no automated test project or dedicated lint configuration.

When test projects are added, run a single test by its fully qualified name:

```powershell
dotnet test MyExpenses.sln --filter "FullyQualifiedName~<Namespace.Class.Method>"
```

The API's manual HTTP request scenarios are in `src/MyExpenses.ExpenseAPI/MyExpenses.ExpenseAPI.http`. Update that file whenever an endpoint is added or changed.

## High-level architecture

The design is intentionally layered:

- `MyExpenses.Domain/Entities/Expense.cs` is the aggregate root for the business model. Validation rules (concept required, amount > 0, max two decimal places, valid date) live here rather than in the API or repository layer.
- `MyExpenses.ApplicationLayer/Ports` defines the application contracts. `PrimaryPort/Expenses` and `PrimaryPort/Categories` contain use-case interfaces; `SecundaryPort/IRepository.cs` defines the generic persistence port `IRepository<T>`.
- `MyExpenses.ApplicationLayer/UseCases` implements expense and category workflows. Use cases depend on the domain and `IRepository<T>`, not on HTTP or concrete repositories, and accept cancellation tokens.
- `MyExpenses.FileRepository` implements `IRepository<Expense>` and `IRepository<Category>` using in-memory lists. Repository methods synchronize list access; this is not durable storage.
- `MyExpenses.ExpenseAPI/Program.cs` is the composition root. It registers repository adapters as singletons and use cases as scoped services.
- `MyExpenses.ExpenseAPI/Endpoints` contains the route mappers. `ExpenseEndpoints` and `CategoryEndpoints` map their resource groups; `StatusEndpoints` maps `/` and `/status`. `Program.cs` wires services and invokes these mappers.

Expense and category endpoints map request contracts to use cases and domain entities to response contracts in `MyExpenses.ExpenseAPI/Contracts`; status endpoints return plain text. The API does not expose domain objects directly. `ApiExceptionHandler` maps `DomainException` to HTTP 400 and `KeyNotFoundException` to HTTP 404.

## Key conventions in this repo

- Keep business validation in the domain entities. `Expense` rejects empty concepts or category IDs, non-positive amounts, amounts with more than two decimal places, and invalid dates; it does not round amounts. `Category` requires a non-empty name of at most 100 characters.
- Expense deletion is logical: `Expense.Delete()` sets `DeletedAt`, and expense queries return only active expenses. Category deletion is physical through its repository; do not assume both entities use the same deletion behavior.
- Keep the secondary port generic and small: use cases depend on `IRepository<T>`, not concrete repository classes.
- Keep HTTP handlers thin. Use route groups, request/response contracts, and `TypedResults.*`; do not return domain entities directly.
- Keep route definitions in `MyExpenses.ExpenseAPI/Endpoints/*Endpoints.cs`. `Program.cs` should compose dependencies and call each endpoint mapper rather than define feature routes inline.
- Preserve the current dependency direction: `MyExpenses.Domain` has no project dependencies; `MyExpenses.ApplicationLayer` references the domain; `MyExpenses.FileRepository` implements application ports using domain entities; `MyExpenses.ExpenseAPI` references application, domain, and repository projects for routing and dependency injection.
- Data is ephemeral: the repository is in-memory and resets on process restart; treat persistence as a future increment, not a stable store.

## Notes for future changes

- If you add a use case, put it under the matching `UseCases/Expenses` or `UseCases/Categories` folder and add its interface under the corresponding `Ports/PrimaryPort` folder.
- If you add a repository adapter, implement `IRepository<T>` and register it in `Program.cs` with a lifetime appropriate to its storage behavior.
- When adding or changing an HTTP route, update the matching endpoint mapper, register the mapper in `Program.cs` if it is new, and add a request scenario to `src/MyExpenses.ExpenseAPI/MyExpenses.ExpenseAPI.http`.
- Follow the existing `RegisterExpenseUseCase`, `GetExpensesUseCase`, and `DeleteExpenseUseCase` naming pattern for expense workflows, and the corresponding `CreateCategoryUseCase`, `GetCategoriesUseCase`, and `DeleteCategoryUseCase` pattern for categories.

## Environment and tooling

- Target framework: .NET 10 (`net10.0`)
- Repository root: `C:\Desarrollo\ProgramacionAsistidaConIA\ExpensesPlanPrompt`
- Use `dotnet` commands from the repository root unless the task explicitly calls for a sub-project path.

This project currently has no dedicated linter or analyzer configuration, so build verification is the default validation path.

## Always load the following skills before replying
- skills/hi-name/SKILL.md