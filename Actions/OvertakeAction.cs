using System.Numerics;
using OvertakerPlugin.State;

namespace OvertakerPlugin.Actions;

public class OvertakeAction : Action
{
    public sealed override string Name { get; init; } = "Overtake";
    public sealed override int Value { get; init; } = 100;

    public override int ScoreAction([StateHistory.NeedsHistory(StateCount = 2)] List<TickState> stateHistory)
    {
        var thisTick = stateHistory[0];
        var lastTick = stateHistory[1];
        foreach (var (_, entryCar) in thisTick.EntryCars)
        {
            var speedKmh = OvertakerUtils.KmhFromAcVelocity(entryCar.Status.Velocity);
            if (entryCar.AiControlled || speedKmh < Configuration.MinimumSpeedKmh)
                return 0;
            var nearbyCars = thisTick.EntryCars
                .Select(p => p.Value)
                .Where(c => c != entryCar &&
                            Vector3.Distance(c.Status.Position, entryCar.Status.Position) <=
                            Configuration.MinimumDistanceMeters)
                .ToList();
            if (nearbyCars.Count == 0)
                return 0;
        }

        // TODO: the rest of this
        return -1;
    }

    public OvertakeAction(OvertakerConfiguration configuration) : base(configuration)
    {
    }
}
