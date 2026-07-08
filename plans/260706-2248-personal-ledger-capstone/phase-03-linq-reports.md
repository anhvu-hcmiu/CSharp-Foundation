---
phase: 3
title: "LINQ Reports"
status: pending
priority: P2
dependencies: [1, 2]
effort: "S"
---

# Phase 3: LINQ Reports

## Overview

Add a `LedgerReports` module that turns raw task/expense data into the aggregate views the curriculum's LINQ phase (05) was building toward: spend-by-category, monthly totals, overdue task list, completion rate. Pure read-side, no mutation - takes data already loaded via the repositories and returns computed view models via LINQ.

## Requirements

- Functional:
  - `SpendByCategory()`: `Dictionary<ExpenseCategory, Money>` or ordered list, grouped/summed from all expenses.
  - `MonthlyTotals()`: totals grouped by year-month.
  - `OverdueTasks()`: tasks where `DueDate < now && Status != Done`, ordered by due date ascending.
  - `CompletionRate()`: `Done` count / total count as a percentage.
- Non-functional:
  - All methods pure functions over `IEnumerable<TaskItem>`/`IEnumerable<Expense>` input (no direct repository/file access inside `LedgerReports` - keeps it testable and honors single-responsibility; `Program.cs` fetches via services/repositories and passes the collections in).
  - Use LINQ method syntax consistent with exercises in `05-collections-linq/` (the existing curriculum folder) - `GroupBy`, `Sum`, `Where`, `OrderBy`, `Average`/ratio calculation.

## Architecture

```
10-capstone-personal-ledger/
  Reports/
    LedgerReports.cs                (static class or instance class with pure LINQ methods)
    ReportModels.cs                 (small record types: CategorySpend, MonthlyTotal - return shapes for the report methods)
```

Keep `LedgerReports` stateless (static methods, or a class with no fields) since it has no reason to hold state between calls - avoids an unnecessary instance lifecycle (YAGNI).

## Related Code Files

- Create: `10-capstone-personal-ledger/Reports/LedgerReports.cs`
- Create: `10-capstone-personal-ledger/Reports/ReportModels.cs`
- Modify: `10-capstone-personal-ledger/Program.cs` (temporary manual invocation to print each report - real menu wiring happens in Phase 4)

## Implementation Steps

1. Add `Reports/ReportModels.cs`: `record CategorySpend(ExpenseCategory Category, Money Total)`, `record MonthlyTotal(int Year, int Month, Money Total)`.
2. Add `Reports/LedgerReports.cs` with static methods:
   - `SpendByCategory(IEnumerable<Expense> expenses)` → `expenses.GroupBy(e => e.Category).Select(g => new CategorySpend(g.Key, new Money(g.Sum(e => e.Amount.Amount)))).OrderByDescending(c => c.Total.Amount)`.
   - `MonthlyTotals(IEnumerable<Expense> expenses)` → group by `(e.Date.Year, e.Date.Month)`, sum, order chronologically.
   - `OverdueTasks(IEnumerable<TaskItem> tasks)` → `tasks.Where(t => t.DueDate is { } d && d < DateTime.Now && t.Status != TaskStatus.Done).OrderBy(t => t.DueDate)`.
   - `CompletionRate(IEnumerable<TaskItem> tasks)` → `tasks.Count(t => t.Status == TaskStatus.Done) / (double)tasks.Count() * 100` (guard divide-by-zero when no tasks exist).
3. Use switch expressions/pattern matching where it reads more naturally than an if-chain (e.g. formatting `TaskStatus`/`ExpenseCategory` for console display) - ties back to curriculum phase 09.
4. Temporarily call all four report methods from `Program.cs` after loading data, print results, to verify output before Phase 4 wires them into the real menu.
5. `dotnet build` and `dotnet run --project 10-capstone-personal-ledger`, add a few tasks/expenses spanning categories/months, confirm report numbers are correct by hand-checking.

## Success Criteria

- [ ] `SpendByCategory` correctly sums multiple expenses in the same category and sorts descending by total.
- [ ] `MonthlyTotals` correctly buckets expenses by year+month, ordered chronologically.
- [ ] `OverdueTasks` excludes completed tasks and tasks without a due date, orders by due date.
- [ ] `CompletionRate` returns 0 (not a crash/NaN) when there are zero tasks.
- [ ] `LedgerReports` methods take `IEnumerable<T>` and have no repository/file dependency (pure functions, easy to reason about/test independently).

## Risk Assessment

- **Risk**: Divide-by-zero or null `DueDate` crashing `CompletionRate`/`OverdueTasks` on empty/partial data. **Mitigation**: explicit guard clauses per step 2/success criteria above; test with an empty dataset before Phase 4 ships the menu.
- **Risk**: Report methods creeping into doing I/O or mutation (breaking single-responsibility). **Mitigation**: keep them static/pure - reject any change that adds a repository parameter to `LedgerReports`.
