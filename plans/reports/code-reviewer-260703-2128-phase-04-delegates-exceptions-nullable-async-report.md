## Code Review Summary

### Scope
- Files: `06-delegates-events/{FuncActionPredicate,CustomDelegatesAndEvents,CustomDelegatesAndEventsSolution}.cs`, `07-exceptions-nullable/{TryCatchFinally,NullableReferenceTypes,NullableReferenceTypesSolution}.cs`, `08-async-await/{TaskBasics,AsyncPatternsTodo,AsyncPatternsTodoSolution}.cs`, `Program.cs` (diff only)
- LOC: ~380 new lines across 9 new files + 9-line diff to `Program.cs`
- Focus: pedagogical correctness for a solo learning curriculum (not production hardening)
- Scout findings: git diff confirms scope is exactly the 9 new files + a 9-line additive `Program.cs` diff (3 `using`s + 6 tuples), no unrelated files touched

### Overall Assessment
Structurally sound and consistent with the phase 3 TODO/Solution convention. Build is clean (0 new warnings; the 1 pre-existing warning in `02-oop-core/PropertiesAndFields.cs` is unrelated and unchanged). Program.cs registration and Solution exclusion are correct. Two real defects found: one factual error in a JS-comparison comment, and one genuine console-output race in the `async void` demo that undermines the exercise's own teaching point.

### Critical Issues
None.

### High Priority
None.

### Medium Priority

**1. `08-async-await/TaskBasics.cs` — fire-and-forget `async void` demo has an unaddressed output race.**

`Run()` calls `RiskyEventHandlerAsync();` (fire-and-forget, no way to await a `void`-returning async method) as its last statement, then returns immediately. The method's own "async void must catch its own exceptions internally: ..." confirmation line races against the caller.

Verified empirically (both in an isolated repro and in the actual app via `dotnet run --no-build` with piped stdin at 50ms and 3s delays between menu selections):
- With short delays, the confirmation line never printed before the process moved to the next menu iteration or exited.
- With a 3s delay it did print, but *after* the next menu's `Select: ` prompt had already been written to the same line — i.e., interleaved into the following screen rather than appearing as part of this exercise's own output block.

This is exactly the kind of non-deterministic behavior the plan's Risk Assessment was trying to avoid ("keep this exercise explanatory... not a literal uncaught crash" — it doesn't crash, but the safe illustration's own proof-of-catch is not reliably observable). A learner who selects this entry and then quickly moves to the next selection or quits may never see the line that demonstrates the mitigation worked, without any comment acknowledging this.

Fix: either block briefly and explain why (e.g. `await Task.Delay(50);` equivalent via a short `Thread.Sleep` after the call, with a comment noting this is purely to make the fire-and-forget output land before `Run()` returns — itself a good teaching moment: you cannot deterministically wait on a fire-and-forget `async void` call, unlike an `async Task`), or add a comment explicitly noting the output timing is best-effort/racy so the learner isn't confused if the line appears late or is absent in fast runs.

**2. `07-exceptions-nullable/NullableReferenceTypes.cs:44` — factual error in JS comparison comment.**

```
// JS has no equivalent guard - a null customerName would just silently become "undefined"
// in a template string instead of failing fast.
```

Incorrect: in JS, a template literal coerces `null` via `String(null)`, which is `"null"`, not `"undefined"`. `` `Welcome, ${null}!` `` produces `"Welcome, null!"`. `"undefined"` would only appear if the parameter were literally omitted/`undefined`, which isn't the scenario being contrasted (the exercise passes `null` explicitly). This is a direct miss against the review criterion "no misleading JS comparisons" — fix the word to `"null"`.

**3. `08-async-await/TaskBasics.cs:7-14` — confusing/self-contradicting comment on `Task` vs `Promise` "hotness".**

The comment first states a Task is "hot" the moment it's created, same as `new Promise(executor)` running its executor synchronously — then pivots to "the real difference is Task.Delay/Task-returning APIs don't auto-start the way `fetch()` kicks off... instantly," which directly contradicts the "hot" framing given two clauses earlier (an async-method-produced `Task` **does** auto-start, same as `fetch()`). The one genuinely correct and useful distinction — that a cold `new Task(...)` (unlike anything `async` methods produce) requires an explicit `.Start()`, with no JS equivalent — is buried under this contradictory run-on. Given phase 4's explicit requirement that every async comment "notes one concrete difference from JS Promises," this comment currently asserts and then un-asserts the same claim, which will confuse rather than clarify. Recommend rewriting to lead with the accurate point (Tasks from `async` methods are hot, same as JS; the real difference is the rare cold `new Task()` + `.Start()` pattern) and drop the contradictory middle clause.

