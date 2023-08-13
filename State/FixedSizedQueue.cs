using System.Collections.Concurrent;

namespace OvertakerPlugin.State;

internal class FixedSizedQueue<T> : ConcurrentQueue<T>
{
    private int Size { get; }
    
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
