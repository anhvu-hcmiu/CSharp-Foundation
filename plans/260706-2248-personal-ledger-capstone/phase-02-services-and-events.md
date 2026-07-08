---
phase: 2
title: "Services and Events"
status: done
priority: P2
dependencies: [1]
effort: "M"
---

# Phase 2: Services and Events

## Overview

Add the business-logic layer on top of Phase 1's repositories: `TaskService` and `ExpenseService`. These own validation, state transitions (e.g. marking a task complete), and raise C# events (`TaskOverdue`, `BudgetExceeded`) that a console subscriber observes. This is where OOP encapsulation, exceptions, and delegates/events get exercised for real.

## Requirements

- Functional:
  - `TaskService`: add task, mark complete, list with overdue check (`DueDate < DateTime.Now && Status != Done`).
  - `ExpenseService`: add expense, budget check against a configurable per-category limit.
  - `TaskService` raises `event EventHandler<TaskOverdueEventArgs> TaskOverdue` when an overdue task is detected (on add or on an explicit check call).
  - `ExpenseService` raises `event EventHandler<BudgetExceededEventArgs> BudgetExceeded` when adding an expense pushes a category over its configured limit.
  - A console-side subscriber (can live in `Program.cs` or a small `ConsoleNotifier` class) subscribes to both events and prints an alert message.
- Non-functional:
  - Validation failures (e.g. empty title, negative amount) throw a custom exception type (e.g. `LedgerValidationException`), not generic `Exception`.
  - Services depend on `IRepository<T>` (the interface, not `JsonFileRepository<T>` directly) for testability/decoupling - constructor injection.

## Architecture

```
10-capstone-personal-ledger/
  Services/
    TaskService.cs
    ExpenseService.cs
  Events/
    TaskOverdueEventArgs.cs
    BudgetExceededEventArgs.cs
  Exceptions/
    LedgerValidationException.cs
  Notifications/
    ConsoleNotifier.cs               (subscribes to both services' events)
```

Event flow: `TaskService.AddTaskAsync(...)` persists via repository, then checks overdue condition; if true, raises `TaskOverdue`. `ExpenseService.AddExpenseAsync(...)` persists, sums existing expenses in that category (via repository `GetAllAsync` + LINQ `Where`/`Sum`), and if the running total exceeds a limit (hardcoded budget dictionary per category is fine - YAGNI, no config file), raises `BudgetExceeded`. `ConsoleNotifier` is constructed with references to both services and subscribes in its constructor - demonstrates delegates/events without over-engineering a pub/sub bus.

## Related Code Files

- Create: `10-capstone-personal-ledger/Services/TaskService.cs`
- Create: `10-capstone-personal-ledger/Services/ExpenseService.cs`
- Create: `10-capstone-personal-ledger/Events/TaskOverdueEventArgs.cs`
- Create: `10-capstone-personal-ledger/Events/BudgetExceededEventArgs.cs`
- Create: `10-capstone-personal-ledger/Exceptions/LedgerValidationException.cs`
- Create: `10-capstone-personal-ledger/Notifications/ConsoleNotifier.cs`
- Modify: `10-capstone-personal-ledger/Program.cs` (wire up services + notifier, replace Phase 1 smoke test)

## Implementation Steps

1. Add `Exceptions/LedgerValidationException.cs` extending `Exception` with a message-only constructor.
2. Add `Events/TaskOverdueEventArgs.cs` (`TaskItem Task`) and `Events/BudgetExceededEventArgs.cs` (`ExpenseCategory Category, Money Total, Money Limit`), both extending `EventArgs`.
3. Add `Services/TaskService.cs`: constructor takes `IRepository<TaskItem>`; methods `AddTaskAsync(title, dueDate, priority)` (validates non-empty title, throws `LedgerValidationException` otherwise), `CompleteTaskAsync(id)`, `GetAllAsync()`, `GetOverdueAsync()` (LINQ filter). Declare and raise `TaskOverdue` event when `AddTaskAsync` detects the new task is already overdue, or expose a `CheckOverdueAsync()` that raises the event for each overdue task found.
4. Add `Services/ExpenseService.cs`: constructor takes `IRepository<Expense>` + a budget limits dictionary (`Dictionary<ExpenseCategory, Money>`, hardcoded default in constructor). `AddExpenseAsync(amount, category, date)` validates non-negative amount, persists, computes category total via `GetAllAsync()` + LINQ, raises `BudgetExceeded` if total > limit for that category.
5. Add `Notifications/ConsoleNotifier.cs`: constructor takes `TaskService`, `ExpenseService`; subscribes to both events in constructor; handlers print a formatted console warning.
6. Update `Program.cs`: replace Phase 1 smoke test with real construction - `JsonFileRepository<TaskItem>`, `JsonFileRepository<Expense>`, `TaskService`, `ExpenseService`, `ConsoleNotifier` - then run a short scripted sequence (add a task with a past due date, add an expense that exceeds budget) to prove both events fire, printed to console.
7. `dotnet build` and `dotnet run --project 10-capstone-personal-ledger` to confirm event output appears.

## Success Criteria

- [x] Adding a task with a past `DueDate` triggers a visible `TaskOverdue` console message via the event, not a direct method call.
- [x] Adding an expense that exceeds the hardcoded category budget triggers a visible `BudgetExceeded` console message via the event.
- [x] Invalid input (empty task title, negative expense amount) throws `LedgerValidationException` with a clear message, caught and displayed by `Program.cs` (not an unhandled crash).
- [x] `TaskService`/`ExpenseService` depend on `IRepository<T>` (interface), not `JsonFileRepository<T>` (concrete type), in their constructors.
- [x] Existing Phase 1 smoke-test behavior (data persists across runs) still holds.

**Validation:** `dotnet build CSharpFoundation.sln` (0 warnings/errors) + `dotnet run --project 10-capstone-personal-ledger` confirmed both events fire and both validation exceptions are caught. Reviewed by `code-reviewer` subagent — 0 critical/high findings.

## Risk Assessment

- **Risk**: Event-raising logic accidentally duplicated between add-time and an explicit check method, causing double alerts. **Mitigation**: pick one trigger point per event (add-time is simplest and sufficient) and document it in code via the method name (`AddTaskAsync` raises on-add; no separate polling method needed - YAGNI unless later phase needs a manual "check all" command).
- **Risk**: Budget limits hardcoded in `ExpenseService` constructor could look like they need to be configurable. **Mitigation**: explicitly out of scope - no config file/CLI flag for limits unless the user asks in a later phase.
- **Risk**: Circular dependency between `ConsoleNotifier` and services if notifier is passed back into service constructors. **Mitigation**: one-directional - services expose events, notifier only subscribes, never the reverse.
