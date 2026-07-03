using CSharpExercises.TypesAndSyntax;
using CSharpExercises.OopCore;
using CSharpExercises.InterfacesInheritance;
using CSharpExercises.Generics;
using CSharpExercises.CollectionsLinq;

var exercises = new (string Label, Action Run)[]
{
    ("01 - Types and Syntax: Hello Types", HelloTypes.Run),
    ("01 - Types and Syntax: Value vs Reference Types", ValueVsReferenceTypes.Run),
    ("01 - Types and Syntax: Static Typing", StaticTyping.Run),
    ("02 - OOP Core: Classes and Constructors", ClassesAndConstructors.Run),
    ("02 - OOP Core: Properties and Fields", PropertiesAndFields.Run),
    ("02 - OOP Core: Access Modifiers", AccessModifiers.Run),
    ("03 - Interfaces/Inheritance: Interfaces Basics", InterfacesBasics.Run),
    ("03 - Interfaces/Inheritance: Abstract Classes", AbstractClasses.Run),
    ("03 - Interfaces/Inheritance: Polymorphism (virtual/override)", PolymorphismVirtualOverride.Run),
    ("04 - Generics: Generic Methods", GenericMethods.Run),
    ("04 - Generics: Generic Classes", GenericClasses.Run),
    ("04 - Generics: Built-in Generic Collections", GenericCollectionsBuiltIn.Run),
    ("05 - Collections/LINQ: Method Syntax Basics", LinqMethodSyntaxBasics.Run),
    ("05 - Collections/LINQ: Deferred Execution", LinqDeferredExecution.Run),
    ("05 - Collections/LINQ: Query Syntax Comparison", LinqQuerySyntaxComparison.Run),
    ("05 - Collections/LINQ: LinqAggregatesTodo", LinqAggregatesTodo.Run),
};

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== C# Exercises ===");
    for (var i = 0; i < exercises.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {exercises[i].Label}");
    }
    Console.WriteLine("0/q. Quit");
    Console.Write("Select: ");

    var input = Console.ReadLine()?.Trim();

    if (input is null || input.Equals("q", StringComparison.OrdinalIgnoreCase) || input == "0")
    {
        break;
    }

    if (int.TryParse(input, out var choice) && choice >= 1 && choice <= exercises.Length)
    {
        Console.WriteLine();
        exercises[choice - 1].Run();
    }
    else
    {
        Console.WriteLine("Invalid selection.");
    }
}
