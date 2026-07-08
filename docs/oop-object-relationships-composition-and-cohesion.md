# OOP Object Relationships, Composition, and Cohesion

This project uses a common OOP pattern: one object receives another object that it needs in order to do its job.

In `TaskService`, the service receives a repository:

```csharp
public class TaskService(IRepository<TaskItem> repository)
```

That means `TaskService` does not personally know how task data is stored. It only knows that it can ask the repository to add, update, or retrieve `TaskItem` values.

For example:

```csharp
public async Task CompleteTaskAsync(Guid id)
{
    var task = await repository.GetByIdAsync(id)
        ?? throw new LedgerValidationException($"Task with Id '{id}' was not found.");

    await repository.UpdateAsync(task with { Status = TaskStatus.Done });
}
```

The service owns the task workflow:

1. Load the task.
2. Validate that it exists.
3. Change the task status.
4. Ask the repository to save the updated task.

The repository owns persistence. The service owns business orchestration.

## Composition

When a class receives another object and uses it, that relationship is called a dependency. Building a larger behavior by combining smaller objects is called composition.

In this project:

```text
Program
  creates JsonFileRepository<TaskItem>
  passes it into TaskService

TaskService
  uses IRepository<TaskItem>
  applies task business rules

JsonFileRepository<T>
  handles storage
```

This keeps responsibilities separated. `TaskService` does not need to care whether tasks are stored in JSON, a database, memory, or an API. As long as the dependency follows `IRepository<TaskItem>`, the service can use it.

## More Dependencies Are Not Automatically Bad

A common rule of thumb says that a class should not have many dependencies. That can be useful as a warning sign, but it is not a fixed rule.

The real question is cohesion.

```text
Many cohesive dependencies can be fine.
Few unrelated dependencies can still be messy.
```

For example, a checkout workflow in a real application may naturally need several collaborators:

```csharp
public class CheckoutService(
    ICartRepository cartRepository,
    IPaymentGateway paymentGateway,
    IInventoryService inventoryService,
    IOrderRepository orderRepository,
    IEmailSender emailSender,
    IAuditLogger auditLogger)
```

That does not automatically mean the class is badly designed. Checkout is an orchestration-heavy workflow. It may need to:

1. Load the cart.
2. Charge the customer.
3. Reserve inventory.
4. Create an order.
5. Send confirmation email.
6. Write audit records.

Those dependencies all support one coherent responsibility: completing checkout.

The design becomes suspicious when unrelated workflows start living in the same class:

```text
CheckoutService
  handles checkout
  edits user profiles
  generates finance reports
  syncs marketing contacts
  manages admin permissions
```

That class would have too many reasons to change. It is no longer large because checkout is complex. It is large because unrelated responsibilities were put together.

## Single Responsibility Does Not Mean One Method

The single responsibility principle is often misunderstood.

It does not mean:

```text
one class = one method
one class = very few dependencies
one class = minimal code
```

It means:

```text
one class = one coherent area of ownership
```

`TaskService` can have several methods and still be cohesive:

```text
AddTaskAsync
CompleteTaskAsync
GetAllAsync
GetOverdueAsync
```

These methods all belong to task-related business workflows.

It would become less cohesive if it started gaining methods like:

```text
SendMarketingEmail
CalculateMonthlyTax
ExportExpenseReport
RegisterUser
```

Those methods may be valid features, but they do not belong to task management.

## A Practical Test for Cohesion

When deciding whether a class is too big, do not start by counting methods or constructor parameters. Ask these questions:

1. Can the class purpose be explained in one clear sentence?
2. Do the dependencies support that purpose?
3. Do the methods belong to the same domain workflow?
4. Would a change to an unrelated feature force this class to change?
5. Is the class hard to test because it mixes different concerns?

Good:

```text
TaskService owns task-related business workflows.
ExpenseService owns expense-related business workflows.
ConsoleNotifier owns console alert subscriptions and output.
```

