using System.Numerics;
using OvertakerPlugin.State;
using Serilog;

namespace OvertakerPlugin.Actions;

public class OvertakeAction : Action
{
    public sealed override string Name { get; init; } = "Overtake";
    public sealed override int Value { get; init; } = 100;

    public override Dictionary<string, uint> ScoreAction(
        [StateHistory.NeedsHistory(StateCount = 2)]
        List<TickState> stateHistory)
    {
        // TODO: the rest of this
        return new Dictionary<string, uint>();
    }

    public OvertakeAction(OvertakerConfiguration configuration) : base(configuration)
    {
    }
}
