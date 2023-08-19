using AssettoServer.Server;
using OvertakerPlugin.Utility;

namespace OvertakerPlugin.State;

public class StateHistory : LazySingleton<StateHistory>
{
    private StateHistory()
    {
    }

    internal FixedSizedStack<TickState> TickStates { get; } = new(100);

    /// <summary>
    ///     Indicates that a new tick has happened, and that a new state should be added to the history.
    /// </summary>
    /// <param name="entryCarManager">The EntryCarManager used to get vehicle states</param>
    public static void NewTickHappened(EntryCarManager entryCarManager)
    {
        var tickState = new TickState(entryCarManager.EntryCars, DateTime.Now);
        Instance.TickStates.Push(tickState);
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
