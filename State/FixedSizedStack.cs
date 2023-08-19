using System.Collections.Concurrent;

namespace OvertakerPlugin.State;

internal class FixedSizedStack<T> : ConcurrentStack<T>
{
    public FixedSizedStack(int size)
    {
        Size = size;
    }

    private int Size { get; }

    public new void Push(T value)
    {
        base.Push(value);
        while (Count > Size)         
            TryPop(out _);
    }
}
