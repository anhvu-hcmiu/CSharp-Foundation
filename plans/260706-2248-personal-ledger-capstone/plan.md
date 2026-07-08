---
title: "Personal Ledger Capstone Project"
description: "Consolidation capstone: combined Task+Expense console app forcing real generics/LINQ/events/async/records reuse across two entities"
status: in-progress
priority: P2
branch: "main"
tags: [csharp, capstone]
blockedBy: []
blocks: []
created: "2026-07-06T15:58:40.943Z"
createdBy: "ck:plan"
source: skill
---

# Personal Ledger Capstone Project

## Overview

Capstone project consolidating the completed C# Fundamentals Curriculum (`plans/260630-2224-csharp-fundamentals-curriculum/`, status: completed). Builds a "Personal Ledger" console app: combined Task + Expense manager sharing one generic `IRepository<T>` + JSON persistence, LINQ reports, C# events for alerts, real async file I/O, records with pattern matching. Pure console, no ASP.NET Core/EF Core (deferred per original curriculum scope). New multi-project solution (`CSharpFoundation.sln`) with `PersonalLedger.csproj` under `10-capstone-personal-ledger/`; existing `CSharpExercises.csproj` (exercises 01-09) stays untouched.

See brainstorm report: `plans/reports/brainstorm-260706-2248-personal-ledger-capstone-report.md` for approaches considered, topic coverage map, and full rationale.

## Phases

| Phase | Name | Status |
|-------|------|--------|
| 1 | [Solution Scaffolding and Models](./phase-01-solution-scaffolding-and-models.md) | Done |
| 2 | [Services and Events](./phase-02-services-and-events.md) | Done |
| 3 | [LINQ Reports](./phase-03-linq-reports.md) | Pending |
| 4 | [Console UI and Persistence Polish](./phase-04-console-ui-and-persistence-polish.md) | Pending |

## Dependencies

None blocking. Related: `plans/260630-2224-csharp-fundamentals-curriculum/` (completed - this plan consolidates its topics, does not modify its files).
