using System.Collections.Concurrent;

namespace OvertakerPlugin.State;

internal class FixedSizedQueue<T> : ConcurrentQueue<T>
{
    public FixedSizedQueue(int size)
    {
        Size = size;
    }

    private int Size { get; }

    public new void Enqueue(T value)
    {
        base.Enqueue(value);
        while (Count > Size)
            TryDequeue(out _);
    }
}
