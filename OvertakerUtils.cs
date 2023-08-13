using System.Numerics;

namespace OvertakerPlugin;

public static class OvertakerUtils
{
    /// <summary>
    /// Given a vector representing the velocity of a car, return the speed in km/h.
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
}
