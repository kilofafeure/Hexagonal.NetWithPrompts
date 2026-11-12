using Microsoft.AspNetCore.Http.HttpResults;
using MyExpenses.ApplicationLayer.Ports.PrimaryPort.Expenses;
using MyExpenses.Domain.Entities;
using MyExpenses.ExpenseAPI.Contracts;

namespace MyExpenses.ExpenseAPI.Endpoints;

internal static class ExpenseEndpoints
{
    public static RouteGroupBuilder MapExpenseEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/expenses")
            .WithTags("Expenses");

        group.MapPost(
                "",
                async Task<Created<ExpenseResponse>> (
                    CreateExpenseRequest request,
                    IRegisterExpenseUseCase useCase,
                    CancellationToken cancellationToken) =>
                {
                    var expense = await useCase.RegisterExpenseAsync(
                        request.Concept,
                        request.Amount,
                        request.Date,
                        request.IdCategory,
                        request.Note,
                        cancellationToken);

                    return TypedResults.Created(
                        $"/api/expenses/{expense.Id}",
                        ToResponse(expense));
                })
            .WithName("RegisterExpense")
            .WithSummary("Register an expense")
            .WithDescription("Registers a new expense and returns its details.")
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet(
                "",
                async Task<Ok<IReadOnlyList<ExpenseResponse>>> (
                    IGetExpensesUseCase useCase,
                    CancellationToken cancellationToken) =>
                {
                    var expenses = await useCase.GetExpensesAsync(cancellationToken);
                    IReadOnlyList<ExpenseResponse> response = expenses
                        .Select(ToResponse)
                        .ToArray();

                    return TypedResults.Ok(response);
                })
            .WithName("GetExpenses")
            .WithSummary("Get active expenses")
            .WithDescription("Returns all expenses that have not been logically deleted.");

        group.MapGet(
                "/{expenseId:guid}",
                async Task<Results<Ok<ExpenseResponse>, NotFound>> (
                    Guid expenseId,
                    IGetExpensesUseCase useCase,
                    CancellationToken cancellationToken) =>
                {
                    var expenses = await useCase.GetExpensesAsync(cancellationToken);
                    var expense = expenses.FirstOrDefault(candidate => candidate.Id == expenseId);

                    return expense is null
                        ? TypedResults.NotFound()
                        : TypedResults.Ok(ToResponse(expense));
                })
            .WithName("GetExpenseById")
            .WithSummary("Get an active expense by ID")
            .WithDescription("Returns an active expense, or 404 if it does not exist or was deleted.")
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapDelete(
                "/{expenseId:guid}",
                async Task<NoContent> (
                    Guid expenseId,
                    IDeleteExpenseUseCase useCase,
                    CancellationToken cancellationToken) =>
                {
                    await useCase.DeleteExpenseAsync(expenseId, cancellationToken);
                    return TypedResults.NoContent();
                })
            .WithName("DeleteExpense")
            .WithSummary("Logically delete an expense")
            .WithDescription("Marks an expense as deleted without physically removing it.")
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return group;
    }

    private static ExpenseResponse ToResponse(Expense expense) =>
        new(
            expense.Id,
            expense.Concept,
            expense.Amount,
            expense.Date,
            expense.IdCategory,
            expense.Note,
            expense.CreatedAt,
            expense.DeletedAt);
}
