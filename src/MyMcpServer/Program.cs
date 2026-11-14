using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using MyExpenses.ApplicationLayer.Ports.PrimaryPort.Expenses;
using MyExpenses.ApplicationLayer.Ports.SecundaryPort;
using MyExpenses.ApplicationLayer.UseCases.Expenses;
using MyExpenses.Domain.Entities;
using MyExpenses.FileRepository;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services.AddSingleton<IRepository<Expense>, ExpenseRepository>();
builder.Services.AddScoped<IGetExpensesUseCase, GetExpensesUseCase>();
builder.Services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();
