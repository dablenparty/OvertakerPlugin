using System.Diagnostics.Contracts;
using System.Numerics;
using AssettoServer.Server;

namespace OvertakerPlugin.State;

public readonly struct CarState : IEquatable<CarState>
{
    public EntryCar EntryCar { get; }

    /// <summary>
    ///     Returns the name of the driver of this car. If the driver is not known, returns "Unknown";
    /// </summary>
    public string DriverName => EntryCar.Client?.Name ?? "Unknown";

    /// <summary>
    ///     Alias for EntryCar.Status.Position, measured in meters
    /// </summary>
    public Vector3 Position
    {
        get => EntryCar.Status.Position;
        set => EntryCar.Status.Position = value;
    }

    /// <summary>
    ///     Alias for EntryCar.Status.Velocity, measured in meters per second
    /// </summary>
    public Vector3 Velocity
    {
        get => EntryCar.Status.Velocity;
        set => EntryCar.Status.Velocity = value;
    }

    /// <summary>
    ///     Calculates the speed of the car in kilometers per hour (km/h)
    /// </summary>
    public float SpeedKmh => OvertakerUtils.MsToKmh(EntryCar.Status.Velocity);

    public bool Equals(CarState other) => EntryCar.Equals(other.EntryCar);

    public override bool Equals(object? obj) => obj is CarState other && Equals(other);

    public override int GetHashCode() => EntryCar.GetHashCode();

    public static bool operator ==(CarState left, CarState right) => left.Equals(right);

    public static bool operator !=(CarState left, CarState right) => !left.Equals(right);

    public CarState(EntryCar entryCar) => EntryCar = entryCar;

    /// <summary>
    ///     Gets the relative position of this car to another car.
    /// </summary>
    /// <param name="other">Other car</param>
    /// <returns>Relative position</returns>
    [Pure]
    public RelativePosition GetRelativePositionTo(CarState other)
    {
        // TODO: double check this math, it might not be right
        var otherNormalizedVelocity = Vector3.Normalize(other.Velocity);
        var normalizedDistance = Vector3.Normalize(Position - other.Position);
        var angleDiff =
            OvertakerUtils.Vector3Angle(otherNormalizedVelocity, normalizedDistance);
        if (angleDiff < 0)
            angleDiff += 360;

        return angleDiff switch
        {
            >= 0 and < 45 or >= 315 and <= 360 => RelativePosition.Front,
            >= 45 and < 135 or >= 225 and < 315 => RelativePosition.Behind,
            _ => RelativePosition.Sides
        };
    }
}
