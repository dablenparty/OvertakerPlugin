using System.Numerics;

namespace OvertakerPlugin;

public static class OvertakerUtils
{
    /// <summary>
    ///     Given a vector representing velocity in meters per second (m/s), returns the speed in kilometers per hour
    ///     (km/h).
    /// </summary>
    /// <param name="velocity">Velocity in m/s</param>
    /// <returns>Scalar speed in KM/H</returns>
    public static float MsToKmh(Vector3 velocity)
    {
        const float ConversionFactor = 3.6f;
        return velocity.Length() * ConversionFactor;
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

    /// <summary>
    ///     Copies a Vector3
    /// </summary>
    /// <param name="vector3">Vector to copy</param>
    /// <returns>Copied vector</returns>
    public static Vector3 CopyVector3(Vector3 vector3)
    {
        return new Vector3(vector3.X, vector3.Y, vector3.Z);
    }
}
