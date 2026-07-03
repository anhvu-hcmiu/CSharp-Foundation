# Phase 3: Generics and LINQ Complete

**Date**: 2026-07-02  
**Phase**: 3 of 5  
**Status**: Completed  
**Duration**: ~2.5 hours (implementation + code-reviewer fixes)  

## Summary

Generics and LINQ fundamentals delivered. Seven files created and integrated; all 6 planned runnable exercises wired into menu. Code-reviewer surfaced two implementation gaps (SimpleStack<T> not truly built from scratch, Peek() missing exception guard); both fixed before closure. Deferred execution behavior verified in output.

## What Was Built

- **04-generics/GenericMethods.cs**: Generic method `Max<T>(T a, T b) where T : IComparable<T>`. Demonstrates why constraint is necessary (unconstrained version can't call `<` operator).
- **04-generics/GenericClasses.cs**: Generic `Stack<T>` class with `Push()`, `Pop()`, `Peek()`. Initially wrapped `List<T>`; redesigned to use raw `T[]` backing with manual `Array.Resize()`-based doubling to show what `List<T>` actually does under the hood per plan spec.
- **04-generics/GenericCollectionsBuiltIn.cs**: Quick survey of `List<T>`, `Dictionary<TKey, TValue>`, `HashSet<T>` with comments mapping to JS equivalents (`Array`, `Object`/`Map`, `Set`).
- **05-collections-linq/LinqMethodSyntaxBasics.cs**: Sample `List<Person>` with `.Where()`, `.Select()`, `.OrderBy()` chain. Output confirms method-syntax LINQ works; each method has JS-equivalent comment.
- **05-collections-linq/LinqDeferredExecution.cs**: Worked example building an `IEnumerable<T>` query, mutating source `List<int>`, enumerating twice, and confirming second pass reflects the mutation. Output printout proves the "gotcha" JS devs encounter with lazy evaluation.
- **05-collections-linq/LinqAggregatesTodo.cs** + **LinqAggregatesTodoSolution.cs**: TODO pattern introduced. File 1 has method stubs throwing `NotImplementedException`; file 2 has full implementation. Intentionally left unimplemented for user to fill in (not registered in menu).
- **05-collections-linq/LinqQuerySyntaxComparison.cs**: Same query in method-syntax and query-syntax side by side. Comment notes query syntax is rare in real ASP.NET Core.
- **Program.cs**: Registered 6 menu entries (all except TODO/Solution pair); build and manual run of each confirmed output.

## Verification

- **Build**: `dotnet build` succeeded, 0 warnings, 0 errors.
- **Manual testing**: Ran all 6 registered exercises; deferred-execution output showed the mutated value (e.g., adding 999 to list, re-enumerating query showed 999 in results on second pass).
- **Code-reviewer audit**: Found 2 medium-severity gaps:
  1. **SimpleStack<T> implementation**: Plan specified "built from scratch... illustrates what List<T> is doing under the hood"; initial implementation just wrapped `List<T>`, violating that intent.
  2. **Peek() API contract**: `Pop()` threw `InvalidOperationException` on empty, but `Peek()` silently returned default. Inconsistent contract.
- **Fixes applied**:
  1. SimpleStack<T> now uses private `T[]` field with manual `Array.Resize()` on capacity overflow (doubling strategy), matching `List<T>` behavior.
  2. Peek() now throws `InvalidOperationException` on empty, matching `Pop()`.
- **Post-fix verification**: Build and manual test re-run; no behavior change to user-facing output.
- **Plan sync**: `phase-03-generics-and-linq.md` status set to `completed`. Success criteria checkboxes: 2 of 4 checked (generics build/run, deferred-execution demo verified); 2 intentionally left unchecked (TODO exercise pending user implementation, comprehension check is user responsibility, not code artifact).

## Key Technical Decisions

- **SimpleStack<T> from scratch**: Chose `T[]` + `Array.Resize()` instead of `List<T>` wrapper to directly expose the growth mechanic, making the connection to internal `List<T>` behavior explicit. Doubling strategy matches CLR list implementation.
- **Peek() guard consistency**: Added `InvalidOperationException` to match `Pop()`, enforcing users handle empty-stack case. Quieter failure (`default`) would hide logic errors.
- **TODO/Solution pattern introduction**: Deferred full TODO implementation to user; provided worked solution file for reference. Keeps exercise honest (no inline peeking) while supporting stuck learners.
- **Query syntax minimal**: Limited to one comparison exercise per plan (method syntax dominates in real ASP.NET Core code); avoids over-teaching a feature rarely used in practice.
- **Deferred execution demo emphasis**: Prioritized showing the re-evaluation behavior in output over explaining the mechanism; target audience (JS devs) expects eager `.map`/`.filter`, so empirical proof is more impactful.

## Next Steps

- User to implement `LinqAggregatesTodo.cs` (`.Sum()`, `.Count()`, `.GroupBy()`, `.Aggregate()` against sample dataset) and diff output against `LinqAggregatesTodoSolution.cs`.
- Phase 4 (Delegates & Events) ready to start; no blockers from Phase 3.
- No commits made; user to commit when ready.

## Lessons for Future Self

The code-reviewer audit caught a real contract inconsistency (Peek vs Pop) and an intent mismatch (SimpleStack should show the metal, not hide behind List). Don't skip implementation details just because they're not strictly *required* — the learning value (JS dev understanding CLR growth strategy) depends on honest exposition. The TODO/Solution pattern works; user won't fill it in this session but has everything needed to do so later without frustration. Deferred execution is the key LINQ insight for this cohort; the output proof (recomputed query) is worth the extra line count in the exercise.
