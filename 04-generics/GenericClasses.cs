namespace CSharpExercises.Generics;

// Why this exists: this is roughly what List<T>/Stack<T> do internally - a fixed-size array
// that grows by reallocating and copying when it fills up. A generic class works like a
// generic method, but the type parameter `T` is fixed for the whole class once you
// instantiate it - `new SimpleStack<int>()` and `new SimpleStack<string>()` are different
// closed types, not the same class reused loosely the way a JS array would be.
public class SimpleStack<T>
{
    private T[] _items = new T[4];

    public int Count { get; private set; }

    public void Push(T item)
    {
        if (Count == _items.Length)
        {
            // Growth strategy List<T> also uses under the hood: double capacity, copy over.
            Array.Resize(ref _items, _items.Length * 2);
        }

        _items[Count] = item;
        Count++;
    }

    public T Pop()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("Stack is empty");
        }

        Count--;
        return _items[Count];
    }

    public T Peek()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("Stack is empty");
        }

        return _items[Count - 1];
    }
}

public static class GenericClasses
{
    public static void Run()
    {
        var numbers = new SimpleStack<int>();
        numbers.Push(1);
        numbers.Push(2);
        numbers.Push(3);
        Console.WriteLine($"Peek: {numbers.Peek()}");
        Console.WriteLine($"Pop: {numbers.Pop()}");
        Console.WriteLine($"Count after pop: {numbers.Count}");

        var words = new SimpleStack<string>();
        words.Push("first");
        words.Push("second");
        Console.WriteLine($"Pop: {words.Pop()}");
        Console.WriteLine($"Pop: {words.Pop()}");
    }
}