Suspicious:

```text
TaskService owns task workflows, expense reports, email delivery, user registration, and UI formatting.
```

## React Container Analogy

This is similar to a React container component.

A drag-and-drop canvas container may be large because the canvas workflow is naturally complex. It may coordinate:

```text
selection state
drag and drop behavior
node configuration
data fetching
keyboard shortcuts
undo and redo
viewport state
validation
persistence
```

That size is not automatically a problem if all of that logic belongs to the canvas experience.

But the container does not need to physically contain every detail in one file. The container can call focused hooks:

```text
useCanvasSelection()
useCanvasDragDrop()
useCanvasPersistence()
useNodeConfiguration()
useCanvasKeyboardShortcuts()
```

Then it can pass simple values and callbacks to dumb child components.

The same idea applies in OOP. A service can orchestrate the workflow while smaller dependencies own focused details.

## Object Relationships Are Not All the Same

Not every object reference means the same thing. Before deciding whether one class should reference another, first name the kind of relationship.

Common cases:

```text
Part-of relationship:
ParkingFloor has ParkingSpots.
Order has OrderItems.

Workflow collaboration:
TaskService uses IRepository<TaskItem>.
CheckoutWorkflow uses PaymentService and InventoryService.

Historical reference:
ParkingTicket remembers which spot was assigned.
Expense remembers which category it belongs to.

Loose reaction:
TaskService raises TaskOverdue.
ConsoleNotifier reacts to the event.
```

The design choice depends on which relationship you are modeling.

## Same-Domain Composition

Example from a parking lot system:

```text
ParkingLotSystem
  contains ParkingFloor (many)

ParkingFloor
  contains ParkingSpot (many)
```

A `ParkingSpot` has limited meaning outside a floor, zone, or lot. It is not just "spot #7" by itself. The meaningful identity is closer to "spot #7 on floor 2."

So it is natural for a parent object to hold its children:

```java
class ParkingFloor {
    Map<String, ParkingSpot> spots;

    Optional<ParkingSpot> findAvailableSpot(Vehicle vehicle) {
        // asks its own spots whether they are free
    }
}
```

This relationship is composition or aggregation: the floor is made of spots. Calling into its own parts is not borrowing another domain's responsibility. It is the class doing its own job.

The usual direction should be:

```text
ParkingLot -> ParkingFloor -> ParkingSpot
```

The parent knows about the child. The child does not automatically need to know about the parent.

This may be enough:

```csharp
public class ParkingFloor
{
    public int FloorNumber { get; }
    public Dictionary<string, ParkingSpot> Spots { get; }
}
```

This needs more justification:

```csharp
public class ParkingSpot
{
    public ParkingFloor Floor { get; }
}
```

Child-to-parent references are not always wrong, but they increase coupling. Prefer one clear ownership direction unless the reverse reference solves a real problem.

## Cross-Domain Coordination

Compare that with `TaskService` needing `ExpenseService`. An `Expense` has a life outside a `Task`. It does not stop existing if a task is deleted, and it was not created by a task. That is a peer-domain relationship, not composition.

The test:

```text
Does B have independent meaning, identity, and lifecycle outside of A?

No  -> B is a part of A (composition/aggregate).
       A calling B directly is fine, no matter how many parts there are.

Yes -> B is a separate concept.
       A depending directly on B is a smell.
       The coordination between them belongs in a third class (an orchestrator).
```

For separate domains, prefer a higher-level workflow:

```csharp
public class PersonalLedgerWorkflowService(
    TaskService taskService,
    ExpenseService expenseService)
{
    public async Task CompleteTaskAndLogRewardExpenseAsync(Guid taskId)
    {
        await taskService.CompleteTaskAsync(taskId);
        await expenseService.AddExpenseAsync(10m, ExpenseCategory.Other, DateTime.Today);
    }
}
```

`TaskService` keeps owning task rules. `ExpenseService` keeps owning expense rules. The cross-domain use case gets a separate home.

