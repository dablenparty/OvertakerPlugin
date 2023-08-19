using System.Diagnostics.Contracts;
using System.Numerics;
using AssettoServer.Server;

namespace OvertakerPlugin.State;

public readonly struct CarState : IEquatable<CarState>
{
    private EntryCar EntryCarRef { get; }

    /// <summary>
    ///     true if this car is controlled by AI, false if it is controlled by a player
    /// </summary>
    public bool AiControlled { get; }

    /// <summary>
    ///     Returns the name of the driver of this car. If the driver is not known, returns "Unknown";
    /// </summary>
    public string DriverName => (AiControlled ? EntryCarRef.AiName : EntryCarRef.Client?.Name) ?? "Unknown";

    /// <summary>
    ///     Position of this car, measured in meters
    /// </summary>
    public Vector3 Position { get; } = new();

    /// <summary>
    ///     Velocity measured in meters per second
    /// </summary>
    public Vector3 Velocity { get; } = new();

    /// <summary>
    ///     This car's session ID
    /// </summary>
    public byte SessionId { get; }


    /// <summary>
    ///     Calculates the speed of the car in kilometers per hour (km/h)
    /// </summary>
    public float SpeedKmh => OvertakerUtils.MsToKmh(Velocity);

    public bool Equals(CarState other) => EntryCarRef.Equals(other.EntryCarRef);

    public override bool Equals(object? obj) => obj is CarState other && Equals(other);

    public override int GetHashCode() => EntryCarRef.GetHashCode();

    public static bool operator ==(CarState left, CarState right) => left.Equals(right);

    public static bool operator !=(CarState left, CarState right) => !left.Equals(right);

    public CarState(EntryCar entryCarRef)
    {
        EntryCarRef = entryCarRef;
        AiControlled = entryCarRef.AiControlled;
        // clone the position and velocity so that they don't change out from under us
        Position = OvertakerUtils.CopyVector3(entryCarRef.Status.Position);
        Velocity = OvertakerUtils.CopyVector3(entryCarRef.Status.Velocity);
        SessionId = entryCarRef.SessionId;
    }

    /// <summary>
    ///     Gets the relative position of this car to another car.
    /// </summary>
    /// <param name="other">Other car</param>
    /// <returns>Relative position</returns>
    [Pure]
    public RelativePosition GetRelativePositionTo(CarState other)
    {
        // TODO: double check this math, it might not be right
        var otherNormalizedVelocity = other.Velocity;
        var normalizedDistance = Position - other.Position;
        var angleDiff =
            OvertakerUtils.Vector3Angle(otherNormalizedVelocity, normalizedDistance);
        if (angleDiff < 0)
            angleDiff += 360;

        return angleDiff switch
        {
            >= 0 and < 45 or >= 315 and <= 360 => RelativePosition.Front,
            >= 45 and < 135 or >= 225 and < 315 => RelativePosition.Sides,
            _ => RelativePosition.Behind
        };
    }
}
