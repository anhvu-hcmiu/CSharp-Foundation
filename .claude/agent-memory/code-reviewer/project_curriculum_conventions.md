---
name: project-curriculum-conventions
description: Repo-specific conventions for the CSharp-Foundation learning curriculum (numbered topic folders, Todo/Solution pairing)
metadata:
  type: project
---

This repo is a single console app teaching C# fundamentals (topics `01-...` through `09-...`), each folder registered as menu entries in `Program.cs` (array of `(Label, Action Run)`).

Established Todo/Solution convention (seen in phase 04 `08-async-await`, `07-exceptions-nullable`, and phase 05 `09-records-pattern-matching`): a `*Todo.cs` file is NOT a file with blanks/`NotImplementedException` — it already contains the fully worked-out code, just with instructional comments explaining the concept. The paired `*TodoSolution.cs` is a byte-for-byte logic duplicate (only the class name and comments differ), intended for the learner to diff against their own attempt. Solution files are deliberately **not** registered in `Program.cs`'s menu.

**Why:** confirmed by reading `AsyncPatternsTodo.cs`/`AsyncPatternsTodoSolution.cs` (phase 4) and `SwitchExpressionsTodo.cs`/`SwitchExpressionsTodoSolution.cs` (phase 5) side by side — logic bodies are identical.

**How to apply:** when reviewing a new phase's Todo/Solution pair, diff the two files' logic (ignoring class name and comments) — they should match exactly. Don't flag a Todo file as "already solved, that defeats the exercise" — that's the intended pattern here, not a bug.

**Known pre-existing exception:** `NullableReferenceTypesSolution.Run` (phase 4, `07-exceptions-nullable`) IS registered in `Program.cs`, contradicting the stated convention. This predates phase 5 and was not introduced by the phase-5 diff — low-priority inconsistency, worth a one-line note if reviewing `Program.cs` again but not a blocker.

See [[feedback_report_output_format]].
