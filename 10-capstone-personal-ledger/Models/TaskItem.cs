namespace PersonalLedger.Models;

public record TaskItem(Guid Id, string Title, DateTime? DueDate, TaskPriority Priority, TaskStatus Status) : IEntity;
