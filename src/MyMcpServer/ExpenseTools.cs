using System.ComponentModel;
using ModelContextProtocol.Server;
using MyExpenses.ApplicationLayer.Ports.PrimaryPort.Expenses;
using MyExpenses.Domain.Entities;

namespace MyMcpServer;

[McpServerToolType]
public static class ExpenseTools
{
    [McpServerTool, Description("Obtiene los gastos activos.")]
    public static async Task<IReadOnlyList<Expense>> GetExpensesAsync(
        IGetExpensesUseCase useCase,
        CancellationToken cancellationToken)
    {
        return await useCase.GetExpensesAsync(cancellationToken);
    }

    [McpServerTool, Description("Registra un gasto con la fecha actual.")]
    public static async Task<Expense> AddExpenseAsync(
        [Description("Concepto del gasto.")] string concept,
        [Description("Monto del gasto.")] decimal amount,
        [Description("Identificador GUID de la categoría.")] Guid idCategory,
        IRegisterExpenseUseCase useCase,
        CancellationToken cancellationToken)
    {
        return await useCase.RegisterExpenseAsync(
            concept,
            amount,
            DateOnly.FromDateTime(DateTime.Today),
            idCategory,
            cancellationToken: cancellationToken);
    }
}
