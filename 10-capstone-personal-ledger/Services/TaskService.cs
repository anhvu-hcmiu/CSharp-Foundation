using PersonalLedger.Events;
using PersonalLedger.Exceptions;
using PersonalLedger.Models;
using PersonalLedger.Repositories;
using TaskStatus = PersonalLedger.Models.TaskStatus;

namespace PersonalLedger.Services;

public class TaskService(IRepository<TaskItem> repository)
{
    public event EventHandler<TaskOverdueEventArgs>? TaskOverdue;

    public async Task<TaskItem> AddTaskAsync(string title, DateTime? dueDate, TaskPriority priority)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new LedgerValidationException("Task title cannot be empty.");
        }

        var task = new TaskItem(Guid.NewGuid(), title, dueDate, priority, TaskStatus.Pending);
        await repository.AddAsync(task);

        if (IsOverdue(task))
        {
            TaskOverdue?.Invoke(this, new TaskOverdueEventArgs(task));
        }

        return task;
    }

    public async Task CompleteTaskAsync(Guid id)
    {
        var task = await repository.GetByIdAsync(id)
            ?? throw new LedgerValidationException($"Task with Id '{id}' was not found.");

        await repository.UpdateAsync(task with { Status = TaskStatus.Done });
    }

    public Task<List<TaskItem>> GetAllAsync() => repository.GetAllAsync();

    public async Task<List<TaskItem>> GetOverdueAsync()
    {
        var tasks = await repository.GetAllAsync();
        return tasks.Where(IsOverdue).ToList();
    }

    private static bool IsOverdue(TaskItem task) =>
        task.DueDate < DateTime.Now && task.Status != TaskStatus.Done;
}
