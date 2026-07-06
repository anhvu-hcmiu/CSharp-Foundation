# Phase 4: Delegates Exceptions Nullable and Async Complete

**Date**: 2026-07-03  
**Phase**: 4 of 5  
**Status**: Completed  
**Duration**: ~3 hours (implementation + code-reviewer fixes)  

## Summary

Delegates, exceptions, nullable reference types, and async fundamentals delivered across three folders. Nine files created and integrated; all 6 planned runnable exercises wired into menu. Code-reviewer surfaced four issues (async void non-determinism, JS comparison inaccuracies, contradictory comment, out-of-scope multicast chaining); all fixed before closure. Task.WhenAll parallelism verified quantitatively: 152–156ms vs. 465–477ms sequential.

## What Was Built

- **06-delegates-events/FuncActionPredicate.cs**: Demonstrates `Func<T, TResult>`, `Action<T>`, `Predicate<T>` as typed callback patterns with JS-equivalent comments.
- **06-delegates-events/CustomDelegatesAndEvents.cs**: Custom `delegate` declaration, `event` keyword, and basic publisher/subscriber pattern (worked half + TODO/Solution pair).
- **07-exceptions-nullable/TryCatchFinally.cs**: Exception hierarchy, ordered catch blocks (specific-to-general), `finally`, custom `InsufficientFundsException` class.
- **07-exceptions-nullable/NullableReferenceTypes.cs**: Nullable reference type operators (`?`, `??`, `?.`), nullable warnings, `ArgumentNullException.ThrowIfNull()` pattern (worked half + TODO/Solution pair).
- **08-async-await/TaskBasics.cs**: `Task`/`Task<T>`, `await`, explicit commentary on why `async void` is dangerous and `Task` is not auto-started like JS Promises.
- **08-async-await/AsyncPatternsTodo.cs** + **AsyncPatternsTodoSolution.cs**: Sequential vs. parallel `await` comparison using `Task.WhenAll` with `Stopwatch` timing (worked half + TODO/Solution pair).
- **Program.cs**: Registered 6 menu entries; build clean, manual run verified all exercises.

## Verification

- **Build**: `dotnet build` succeeded, 0 warnings (excluding pre-existing unrelated warning).
- **Manual testing**: Ran all 6 registered exercises; Task.WhenAll parallel exercise measured 152–156ms vs. 465–477ms sequential, confirming concurrency benefit.
- **Nullable reference type warnings**: Temporarily forced CS8602 via intentional nullable misuse, verified warning in build output, then reverted cleanly.
- **Code-reviewer audit**: Found 4 issues:
  1. **Async void demo non-determinism**: Exercise relied on fire-and-forget behavior; console output timing unreliable in demo context.
  2. **JS Promise comparison error**: Comment incorrectly stated null becomes "undefined" in template literal (actually becomes "null").
  3. **Contradictory Task/Promise "hotness" comment**: Stated both that `Task` is lazy and eager in adjacent lines.
  4. **Out-of-scope multicast chaining**: CustomDelegatesAndEvents worked half included `+=` multicast, which exceeds "basic pub/sub only" spec.
- **Fixes applied**:
  1. Added `Thread.Sleep(50)` to async void demo with comment noting this is demonstration-only workaround, not production pattern.
  2. Corrected JS comparison: null in template literal becomes "null".
  3. Rewrote "hotness" comment to clarify `Task` execution model vs. `Promise`.
  4. Removed multicast chaining from worked half; kept pub/sub single-subscriber pattern.
- **Post-fix verification**: Build and manual test re-run; behavior unchanged, audit resolved.
- **Plan sync**: `phase-04-delegates-exceptions-nullable-and-async.md` status set to `completed`. Success criteria: 3 of 4 checked (build/run verified, parallelism quantified, nullable warnings demonstrated); 1 pending (user comprehension check: "state async void pitfall in one sentence" — not implementation, not code-artifact-verifiable).

## Key Technical Decisions

- **Async void demo safety**: Used demo-only `Thread.Sleep()` guard instead of real event handler to avoid confusing fire-and-forget behavior in a teaching context; commented why this is not production-ready.
- **Task.WhenAll timing emphasis**: Opted for explicit `Stopwatch` measurement (152ms vs. 465ms) over theory; target audience (JS devs) expects concrete proof of parallelism benefit.
- **Nullable reference type demo**: Intentional CS8602 violation + fix approach shows warning system in action without requiring production-grade null-safety refactor.
- **Event/pub-sub scope boundary**: Stopped at single-subscriber basic pattern per plan; explicitly rejected multicast edge cases and weak-event patterns.
- **JS comparison honesty**: Corrected null-literal and Promise-hotness comments when audit flagged inaccuracies; teaching credibility depends on technical precision.

## Next Steps

- User to state async void pitfall in one sentence (comprehension self-check, not code artifact).
- Phase 5 (Records & Pattern Matching) ready to start; no blockers from Phase 4.
- No commits made; changes remain uncommitted on main per user request.

## Lessons for Future Self

Code-reviewer audit caught real credibility issues (incorrect JS comparisons, contradictory comments) alongside legitimate scope scope violations (multicast chaining). Don't assume async demo safety; fire-and-forget timing in console apps is inherently racy without explicit coordination. The quantitative parallelism proof (stopwatch timing) is more persuasive than explanation for this cohort. CS8602 nullable-reference warnings are real and visible; demo them without over-engineering the fix. Trusted learning material requires correcting myself when audited, even on "minor" comments — inaccuracy erodes credibility faster than technical depth.
