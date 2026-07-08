using PersonalLedger.Exceptions;
using PersonalLedger.Models;
using PersonalLedger.Notifications;
using PersonalLedger.Repositories;
using PersonalLedger.Services;

var dataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");
var taskRepository = new JsonFileRepository<TaskItem>(Path.Combine(dataDirectory, "tasks.json"));
var expenseRepository = new JsonFileRepository<Expense>(Path.Combine(dataDirectory, "expenses.json"));

var taskService = new TaskService(taskRepository);
var expenseService = new ExpenseService(expenseRepository);
_ = new ConsoleNotifier(taskService, expenseService);

Console.WriteLine("Adding an overdue task...");
await taskService.AddTaskAsync("Submit late report", DateTime.Today.AddDays(-1), TaskPriority.High);

Console.WriteLine("Adding an expense that exceeds the Food budget...");
await expenseService.AddExpenseAsync(600m, ExpenseCategory.Food, DateTime.Today);

try
{
    await taskService.AddTaskAsync(string.Empty, DateTime.Today, TaskPriority.Low);
}
catch (LedgerValidationException ex)
{
    Console.WriteLine($"[VALIDATION] {ex.Message}");
}

try
{
    await expenseService.AddExpenseAsync(-10m, ExpenseCategory.Other, DateTime.Today);
}
catch (LedgerValidationException ex)
{
    Console.WriteLine($"[VALIDATION] {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("Tasks:");
foreach (var t in await taskService.GetAllAsync())
{
    Console.WriteLine($"  {t.Id} | {t.Title} | Due: {t.DueDate:d} | {t.Priority} | {t.Status}");
}

Console.WriteLine("Expenses:");
foreach (var e in await expenseService.GetAllAsync())
{
    Console.WriteLine($"  {e.Id} | {e.Amount.Amount:C} | {e.Category} | {e.Date:d}");
}
