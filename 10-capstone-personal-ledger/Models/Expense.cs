namespace PersonalLedger.Models;

public record Expense(Guid Id, Money Amount, ExpenseCategory Category, DateTime Date) : IEntity;
