---
phase: 4
title: "Console UI and Persistence Polish"
status: pending
priority: P2
dependencies: [1, 2, 3]
effort: "M"
---

# Phase 4: Console UI and Persistence Polish

## Overview

Wire everything into a real console menu loop (own `Program.cs`, not another entry in the exercises dispatcher). Replace the temporary manual invocations from Phases 1-3 with an interactive menu: add/list/complete tasks, add/list expenses, view reports, quit - with data loading on start and saving on exit (or per-action, since `JsonFileRepository<T>` already persists per call). This phase is the "consolidation" payoff: every prior phase's pieces get exercised through one coherent user flow.

## Requirements

- Functional:
  - Menu loop mirroring the style of the existing `Program.cs` dispatcher (numbered options, loop until quit) but standalone for this project.
  - Options: Add Task, List Tasks (with overdue flagged), Complete Task, Add Expense, List Expenses, View Reports (all 4 from Phase 3), Quit.
  - Console input parsing with basic validation (re-prompt on bad input, don't crash).
- Non-functional:
  - Catch `LedgerValidationException` at the menu boundary and print a friendly message instead of propagating/crashing.
  - No new business logic here - this phase only wires UI to the services/reports built in Phases 1-3.

## Architecture

```
10-capstone-personal-ledger/
  Program.cs                        (final version: menu loop, wires repositories -> services -> notifier -> reports)
  Menu/
    LedgerMenu.cs                   (optional extraction if Program.cs grows past ~150-200 lines; otherwise keep inline per repo's own modularization threshold)
```

Follow the existing repo convention from root `Program.cs` (array of labeled actions + `while(true)` loop with numeric selection) for familiarity, but this project's loop lives entirely inside `10-capstone-personal-ledger/` and does not touch the root dispatcher.

## Related Code Files

- Modify: `10-capstone-personal-ledger/Program.cs` (final menu loop; supersedes Phase 1-3 temporary smoke-test/manual-invocation code)
- Create (if `Program.cs` exceeds ~200 lines): `10-capstone-personal-ledger/Menu/LedgerMenu.cs`

## Implementation Steps

1. In `Program.cs`, construct the full dependency chain once at startup: `JsonFileRepository<TaskItem>`, `JsonFileRepository<Expense>`, `TaskService`, `ExpenseService`, `ConsoleNotifier`.
2. Build the menu loop: print numbered options, read `Console.ReadLine()`, dispatch via switch expression (ties back to curriculum phase 09 pattern matching) to handler methods.
3. Add Task handler: prompt title/due date (optional, parse `DateTime?`)/priority (parse enum, re-prompt on invalid), call `TaskService.AddTaskAsync`, catch `LedgerValidationException` and print message.
4. List Tasks handler: call `TaskService.GetAllAsync()`, print each with status; mark overdue ones distinctly (reuse `LedgerReports.OverdueTasks` or a simple inline check).
5. Complete Task handler: list tasks with index/id, prompt selection, call `TaskService.CompleteTaskAsync`.
6. Add Expense handler: prompt amount/category/date, call `ExpenseService.AddExpenseAsync`, catch validation exception.
7. List Expenses handler: call repository/service `GetAllAsync()`, print each.
8. View Reports handler: call all four `LedgerReports` methods, print formatted output (this replaces the temporary Phase 3 manual calls).
9. Quit: exit loop cleanly (data already persisted per-action via `JsonFileRepository<T>`, so no special save-on-exit logic needed beyond confirming nothing is buffered in memory only).
10. If `Program.cs` grows unwieldy (rough threshold: 200 lines per repo's own modularization guidance), extract the menu loop into `Menu/LedgerMenu.cs` and keep `Program.cs` as a thin composition root.
11. Manual end-to-end pass: run the app, add 2-3 tasks (one overdue), add 2-3 expenses (one triggering budget alert), complete a task, view all reports, quit, restart the app, confirm all data reloads correctly.

## Success Criteria

- [ ] `dotnet run --project 10-capstone-personal-ledger` launches an interactive menu, not the Phase 1-3 smoke test/manual calls.
- [ ] Full user flow works end-to-end: add task/expense → list → complete task → view reports → quit → restart → data still present.
- [ ] Invalid menu input (non-numeric, out-of-range) re-prompts instead of crashing.
- [ ] `LedgerValidationException` from services is caught at the menu layer with a clear message, app continues running.
- [ ] `TaskOverdue`/`BudgetExceeded` console alerts (from Phase 2) still visibly fire during normal menu use.
- [ ] Existing `CSharpExercises.csproj` dispatcher (exercises 01-09) is unaffected - still runs standalone via `dotnet run --project CSharpExercises.csproj`.
- [ ] All success criteria from Phases 1-3 remain true (regression check after wiring).

## Risk Assessment

- **Risk**: Menu-layer input parsing (dates, enums) turning into a large ad-hoc parsing mess. **Mitigation**: small dedicated `TryParse`-style helper methods per input type, re-prompt loop pattern consistent across all fields - don't invent a generic input-validation framework (YAGNI).
- **Risk**: Scope creep - "polish" could invite unplanned features (undo, search, export). **Mitigation**: this phase only wires existing Phase 1-3 capability into a menu; new features are out of scope unless the user explicitly requests a follow-up phase.
- **Risk**: `Program.cs` becoming a god-file mixing composition root + menu + handlers. **Mitigation**: extraction threshold explicitly stated in step 10; don't pre-extract before it's actually needed.
