using System;
using System.Collections.Generic;

public class StackList<T>
{
    private readonly int _maxStackSize;
    private readonly List<Stack<T>> _stacks;

    public StackList(int maxStackSize)
    {
        if (maxStackSize <= 0)
        {
            throw new ArgumentException("Max stack size must be greater than zero.");
        }

        _maxStackSize = maxStackSize;
        _stacks = new List<Stack<T>>();
    }

    public void Push(T value)
    {
        if (_stacks.Count == 0 || _stacks[^1].Count == _maxStackSize)
        {
            _stacks.Add(new Stack<T>());
        }

        _stacks[^1].Push(value);
    }

    public T Pop()
    {
        if (_stacks.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        T value = _stacks[^1].Pop();

        if (_stacks[^1].Count == 0)
        {
            _stacks.RemoveAt(_stacks.Count - 1);
        }

        return value;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var stackList = new StackList<int>(3);

        stackList.Push(1);
        stackList.Push(2);
        stackList.Push(3);
        stackList.Push(4); // here comes new stack

        
        Console.WriteLine(stackList.Pop()); 
        Console.WriteLine(stackList.Pop()); 
        Console.WriteLine(stackList.Pop()); 
        Console.WriteLine(stackList.Pop()); 

        try
        {
            Console.WriteLine(stackList.Pop()); 
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
