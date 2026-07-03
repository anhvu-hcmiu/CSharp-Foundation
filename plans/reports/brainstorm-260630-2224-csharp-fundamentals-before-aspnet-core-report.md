# Brainstorm: Pure C# fundamentals before resuming ASP.NET Core

## Problem
User learning ASP.NET Core, struggling to follow C# syntax/semantics during code review (OOP, generics, LINQ specifically). Hypothesis: ASP.NET Core review pain is mostly C# fluency gap, not framework gap (~80% of ASP.NET Core code is plain C#). Workspace `d:\PersonalLearning\C#Learning` was empty (no existing code/docs) - greenfield.

## User profile
- Strong JS/React, comfortable with FP style
- Some C# exposure already but OOP/generics/LINQ feel unstable - these concepts are thin or absent in typical React/FP code
- .NET 10 SDK installed and verified (`dotnet --version` = 10.0.300)

## Decision
Pause ASP.NET Core. Build pure C# fluency via small console exercises first, then resume ASP.NET Core. Isolates "is this confusing because of C# or because of DI/middleware/routing" - removes the confound.

## Curriculum (Core + intermediate depth, skip basic syntax user already knows from JS)
1. Types & syntax mapping (short) - static typing, value vs reference types, `var`
2. OOP core - classes, constructors, properties/auto-properties, access modifiers
3. Interfaces & inheritance - abstract classes, polymorphism, `virtual`/`override`
4. Generics - `T`, constraints, generic collections
5. Collections & LINQ - `List<T>`, `Dictionary<TKey,TValue>`, method syntax vs query syntax, deferred execution
6. Delegates/events, `Func`/`Action` (expected to click fast - maps to JS callbacks)
7. Exceptions, nullable reference types, null-conditional `?.`/`??`
8. async/await, `Task`/`Task<T>` (maps to Promises, but cover real differences e.g. sync context)
9. Records & pattern matching (shows up constantly in ASP.NET Core DTOs)

Weight exercise count toward steps 2-5 - likely source of most review confusion.

## Structure (approved)
Single console project, NOT separate .csproj per topic (avoids boilerplate overhead):
```
d:\PersonalLearning\C#Learning\
├── CSharpExercises.csproj
├── Program.cs                  # menu dispatcher, pick exercise by number
├── 01-types-and-syntax/
├── 02-oop-core/
├── 03-interfaces-inheritance/
├── 04-generics/
├── 05-collections-linq/
├── 06-delegates-events/
├── 07-exceptions-nullable/
├── 08-async-await/
└── 09-records-pattern-matching/
```
Each exercise file = one static class with `Run()` method, inline comments explain *why* (not just what).

## Exercise style (approved)
Worked examples first (topics 1-3ish) so user reads/runs without implementation burden while syntax is unfamiliar. Once patterns are familiar, later topics (4+) include TODO blanks for active implementation.

## Alternatives considered, rejected
- Separate .csproj per topic: more realistic to multi-project repos but more navigation/boilerplate overhead for a learning repo - rejected for KISS.
- TODOs from the start: too steep given unfamiliar syntax in early topics - rejected, deferred to later topics.
- Minimal/just-unblock-the-review depth: rejected, user explicitly wants Core + intermediate depth (generics, delegates, async, nullable) since these recur throughout ASP.NET Core code.

## Next steps
Hand off to `/ck:plan` to create phase-by-phase implementation plan (9 topics = roughly 9 phases or grouped phases).

## Unresolved questions
None.
