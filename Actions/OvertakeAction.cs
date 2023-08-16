using OvertakerPlugin.State;

namespace OvertakerPlugin.Actions;

public class OvertakeAction : AbstractOvertakerAction
{
    public OvertakeAction(OvertakerConfiguration configuration) : base(configuration)
    {
    }

    public sealed override string Name { get; init; } = "Overtake";
    public sealed override int Value { get; init; } = 100;

    public override Dictionary<byte, uint> ScoreAction(
        [StateHistory.NeedsHistory(StateCount = 2)]
        List<TickState> stateHistory)
    {
        // TODO: the rest of this
        return new Dictionary<byte, uint>();
    }
}
