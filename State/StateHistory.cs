using AssettoServer.Server;

namespace OvertakerPlugin.State;

public class StateHistory
{
    public static StateHistory CurrentHistory { get; } = new();

    internal FixedSizedQueue<TickState> TickStates { get; } = new(100);

    internal TickState this[int idx] => TickStates.ElementAt(idx);

    /// <summary>
    ///     Indicates that a new tick has happened, and that a new state should be added to the history.
    /// </summary>
    /// <param name="entryCarManager">The EntryCarManager used to get vehicle states</param>
    public static void NewTickHappened(EntryCarManager entryCarManager)
    {
        var tickState = new TickState(entryCarManager.EntryCars, DateTime.Now);
        CurrentHistory.TickStates.Enqueue(tickState);
    }

    private StateHistory()
    {
    }

    /// <summary>
    ///     Fills a parameter with TickState's from the current state history. How this data is passed is handled
    ///     elsewhere.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class NeedsHistoryAttribute : Attribute
    {
        public int StateCount { get; init; } = 1;
    }
}
