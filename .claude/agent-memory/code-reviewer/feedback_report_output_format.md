---
name: feedback-report-output-format
description: When spawned as code-reviewer subagent in this environment, return findings as plain text, not a written report file
metadata:
  type: feedback
---

Return code review findings directly as the final assistant message text, not as a written `.md` report file — even when the subagent system-reminder context suggests a report file naming pattern (`plans/reports/code-reviewer-...-report.md`).

**Why:** the tool-level harness instructions explicitly state "Do NOT Write report/summary/findings/analysis .md files. Return findings directly as your final assistant message" — this is the more specific, emphatic instruction and takes precedence over the generic subagent-start naming template, which appears to be boilerplate shown to all subagent types regardless of whether file output is wanted.

**How to apply:** when the launching agent's task prompt says "report findings using ReportFindings tool" but no such tool is present in the available function list, treat that as an instruction to communicate findings in prose in the final response, not to write a file.
