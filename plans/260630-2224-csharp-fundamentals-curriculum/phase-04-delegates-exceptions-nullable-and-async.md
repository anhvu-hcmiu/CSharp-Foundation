---
phase: 4
title: "Delegates Exceptions Nullable and Async"
status: completed
priority: P2
effort: "4h"
dependencies: [3]
---

# Phase 4: Delegates Exceptions Nullable and Async

## Overview
Three sub-topics expected to click fast since they map to known JS concepts (callbacks, try/catch, Promises) - real C# differences get called out explicitly. Async is the one most directly relevant to ASP.NET Core controllers/services, so gets the most exercise weight. TODO-style exercises continue here.

## Requirements
- Functional: delegates/exceptions exercises worked-example-then-TODO; async exercises worked-example-then-TODO with explicit JS Promise comparison comments.
- Non-functional: every async exercise comment notes one concrete difference from JS Promises (e.g. `Task` is not auto-started the way `fetch()` is, `async void` vs `async Task` pitfall, no automatic `.then()` chaining sugar).

## Architecture
Folders `06-delegates-events/`, `07-exceptions-nullable/`, `08-async-await/`. Same `static class` + `Run()` pattern, same TODO/Solution pairing from phase 3 for exercises 3+.

## Related Code Files
- Create: `06-delegates-events/FuncActionPredicate.cs` - `Func<T,TResult>`, `Action<T>`, `Predicate<T>` as typed callbacks, JS-equivalent comments
- Create: `06-delegates-events/CustomDelegatesAndEvents.cs` - `delegate` keyword, `event` keyword, simple publisher/subscriber TODO exercise + Solution
- Create: `07-exceptions-nullable/TryCatchFinally.cs` - exception hierarchy, `catch` ordering, `finally`, custom exception class
- Create: `07-exceptions-nullable/NullableReferenceTypes.cs` - `?`/`??`/`?.`, nullable warnings, `ArgumentNullException` pattern TODO + Solution
- Create: `08-async-await/TaskBasics.cs` - `Task`/`Task<T>`, `await`, why `async void` is dangerous (event handlers only)
- Create: `08-async-await/AsyncPatternsTodo.cs` + `AsyncPatternsTodoSolution.cs` - TODO: `Task.WhenAll`, sequential vs parallel await, simulated I/O via `Task.Delay`
- Modify: `Program.cs` - register 6 runnable menu entries

## Implementation Steps
1. Write `06-delegates-events/FuncActionPredicate.cs`: pass `Func<int,int,int>` and `Action<string>` as method parameters, comment mapping to JS function-as-value.
2. Write `06-delegates-events/CustomDelegatesAndEvents.cs` worked half (custom `delegate` declaration + invocation) then TODO half (wire an `event` with a subscriber method) + matching Solution file.
3. Write `07-exceptions-nullable/TryCatchFinally.cs`: nested try/catch, multiple catch blocks ordered specific-to-general, custom `InsufficientFundsException : Exception`.
4. Write `07-exceptions-nullable/NullableReferenceTypes.cs` worked half (`?`/`??`/`?.` demo) then TODO half (write a method that throws `ArgumentNullException` via `ArgumentNullException.ThrowIfNull`) + Solution.
5. Write `08-async-await/TaskBasics.cs`: `async Task<T>` method, `await Task.Delay` to simulate I/O, comment contrasting with JS `await fetch()`; second snippet showing why `async void` swallows exceptions (commented explanation, not a literal crash demo).
6. Write `08-async-await/AsyncPatternsTodo.cs` worked half (sequential `await` calls) then TODO half (rewrite using `Task.WhenAll` for parallelism, compare elapsed time via `Stopwatch`) + Solution.
7. Register all 6 in `Program.cs`, build, run each, confirm `Task.WhenAll` TODO solution is measurably faster than sequential.

## Success Criteria
- [x] All exercises build/run; TODO portions match Solution file behavior when filled in
- [x] `Stopwatch`-measured `Task.WhenAll` exercise shows parallel run is faster than sequential equivalent - verified 477ms sequential vs 152ms parallel
- [ ] User can state the `async void` pitfall in one sentence - pending: user-side comprehension check, not implementation work
- [x] Nullable reference type warnings are visible in build output for intentionally-nullable-misused code (then fixed) - verified CS8602 via temporary misuse edit, reverted cleanly

## Risk Assessment
- `async void` demo risk: an actual unhandled-exception crash demo could be confusing/destructive to the menu loop - keep this exercise explanatory via comments + a safe `try/catch`-wrapped illustration, not a literal uncaught crash.
- Events/delegates depth: stop at basic publisher/subscriber, don't expand into multicast delegate edge cases or weak event patterns - out of scope per brainstorm's "core + intermediate" line.
