---
phase: 1
title: "Solution Scaffolding and Models"
status: done
priority: P2
dependencies: []
effort: "M"
---

# Phase 1: Solution Scaffolding and Models

## Overview

Stand up the multi-project solution, scaffold `PersonalLedger.csproj`, and build the domain models (`TaskItem`, `Expense`, supporting enums/value types) plus the generic repository abstraction (`IRepository<T>`, `JsonFileRepository<T>`). This phase produces the foundation everything else builds on: no UI/services yet, but the app should compile and a throwaway smoke-test call should prove the generic repository round-trips both entity types to/from JSON.

## Requirements

- Functional:
  - `CSharpFoundation.sln` at repo root referencing both `CSharpExercises.csproj` (existing) and new `PersonalLedger.csproj`.
  - `TaskItem` and `Expense` as records with nullable/enum fields per design.
  - `IRepository<T>` generic interface: `GetAllAsync`, `GetByIdAsync`, `AddAsync`, `UpdateAsync`, `DeleteAsync`.
  - `JsonFileRepository<T>` generic implementation using `System.Text.Json`, async file read/write, one JSON file per entity type.
- Non-functional:
  - No ASP.NET Core/EF Core dependencies (pure console/BCL only).
  - Nullable reference types enabled (match existing `CSharpExercises.csproj` settings).
  - Existing `CSharpExercises.csproj` and its exercises must remain unmodified and runnable.

## Architecture

```
CSharpFoundation.sln
CSharpExercises.csproj              (existing, untouched)
10-capstone-personal-ledger/
  PersonalLedger.csproj
  Program.cs                        (placeholder Main for this phase - smoke test only)
  Models/
    TaskItem.cs                     (record: Id, Title, DueDate DateTime?, Priority, Status)
    Expense.cs                      (record: Id, Amount Money, Category, Date DateTime)
    Enums.cs                        (TaskPriority, TaskStatus, ExpenseCategory)
    Money.cs                        (readonly record struct wrapping decimal)
  Repositories/
    IRepository.cs                  (generic interface)
    JsonFileRepository.cs           (generic impl)
  Data/
    tasks.json                      (empty array seed: [])
    expenses.json                   (empty array seed: [])
```

`TaskItem`/`Expense` both need a stable `Id` (Guid) so `IRepository<T>` can do `GetByIdAsync`/`UpdateAsync`/`DeleteAsync` generically without per-entity key logic - use a shared marker interface `IEntity { Guid Id }` that both records implement, so `JsonFileRepository<T>` can be constrained with `where T : IEntity`.

`JsonFileRepository<T>` loads the whole file into memory, mutates the in-memory `List<T>`, and rewrites the whole file on save (no partial writes/locking - single-user app, YAGNI). File path passed into constructor so the same class serves both `tasks.json` and `expenses.json`.

## Related Code Files

- Create: `CSharpFoundation.sln`
- Create: `10-capstone-personal-ledger/PersonalLedger.csproj`
- Create: `10-capstone-personal-ledger/Program.cs`
- Create: `10-capstone-personal-ledger/Models/TaskItem.cs`
- Create: `10-capstone-personal-ledger/Models/Expense.cs`
- Create: `10-capstone-personal-ledger/Models/Enums.cs`
- Create: `10-capstone-personal-ledger/Models/Money.cs`
- Create: `10-capstone-personal-ledger/Models/IEntity.cs`
- Create: `10-capstone-personal-ledger/Repositories/IRepository.cs`
- Create: `10-capstone-personal-ledger/Repositories/JsonFileRepository.cs`
- Create: `10-capstone-personal-ledger/Data/tasks.json`
- Create: `10-capstone-personal-ledger/Data/expenses.json`
- Modify: none in existing `CSharpExercises.csproj` tree

## Implementation Steps

1. Run `dotnet new sln -n CSharpFoundation` at repo root; `dotnet sln add CSharpExercises.csproj`.
2. `dotnet new console -o 10-capstone-personal-ledger -n PersonalLedger --framework net10.0`; enable `ImplicitUsings`/`Nullable` in the new `.csproj` to match existing project; `dotnet sln add 10-capstone-personal-ledger/PersonalLedger.csproj`.
3. Add `Models/IEntity.cs`: `interface IEntity { Guid Id { get; } }`.
4. Add `Models/Enums.cs`: `TaskPriority` (Low/Medium/High), `TaskStatus` (Pending/InProgress/Done), `ExpenseCategory` (Food/Housing/Transport/Entertainment/Other).
5. Add `Models/Money.cs`: `readonly record struct Money(decimal Amount)` with basic validation (no negative amounts - throw in constructor or a factory).
6. Add `Models/TaskItem.cs` and `Models/Expense.cs` as records implementing `IEntity`, with `Guid Id` defaulted via `Guid.NewGuid()` at creation call sites (not inside the record itself, to keep records simple data carriers).
7. Add `Repositories/IRepository.cs`: `interface IRepository<T> where T : IEntity` with `GetAllAsync/GetByIdAsync/AddAsync/UpdateAsync/DeleteAsync` returning `Task<...>`.
8. Add `Repositories/JsonFileRepository.cs`: generic class implementing `IRepository<T>`, constructor takes file path, uses `System.Text.Json.JsonSerializer` with async `File.ReadAllTextAsync`/`WriteAllTextAsync`, in-memory `List<T>` cache reloaded per call (simplest correct approach - no caching complexity, YAGNI).
9. Seed `Data/tasks.json` and `Data/expenses.json` with `[]`.
10. Write a temporary smoke test in `Program.cs` (Main): construct `JsonFileRepository<TaskItem>` and `JsonFileRepository<Expense>`, add one record of each, read them back, print to console. This proves generics + async + JSON round-trip before Phase 2 builds on it.
11. `dotnet build` the solution; `dotnet run --project 10-capstone-personal-ledger` to confirm the smoke test output.

## Success Criteria

- [x] `dotnet build` succeeds for the whole solution (`CSharpFoundation.sln`), both projects.
- [x] `dotnet run --project CSharpExercises.csproj` still launches the existing exercises menu unchanged.
- [x] `dotnet run --project 10-capstone-personal-ledger` runs the smoke test: adds one `TaskItem` and one `Expense`, persists to `tasks.json`/`expenses.json`, reads them back, prints correctly.
- [x] Same `JsonFileRepository<T>` class instantiated for both `TaskItem` and `Expense` with no entity-specific branching inside it (proves generics reuse).
- [x] `tasks.json`/`expenses.json` are valid JSON after a run (inspect manually).

**Validation:** `plans/reports/tester-260706-2337-phase-01-solution-scaffolding-validation-report.md` — all criteria PASS.

## Risk Assessment

- **Risk**: `dotnet new sln`/`dotnet sln add` CLI commands may differ slightly by SDK version. **Mitigation**: verify with `dotnet --version` first; fall back to hand-writing the `.sln` if needed (well-documented format).
- **Risk**: Money/negative-amount validation could balloon into a full value-object framework. **Mitigation**: keep `Money` to a single constructor guard clause - no operators/formatting beyond what Phase 3 reports need.
- **Risk**: Over-generalizing `IRepository<T>` (e.g., adding query/filter methods now). **Mitigation**: only the 5 CRUD methods listed; filtering/reporting logic belongs in Phase 3 (LINQ over `GetAllAsync()` results), not the repository.
