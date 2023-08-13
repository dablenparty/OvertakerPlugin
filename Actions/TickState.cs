using AssettoServer.Server;

namespace OvertakerPlugin.Actions;

internal readonly struct TickState
{
    public Dictionary<string, EntryCar> EntryCars { get; init; }
    public DateTime TimeOfTick { get; init; }

    public TickState(IEnumerable<EntryCar> entryCars, DateTime timeOfTick)
    {
        EntryCars = entryCars.ToDictionary(c => c.Client?.HashedGuid ?? c.Model);
        TimeOfTick = timeOfTick;
    }

    /// <summary>
    /// Funky method to get a car from a car. This is used by the event loop to get car data from multiple TickState's
    /// </summary>
    /// <param name="car"></param>
    /// <returns></returns>
    public EntryCar? GetCarByCar(EntryCar car)
    {
        return EntryCars.TryGetValue(car.Client?.HashedGuid ?? car.Model, out var entryCar) ? entryCar : null;
    }
}
