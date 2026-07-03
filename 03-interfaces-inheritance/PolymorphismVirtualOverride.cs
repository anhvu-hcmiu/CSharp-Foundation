namespace CSharpExercises.InterfacesInheritance;

// Why this exists: JS methods are always "overridable" by default - any subclass method
// with the same name replaces the parent's, no keyword needed, and `super.method()` always
// works. C# requires you to opt in explicitly: a base method must be marked `virtual` to
// allow overriding, and the subclass must use `override` (not just redeclare the method).
// This is deliberate - a plain (non-virtual) method call is resolved at compile time and
// can be faster / harder to accidentally break; `virtual` opts into runtime (polymorphic)
// dispatch, where the ACTUAL object's type decides which method body runs, even when
// accessed through a base-type reference/collection.
public class Notifier
{
    public virtual void Send(string message)
    {
        Console.WriteLine($"[Notifier] {message}");
    }
}

public class EmailNotifier : Notifier
{
    public override void Send(string message)
    {
        base.Send(message); // calls Notifier.Send explicitly, then adds its own behavior
        Console.WriteLine($"[Email] delivering: {message}");
    }
}

public class SmsNotifier : Notifier
{
    public override void Send(string message)
    {
        Console.WriteLine($"[SMS] {message}");
    }
}

public static class PolymorphismVirtualOverride
{
    public static void Run()
    {
        // The list is typed as the BASE class, but each element's ACTUAL (subclass) type
        // decides which Send() runs - that's runtime dispatch. Removing `virtual`/`override`
        // would make every call go to Notifier.Send() regardless of the real object type.
        List<Notifier> notifiers = new()
        {
            new Notifier(),
            new EmailNotifier(),
            new SmsNotifier(),
        };

        foreach (var notifier in notifiers)
        {
            notifier.Send("Server restarted");
        }
    }
}
