using AssettoServer.Server;

namespace OvertakerPlugin.State;

public readonly struct TickState
{
    public Dictionary<byte, CarState> CarStates { get; } = new();
    public DateTime TimeOfTick { get; }

    public CarState this[byte sessionId] => CarStates[sessionId];

    public TickState(IEnumerable<EntryCar> entryCars, DateTime timeOfTick)
    {
        foreach (var entryCar in entryCars)
        {
            // AI cars don't keep their EntryCar.Status property updated, so we have to do it ourselves
            if (entryCar.AiControlled)
            {
                entryCar.GetPositionUpdateForCar(entryCar, out var positionUpdate);
                entryCar.Status.Position = positionUpdate.Position;
                entryCar.Status.Velocity = positionUpdate.Velocity;
            }

            var carState = new CarState(entryCar);
            CarStates[entryCar.SessionId] = carState;
        }

        TimeOfTick = timeOfTick;
    }
}
