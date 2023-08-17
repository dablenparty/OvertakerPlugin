using System.Numerics;
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
        var scoreUpdates = new Dictionary<byte, uint>();

        var thisTick = stateHistory[0];
        var prevTick = stateHistory[1];

        foreach (var (_, currentCarState) in thisTick.CarStates)
        {
            if (currentCarState.AiControlled || currentCarState.SpeedKmh < Configuration.MinimumSpeedKmh)
                continue;
            var nearbyCars = thisTick.CarStates.Where(p =>
            {
                var (sessionId, carState) = p;
                var distance = Vector3.Distance(carState.Position, currentCarState.Position);
                return sessionId != currentCarState.SessionId && 0 < distance &&
                       distance <= Configuration.MinimumDistanceMeters;
            }).ToList();

            if (nearbyCars.Count == 0)
                continue;

            var overtakes = 0;
            var distancesToMe = new List<float>();
            var speedDifferences = new List<float>();
            var prevCarState = prevTick[currentCarState.SessionId];
            foreach (var (_, nearCar) in nearbyCars)
            {
                var prevNearCarState = prevTick[nearCar.SessionId];
                var relPosBefore = prevNearCarState.GetRelativePositionTo(prevCarState);
                var relPosAfter = nearCar.GetRelativePositionTo(currentCarState);
                // was beside, now behind
                if (relPosBefore != RelativePosition.Sides || relPosAfter != RelativePosition.Behind)
                    continue;
                var distanceToCurrent = Vector3.Distance(nearCar.Position, currentCarState.Position);
                distancesToMe.Add(distanceToCurrent);
                var speedDifference = nearCar.SpeedKmh - currentCarState.SpeedKmh;
                speedDifferences.Add(speedDifference);
                overtakes++;
            }

            if (overtakes == 0)
                continue;

            var distancesAvg = distancesToMe.Average();
            var speedDiffsAvg = speedDifferences.Average();
            var score = (uint) (Value * overtakes + distancesAvg + speedDiffsAvg +
                                currentCarState.SpeedKmh);
            if (scoreUpdates.TryGetValue(currentCarState.SessionId, out var existingScore))
                scoreUpdates[currentCarState.SessionId] = existingScore + score;
            else
                scoreUpdates.Add(currentCarState.SessionId, score);
        }

        return scoreUpdates;
    }
}
