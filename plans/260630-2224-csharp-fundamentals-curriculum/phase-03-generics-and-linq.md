---
phase: 3
title: "Generics and LINQ"
status: completed
priority: P1
effort: "4h"
dependencies: [2]
---

# Phase 3: Generics and LINQ

## Overview
Generics (TS-adjacent but hits differently without structural typing) and LINQ (maps to `.map`/`.filter`/`.reduce` instincts but with deferred execution as the real gotcha). First phase to introduce TODO blanks - patterns from phase 2 should now be familiar enough to implement, not just read.

## Requirements
- Functional: generics exercises are worked examples (new concept, no JS equivalent to lean on); LINQ exercises are worked-example-then-TODO pairs (concept maps to known JS methods, so active practice is more valuable here).
- Non-functional: LINQ exercises show method syntax primarily (closer to JS `.map`/`.filter` chains); query syntax gets one comparison exercise, not full parity, to avoid redundant teaching.

## Architecture
Folders `04-generics/`, `05-collections-linq/`. Same `static class` + `Run()` pattern. TODO exercises use a sibling `*Solution.cs` file (not committed to the menu) the user can diff against if stuck - keeps the exercise file itself honest (no peeking at inline answers).

## Related Code Files
- Create: `04-generics/GenericMethods.cs` - generic method `T Max<T>(T a, T b) where T : IComparable<T>`, why constraints exist
- Create: `04-generics/GenericClasses.cs` - generic `Stack<T>`-style class built from scratch (illustrates what `List<T>` is doing under the hood)
- Create: `04-generics/GenericCollectionsBuiltIn.cs` - `List<T>`, `Dictionary<TKey,TValue>`, `HashSet<T>` survey
- Create: `05-collections-linq/LinqMethodSyntaxBasics.cs` - `.Where`/`.Select`/`.OrderBy` worked examples, explicit JS-equivalent comment per method
- Create: `05-collections-linq/LinqDeferredExecution.cs` - worked example showing a query re-evaluating on each enumeration (the LINQ gotcha JS devs don't expect)
- Create: `05-collections-linq/LinqAggregatesTodo.cs` + `LinqAggregatesTodoSolution.cs` - TODO: implement `.Sum`/`.Count`/`.GroupBy`/`.Aggregate` against a sample dataset
- Create: `05-collections-linq/LinqQuerySyntaxComparison.cs` - one example written both method-syntax and query-syntax side by side
- Modify: `Program.cs` - register 6 runnable menu entries (TODO file registered once user fills it in; Solution file never registered)

## Implementation Steps
1. Write `04-generics/GenericMethods.cs`: unconstrained generic first, then add `where T : IComparable<T>` constraint with comment on why unconstrained version fails to compile for `<`.
2. Write `04-generics/GenericClasses.cs`: minimal generic `Stack<T>` with `Push`/`Pop`/`Peek`, demo with `int` and `string` instantiations.
3. Write `04-generics/GenericCollectionsBuiltIn.cs`: quick survey of `List<T>`, `Dictionary<TKey,TValue>`, `HashSet<T>` with JS-equivalent comments (`Array`, `Object`/`Map`, `Set`).
4. Write `05-collections-linq/LinqMethodSyntaxBasics.cs`: sample `List<Person>`, run `.Where().Select().OrderBy()` chain, print results.
5. Write `05-collections-linq/LinqDeferredExecution.cs`: build an `IEnumerable<T>` query, mutate the source collection, enumerate twice, show the second enumeration reflects the mutation - this is the concept most likely to confuse a JS dev used to eager `.map`/`.filter`.
6. Write `05-collections-linq/LinqAggregatesTodo.cs` with method stubs throwing `NotImplementedException` and inline comments describing expected behavior; write matching `LinqAggregatesTodoSolution.cs` with the filled-in implementation.
7. Write `05-collections-linq/LinqQuerySyntaxComparison.cs`: same query, two ways, comment noting query syntax is rare in real ASP.NET Core code (so don't over-invest here).
8. Register runnable exercises in `Program.cs`, build, run each, confirm output including the deferred-execution demo actually prints the mutated value.

## Success Criteria
- [x] All generics exercises build/run, constraint violation example documented (not necessarily compiled)
- [x] Deferred execution exercise demonstrably shows re-evaluation behavior in its printed output
- [ ] TODO exercise has user-filled implementation matching `*Solution.cs` behavior (verified by running both and diffing output) - pending: `LinqAggregatesTodo.cs` intentionally left unimplemented for the user to fill in later, per plan design
- [ ] User can explain in one sentence why LINQ queries are lazy by default - pending: user-side comprehension check, not implementation work

## Risk Assessment
- Query syntax over-investment risk: real ASP.NET Core code is dominated by method syntax - cap query syntax to the one comparison exercise per the brainstorm's weighting toward steps 2-5.
- TODO/Solution pattern only introduced now (not phase 2) - if user finds even this premature, fall back to fully-worked and defer TODOs to phase 4.
