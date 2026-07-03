---
phase: 5
title: "Records and Pattern Matching"
status: pending
priority: P2
effort: "2h"
dependencies: [4]
---

# Phase 5: Records and Pattern Matching

## Overview
Final phase. Modern C# features that show up constantly in ASP.NET Core DTOs/controllers (records for request/response models, pattern matching in switch expressions for control flow). Shorter phase - fewer, denser exercises since this builds directly on phases 2-4.

## Requirements
- Functional: worked-example-then-TODO, consistent with phase 3/4 pattern.
- Non-functional: at least one exercise explicitly contrasts a `record` DTO against the `class` from phase 2 to make the "why records for data" case concrete.

## Architecture
Folder `09-records-pattern-matching/`. Same `static class` + `Run()` pattern, TODO/Solution pairing continues.

## Related Code Files
- Create: `09-records-pattern-matching/RecordsBasics.cs` - `record` declaration, positional syntax, value equality vs class reference equality (direct callback to phase 2's `ValueVsReferenceTypes.cs`)
- Create: `09-records-pattern-matching/RecordsWithMutation.cs` - `with` expression for non-destructive updates, immutability rationale
- Create: `09-records-pattern-matching/SwitchExpressionsTodo.cs` + `SwitchExpressionsTodoSolution.cs` - TODO: rewrite an if/else chain as a switch expression with pattern matching (type patterns, property patterns)
- Modify: `Program.cs` - register 3 runnable menu entries

## Implementation Steps
1. Write `09-records-pattern-matching/RecordsBasics.cs`: `record Point(double X, double Y)`, compare two instances with same values via `==`, then instantiate the phase-2 `PointRef` class with same values and show `==` is reference equality there - direct side-by-side contrast.
2. Write `09-records-pattern-matching/RecordsWithMutation.cs`: `with` expression producing a modified copy, print both original and copy to show original is untouched.
3. Write `09-records-pattern-matching/SwitchExpressionsTodo.cs` worked half (a switch expression with type patterns over a small class hierarchy from phase 2) then TODO half (extend it with a property pattern, e.g. matching on a record's field range) + Solution.
4. Register all 3 in `Program.cs`, build, run each, confirm output.
5. Final pass: build entire project, run through full menu once end-to-end as a capstone check.

## Success Criteria
- [ ] All 3 exercises build/run; TODO matches Solution behavior
- [ ] `RecordsBasics.cs` output demonstrably shows value equality (records) vs reference equality (class) side by side
- [ ] Full project builds clean, all ~20 exercises reachable from the menu
- [ ] User can read an unfamiliar ASP.NET Core controller snippet using records + pattern matching without needing syntax explained (informal self-check, not automated)

## Risk Assessment
- Low risk, smallest phase, builds only on already-covered phase 2 concepts.
- After this phase: resume ASP.NET Core learning as a separate plan - explicitly out of scope here per brainstorm.
