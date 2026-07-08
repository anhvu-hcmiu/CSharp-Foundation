using PersonalLedger.Events;
using PersonalLedger.Exceptions;
using PersonalLedger.Models;
using PersonalLedger.Repositories;

namespace PersonalLedger.Services;

public class ExpenseService(IRepository<Expense> repository, Dictionary<ExpenseCategory, Money>? budgetLimits = null)
{
    private readonly Dictionary<ExpenseCategory, Money> _budgetLimits = budgetLimits ?? new Dictionary<ExpenseCategory, Money>
    {
        [ExpenseCategory.Food] = new Money(500m),
        [ExpenseCategory.Housing] = new Money(2000m),
        [ExpenseCategory.Transport] = new Money(300m),
        [ExpenseCategory.Entertainment] = new Money(200m),
        [ExpenseCategory.Other] = new Money(300m),
    };

    public event EventHandler<BudgetExceededEventArgs>? BudgetExceeded;

    public async Task<Expense> AddExpenseAsync(decimal amount, ExpenseCategory category, DateTime date)
    {
        if (amount < 0)
        {
            throw new LedgerValidationException("Expense amount cannot be negative.");
        }

        var expense = new Expense(Guid.NewGuid(), new Money(amount), category, date);
        await repository.AddAsync(expense);

        var total = (await repository.GetAllAsync())
            .Where(e => e.Category == category)
            .Sum(e => e.Amount.Amount);

        if (_budgetLimits.TryGetValue(category, out var limit) && total > limit.Amount)
        {
            BudgetExceeded?.Invoke(this, new BudgetExceededEventArgs(category, new Money(total), limit));
        }

        return expense;
    }

    public Task<List<Expense>> GetAllAsync() => repository.GetAllAsync();
}
