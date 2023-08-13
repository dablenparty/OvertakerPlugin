using AssettoServer.Server;
using Serilog;

namespace OvertakerPlugin.Actions;

public class StateHistory
{
    private readonly ILogger _logger = Log.ForContext<StateHistory>();
    internal FixedSizedQueue<TickState> TickStates { get; private init; } = new(100);
    
    internal TickState this[int idx] => TickStates.ElementAt(idx);

    internal TickState NewTickHappened(EntryCarManager entryCarManager)
    {
        var tickState = new TickState(entryCarManager.EntryCars, DateTime.Now);
        TickStates.Enqueue(tickState);
        return tickState;
    }

    public class NeedsHistoryAttribute : Attribute
    {
        public int StateCount { get; init; } = 1;
    }
}
