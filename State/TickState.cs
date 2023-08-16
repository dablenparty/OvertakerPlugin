using AssettoServer.Server;

namespace OvertakerPlugin.State;

public readonly struct TickState
{
    public Dictionary<string, CarState> CarStates { get; init; }
    public DateTime TimeOfTick { get; init; }

    public TickState(IEnumerable<EntryCar> entryCars, DateTime timeOfTick)
    {
        var carStates = new Dictionary<string, CarState>();
        foreach (var entryCar in entryCars)
        {
            var carState = new CarState(entryCar);
            if (!entryCar.AiControlled)
            {
                carStates.Add(entryCar.Client?.HashedGuid ?? entryCar.Model, carState);
                continue;
            }

            // AI cars don't keep their EntryCar.Status property updated, so we have to do it ourselves
            entryCar.GetPositionUpdateForCar(entryCar, out var positionUpdate);
            carState.Position = positionUpdate.Position;
            carState.Velocity = positionUpdate.Velocity;
            carStates.Add(entryCar.AiName ?? entryCar.Model, carState);
        }

        CarStates = carStates;
        TimeOfTick = timeOfTick;
    }

    /// <summary>
    ///     Funky method to get a car from a car. This is used by the event loop to get car data from multiple TickState's
    /// </summary>
    /// <param name="key">The car to get the key from</param>
    /// <param name="car">The CarState, if found</param>
    /// <returns>true if the CarState was found, false otherwise</returns>
    public bool TryGetCarByCar(EntryCar key, out CarState car)
    {
        return CarStates.TryGetValue(key.Client?.HashedGuid ?? key.Model, out car);
    }
}