## Events and IDs

Direct references are not the only option.

Use events when one area should announce that something happened without controlling who reacts:

```text
TaskService raises TaskOverdue.
ConsoleNotifier subscribes and prints a message.
```

That is better than making `TaskService` directly depend on `ConsoleNotifier`.

Use IDs or values when an object only needs to remember a relationship:

```csharp
public class ParkingTicket
{
    public string SpotId { get; init; }
    public string VehicleLicenseNumber { get; init; }
}
```

For a learning project, storing a `ParkingSpot` object inside `ParkingTicket` can be fine. In a real persisted app, storing `SpotId` is often safer because a ticket is a historical record. The spot object may change later, but the ticket should still remember what was assigned.

## Dependency Direction

A practical direction rule:

```text
Parent/owner can know child/part.
Child/part should usually not control parent/owner.
Sibling domains should usually meet through an orchestrator or event.
```

Healthy direction:

```text
ParkingLot -> ParkingFloor -> ParkingSpot
```

Suspicious direction:

```text
ParkingSpot -> ParkingFloor -> ParkingLotSystem
```

Not forbidden, but it needs a strong reason.

Also keep domain objects away from infrastructure details:

```text
Good:
TaskService -> IRepository<TaskItem>

Suspicious:
TaskItem -> JsonFileRepository
ParkingSpot -> DatabaseConnection
Vehicle -> EmailSender
```

The repository is a technical collaborator. It supports the service's workflow without adding a second business domain into the service.

## Class Roles

Most dependency questions get easier once you name what role a class is playing.

| Role | Owns | OK to depend on | Not OK to depend on |
|---|---|---|---|
| Domain model (`ParkingFloor`, `ParkingSpot`, `TaskItem`) | state and rules for one business concept | its own values and composed parts | repositories, console, database, email |
| Domain service (`TaskService`, `ExpenseService`) | one business workflow area | repositories, strategies, its own domain models | unrelated domain services |
| Infrastructure (`JsonFileRepository`) | a technical capability such as file I/O, DB, network | technical APIs and simple contracts | business decisions |
| Orchestrator (`PersonalLedgerWorkflowService`) | sequencing a process across domains | multiple domain services | low-level storage details when avoidable |

Infrastructure dependencies, like a repository, are not a violation of "one domain, one responsibility." They do not represent another business domain. They are plumbing that the service needs to complete its own workflow.

When a domain class seems to need another domain's capability (for example, "check the budget before completing a task"), that is usually a sign a new orchestrator role is needed, not that the dependency should be forced into the existing domain class:

```csharp
public class TaskCompletionWorkflow(
    TaskService taskService,
    ExpenseService expenseService)
{
    public async Task CompleteTaskAsync(Guid id)
    {
        await taskService.CompleteTaskAsync(id);
        // Coordinate expense-related side effects here if this use case needs them.
    }
}
```

`TaskService` and `ExpenseService` stay narrow. The cross-domain rule gets a home whose name says exactly what it does.

## Decision Checklist

When unsure, ask these questions in order:

1. Is this a natural part-of relationship?
2. Does the child need to know the parent, or is parent-to-child enough?
3. Is this class storing a fact, enforcing a rule, or coordinating a workflow?
4. Are the two objects in the same domain, or are they sibling domains?
5. If they are sibling domains, should a higher-level orchestrator own the use case?
6. If one side only needs to announce something, would an event be cleaner?
7. If one side only needs to remember the link, would an ID/value be cleaner?

## Project Guideline

For this project, prefer this rule:

```text
Do not judge a class by dependency count alone.
Judge it by whether its dependencies and methods are cohesive.
Inside one domain, direct references can be fine when they model real ownership.
Across domains, prefer orchestration, events, or IDs over direct control.
```

It is acceptable for a service to grow when the workflow it owns grows. Refactor when the growth comes from unrelated responsibilities, unclear ownership, or too many reasons to change.
