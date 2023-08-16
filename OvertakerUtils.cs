using System.Numerics;

namespace OvertakerPlugin;

public static class OvertakerUtils
{
    /// <summary>
    ///     Given a vector representing the velocity of a car, return the speed in km/h.
    /// </summary>
    /// <param name="velocity">Car velocity as a 3D vector</param>
    /// <returns>Scalar speed in KM/H</returns>
    public static float KmhFromAcVelocity(Vector3 velocity)
    {
        // I have no idea where this magic number comes from, but I found it in the source code linked below and it
        // seems to work well enough.
        // https://github.com/Jonfinity/LightspeedPlugin/blob/main/LightspeedPlugin.cs#L104
        const float MagicNumber = 3.6f;
        return velocity.Length() * MagicNumber;
    }

    /// <summary>
    ///     Returns the angle between two vectors in degrees.
    /// </summary>
    /// <param name="first">First vector</param>
    /// <param name="second">Second vector</param>
    /// <returns>Angle, in degrees</returns>
    public static float Vector3Angle(Vector3 first, Vector3 second)
    {
        var denominator = first.Length() * second.Length();
        // if the vectors are the same or one of them zeros out, the angle is 0
        var radians = first == second || denominator == 0
            ? 0.0f
            : Math.Acos(Vector3.Dot(first, second) / denominator);
        var degrees = radians * 180.0 / Math.PI;
        return (float) (degrees < 0 ? degrees + 360 : degrees);
    }
}