### Low Priority

- `06-delegates-events/CustomDelegatesAndEvents.cs:17-23` (`CustomDelegatesAndEventsWorked.RunWorked`) demonstrates multicast delegate chaining via `+=`. The plan's Risk Assessment says to "stop at basic publisher/subscriber, don't expand into multicast delegate edge cases." This is a single introductory line (not exception-in-chain semantics, `GetInvocationList()`, or return-value-truncation discussion), and it's arguably necessary context since `event +=` relies on the same multicast mechanism — but it is technically outside the literal "basic pub/sub" framing. Borderline; not blocking.
- `07-exceptions-nullable/NullableReferenceTypesSolution.cs` and `08-async-await/AsyncPatternsTodoSolution.cs` are logically identical to their TODO counterparts (no verbose-vs-idiomatic contrast, unlike `CustomDelegatesAndEventsSolution.cs` or phase 3's `LinqAggregatesTodoSolution.cs` `TotalByCategory`). This mirrors an already-established phase 3 precedent (several `LinqAggregatesTodo` methods were also pre-solved rather than blanked stubs), so it's not a new deviation, but the Solution files add no pedagogical value for these two exercises as written.

### Edge Cases Found by Scout
- Fire-and-forget `async void` output race (Medium #1 above) — only surfaces under fast/automated input, not necessarily during normal interactive typing, but is a real, reproducible race with no acknowledging comment.
- Verified nullable-misuse revert is clean: full rebuild (`dotnet build --no-incremental`) shows exactly one warning, in the pre-existing unrelated `02-oop-core/PropertiesAndFields.cs:31` (`CS8618`), confirming no leftover nullable-misuse code and no new warnings from phase 4 files.

### Positive Observations
- `Program.cs` registers exactly the 6 TODO/worked entries and correctly excludes all 3 Solution classes from the menu array — matches phase 3 convention precisely.
- `TryCatchFinally.cs` catch-ordering and rethrow (`throw;` preserving stack trace) behavior verified correct by running all three demo branches (successful, insufficient funds, negative amount) — output matches expected nested-catch-then-rethrow-to-outer-catch semantics.
- `Task.WhenAll` speedup verified via actual run: sequential 477ms vs parallel 152ms (three 150ms delays), consistent with the ~3x speedup the task description reported (465ms/156ms).

### Recommended Actions
1. Fix the `"undefined"` → `"null"` factual error in `NullableReferenceTypes.cs`.
2. Address or explicitly comment the fire-and-forget async void output race in `TaskBasics.cs` (Medium #1).
3. Rewrite the contradictory Task/Promise "hotness" comment in `TaskBasics.cs` for clarity (Medium #3).
4. Optional: note in `CustomDelegatesAndEvents.cs` that the `+=` chaining shown is foundational context for `event`, not a deep-dive into multicast delegates, to make the scope boundary explicit (Low, non-blocking).

### Metrics
- Type Coverage: N/A (not a typed-JS project; `Nullable` reference types enabled project-wide, 0 new nullable warnings)
- Test Coverage: N/A (no automated tests in this repo; verification is manual menu execution)
- Linting Issues: 0 new (1 pre-existing unrelated warning confirmed via full rebuild)

### Plan Follow-ups
All 7 Implementation Steps in `phase-04-delegates-exceptions-nullable-and-async.md` appear complete: all 6 files created, `Program.cs` registers all 6, build/run verified, `Task.WhenAll` speedup confirmed. Success Criteria checklist items are functionally met except the `async void` illustration's reliability (see Medium #1) — the pitfall is explained via comments correctly, but the "safe illustration" doesn't reliably display its own proof within the interactive loop. Recommend fixing Medium #1 and #2 before marking phase 4 fully done; #3 and Low items are polish.

### Unresolved Questions
- Should the fire-and-forget async void race (Medium #1) be treated as blocking for this solo-learning context, or acceptable given a human typist will almost always outlast the 10ms internal delay? Recommend at minimum adding a comment acknowledging the race, per the user's stated preference for accurate comments over silent gaps.
