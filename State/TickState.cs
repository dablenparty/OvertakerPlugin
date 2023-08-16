using AssettoServer.Server;

namespace OvertakerPlugin.State;

public readonly struct TickState
{
    public Dictionary<string, CarState> CarStates { get; init; }
    public DateTime TimeOfTick { get; init; }

    public TickState(IEnumerable<EntryCar> entryCars, DateTime timeOfTick)
    {
        // TODO: differentiate between players and AI since AI cars don't update their Status property for some reason
        CarStates = entryCars.ToDictionary(c => c.Client?.HashedGuid ?? c.Model, c => new CarState(c));
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
