---
phase: 2
title: "OOP Core and Interfaces"
status: completed
priority: P1
effort: "4h"
dependencies: [1]
---

# Phase 2: OOP Core and Interfaces

## Overview
Highest-friction topics from the original ASP.NET Core code review. Covers types/syntax mapping, OOP fundamentals, and interfaces/inheritance - the things JS classes don't really have. Fully worked examples (read+run), no TODOs - syntax is still unfamiliar at this point.

## Requirements
- Functional: each exercise runs standalone from the menu, prints output that demonstrates the concept (e.g. constructor order, access modifier violations would be compile errors shown via comments, polymorphic dispatch).
- Non-functional: every file has a short comment block explaining *why* the syntax exists, not just what it does (e.g. why `readonly` instead of `private set`, why interfaces over abstract classes here).

## Architecture
Folders `01-types-and-syntax/`, `02-oop-core/`, `03-interfaces-inheritance/`. Each exercise = one `.cs` file, one `static class XxxExercise { public static void Run() { ... } }`. Each registered in `Program.cs` menu (extend Phase 1's dispatcher).

## Related Code Files
- Create: `01-types-and-syntax/ValueVsReferenceTypes.cs` - struct vs class, stack vs heap, `var` type inference
- Create: `01-types-and-syntax/StaticTyping.cs` - compile-time type checking vs JS dynamic typing, boxing/unboxing
- Create: `02-oop-core/ClassesAndConstructors.cs` - constructors, constructor overloading, `this()`  chaining
- Create: `02-oop-core/PropertiesAndFields.cs` - auto-properties, `get`/`set`, `init`, backing fields, `readonly`
- Create: `02-oop-core/AccessModifiers.cs` - `public`/`private`/`protected`/`internal` with a 2-class example showing what compiles
- Create: `03-interfaces-inheritance/InterfacesBasics.cs` - interface declaration, implementation, multiple interface implementation
- Create: `03-interfaces-inheritance/AbstractClasses.cs` - abstract class vs interface, when to use which
- Create: `03-interfaces-inheritance/PolymorphismVirtualOverride.cs` - `virtual`/`override`/`base`, runtime dispatch demo
- Modify: `Program.cs` - register 8 new menu entries

## Implementation Steps
1. Write `01-types-and-syntax/ValueVsReferenceTypes.cs`: define a `struct Point` and `class PointRef`, mutate a copy vs reference, print to show the divergence.
2. Write `01-types-and-syntax/StaticTyping.cs`: show `var` inferring a type, then a deliberate type mismatch as a commented-out line with explanation (can't actually compile broken code).
3. Write `02-oop-core/ClassesAndConstructors.cs`: a `BankAccount` class with 2 constructors (one chaining to the other via `: this(...)`).
4. Write `02-oop-core/PropertiesAndFields.cs`: same `BankAccount`-style class extended with auto-properties, computed property (e.g. `FullName => $"{First} {Last}"`), `init`-only property.
5. Write `02-oop-core/AccessModifiers.cs`: two classes in one file, one calling the other's public/internal members, comments marking what would fail to compile if uncommented.
6. Write `03-interfaces-inheritance/InterfacesBasics.cs`: `IPayable` interface, 2 implementing classes, call through the interface type.
7. Write `03-interfaces-inheritance/AbstractClasses.cs`: `abstract class Shape` with abstract `Area()`, 2 concrete subclasses, contrast with interface from step 6 in comments.
8. Write `03-interfaces-inheritance/PolymorphismVirtualOverride.cs`: base class with `virtual` method, override in subclass, demonstrate dispatch via a `List<BaseType>` loop.
9. Register all 8 in `Program.cs` menu, build, run each once to confirm output.

## Success Criteria
- [x] All 8 exercises build with zero warnings and run from the menu
- [ ] User can read each file and verbally explain the concept back before moving to phase 3 (self-check, not automated - pending user)
- [x] No TODOs in this phase - fully worked examples only

## Risk Assessment
- Scope creep risk: easy to over-add OOP edge cases (sealed classes, operator overloading, etc.) - stick to the list above, defer anything not in the brainstorm's "core OOP" scope.
- If exercises feel too easy/too hard once written, recalibrate phase 3 difficulty accordingly rather than expanding phase 2.
