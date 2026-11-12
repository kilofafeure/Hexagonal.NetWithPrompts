using Microsoft.AspNetCore.Diagnostics;
using MyExpenses.ApplicationLayer.Ports.PrimaryPort.Expenses;
using MyExpenses.ApplicationLayer.Ports.SecundaryPort;
using MyExpenses.ApplicationLayer.UseCases.Expenses;
using MyExpenses.ExpenseAPI.Endpoints;
using MyExpenses.ExpenseAPI.Middleware;
using MyExpenses.FileRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddSingleton<IRepository, ExpenseRepository>();
builder.Services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
builder.Services.AddScoped<IGetExpensesUseCase, GetExpensesUseCase>();
builder.Services.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "API Corriendo");
app.MapExpenseEndpoints();

app.Run();
