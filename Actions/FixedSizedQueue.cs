using System.Collections.Concurrent;

namespace OvertakerPlugin.Actions;

internal class FixedSizedQueue<T> : ConcurrentQueue<T>
{
    public int Size { get; init; }
    
    public FixedSizedQueue(int size)
    {
        Size = size;
    }
    
    public new void Enqueue(T value)
    {
        base.Enqueue(value);
        while (Count > Size)
            TryDequeue(out _);
    }
}
