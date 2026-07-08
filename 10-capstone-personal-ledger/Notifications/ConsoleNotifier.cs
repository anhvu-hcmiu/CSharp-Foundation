using PersonalLedger.Services;

namespace PersonalLedger.Notifications;

public class ConsoleNotifier
{
    public ConsoleNotifier(TaskService taskService, ExpenseService expenseService)
    {
        taskService.TaskOverdue += (_, e) =>
            Console.WriteLine($"[ALERT] Task overdue: '{e.Task.Title}' was due {e.Task.DueDate:d}.");

        expenseService.BudgetExceeded += (_, e) =>
            Console.WriteLine($"[ALERT] Budget exceeded for {e.Category}: spent {e.Total.Amount:C} of {e.Limit.Amount:C} limit.");
    }
}
