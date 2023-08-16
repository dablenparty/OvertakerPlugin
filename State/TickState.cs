using AssettoServer.Server;

namespace OvertakerPlugin.State;

public readonly struct TickState
{
    public CarState[] CarStates { get; }
    public DateTime TimeOfTick { get; init; }
    
    public CarState this[byte sessionId] => CarStates[sessionId];

    public TickState(IReadOnlyCollection<EntryCar> entryCars, DateTime timeOfTick)
    {
        CarStates = new CarState[entryCars.Count];
        foreach (var entryCar in entryCars)
        {
            var carState = new CarState(entryCar);
            // AI cars don't keep their EntryCar.Status property updated, so we have to do it ourselves
            if (entryCar.AiControlled)
            {
                entryCar.GetPositionUpdateForCar(entryCar, out var positionUpdate);
                carState.Position = positionUpdate.Position;
                carState.Velocity = positionUpdate.Velocity;
            }

            CarStates[entryCar.SessionId] = carState;
        }

        TimeOfTick = timeOfTick;
    }
}
