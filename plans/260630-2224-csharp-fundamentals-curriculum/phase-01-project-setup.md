---
phase: 1
title: "Project Setup"
status: completed
priority: P1
effort: "30m"
dependencies: []
---

# Phase 1: Project Setup

## Overview
Scaffold the single console project and the menu dispatcher that all later exercises plug into. No C# teaching content here - pure structure.

## Requirements
- Functional: `dotnet run` from repo root shows a numbered menu of available exercises and runs the chosen one.
- Non-functional: adding a new exercise later = add one file + one menu entry, no project restructuring.

## Architecture
Single console app (`CSharpExercises.csproj`, target `net10.0`). Each exercise is a `static class` with a `public static void Run()` method in its topic folder. `Program.cs` holds a `Dictionary<string, Action>` or `switch` mapping menu keys to `Run()` calls, prints a menu, reads `Console.ReadLine()`, dispatches, loops until exit.

## Related Code Files
- Create: `CSharpExercises.csproj`
- Create: `Program.cs`
- Create: `01-types-and-syntax/.gitkeep` through `09-records-and-pattern-matching/.gitkeep` (empty topic folders, populated in later phases)

## Implementation Steps
1. `dotnet new console -n CSharpExercises -o .` in `d:\PersonalLearning\C#Learning` (or manually write `.csproj` + `Program.cs` if `dotnet new` scaffolds unwanted files - check output first).
2. Confirm `CSharpExercises.csproj` targets `net10.0`, `Nullable` enabled (`<Nullable>enable</Nullable>`) - nullable reference types matter for phase 4.
3. Create 9 topic folders: `01-types-and-syntax`, `02-oop-core`, `03-interfaces-inheritance`, `04-generics`, `05-collections-linq`, `06-delegates-events`, `07-exceptions-nullable`, `08-async-await`, `09-records-pattern-matching`.
4. Write `Program.cs` with a menu loop: print numbered list of registered exercises (label = topic + exercise name), read selection, invoke `Run()`, return to menu after exercise completes (don't exit process), `0` or `q` to quit.
5. Add one placeholder exercise (e.g. `01-types-and-syntax/HelloTypes.cs` with a trivial `Run()` that prints a line) wired into the menu, to prove the dispatch loop works end-to-end before phase 2 adds real content.
6. Run `dotnet build` then `dotnet run`, select the placeholder, confirm output prints and menu loop returns.

## Success Criteria
- [x] `dotnet build` succeeds with zero warnings/errors
- [x] `dotnet run` shows a menu, running the placeholder exercise prints expected output and returns to menu
- [x] All 9 topic folders exist (even if empty except phase-1 placeholder)
- [x] Nullable reference types enabled in `.csproj`

## Risk Assessment
- `dotnet new console` may scaffold a `Program.cs` with top-level statements that conflicts with the menu-dispatcher pattern - rewrite `Program.cs` fully rather than patching the template output.
- Low risk overall: no external dependencies, no network/DB, single local project.
