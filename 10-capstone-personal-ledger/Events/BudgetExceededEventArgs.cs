using PersonalLedger.Models;

namespace PersonalLedger.Events;

public class BudgetExceededEventArgs(ExpenseCategory category, Money total, Money limit) : EventArgs
{
    public ExpenseCategory Category { get; } = category;
    public Money Total { get; } = total;
    public Money Limit { get; } = limit;
}
