namespace CSharpExercises.OopCore;

// Why this exists: JS has no real access modifiers - `#private` fields exist but there's no
// protected/internal equivalent, and most "privacy" is convention (underscore prefix) rather
// than enforcement. C# enforces four levels at compile time: `public` (anyone), `private`
// (this class only), `protected` (this class + subclasses), `internal` (this assembly/project
// only). Getting this wrong is a compile error, not a runtime surprise - which is exactly why
// it trips people reading C# for the first time: the compiler is stricter than you expect.
public class Vault
{
    public string Owner; // accessible from anywhere
    private decimal _secretBalance = 500m; // accessible only inside Vault itself
    protected string AuditNote = "unaudited"; // accessible in Vault and subclasses
    internal bool IsLocked = true; // accessible anywhere in this project/assembly, not outside it

    public Vault(string owner)
    {
        Owner = owner;
    }

    public decimal PeekBalance()
    {
        return _secretBalance; // fine: same class
    }
}

public class VaultInspector
{
    public void Inspect(Vault vault)
    {
        Console.WriteLine($"Owner: {vault.Owner}"); // fine: public
        Console.WriteLine($"Locked: {vault.IsLocked}"); // fine: internal, same project
        Console.WriteLine($"Balance via method: {vault.PeekBalance()}"); // fine: public method

        // var balance = vault._secretBalance; // would NOT compile - CS0122: _secretBalance is private
        // var note = vault.AuditNote;          // would NOT compile - CS0122: AuditNote is protected,
        //                                      // and VaultInspector does not inherit from Vault
    }
}

public static class AccessModifiers
{
    public static void Run()
    {
        var vault = new Vault("Carol");
        var inspector = new VaultInspector();
        inspector.Inspect(vault);
    }
}
