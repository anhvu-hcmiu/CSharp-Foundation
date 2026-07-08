using PersonalLedger.Models;

namespace PersonalLedger.Events;

public class TaskOverdueEventArgs(TaskItem task) : EventArgs
{
    public TaskItem Task { get; } = task;
}
