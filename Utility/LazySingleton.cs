using JetBrains.Annotations;

namespace OvertakerPlugin.Utility;

/// <summary>
///     Represents a lazily instantiated singleton. This is useful for classes that need to be instantiated
///     once and only once, but not necessarily at startup.
/// </summary>
/// <example>
///     <code>
/// public class MySingleton : LazySingleton&lt;MySingleton&gt;
/// {
///   // ...
/// }
/// </code>
/// </example>
/// <typeparam name="T">The type of the singleton. This must be a class</typeparam>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers | ImplicitUseTargetFlags.WithInheritors)]
public abstract class LazySingleton<T> where T : class
{
    private static readonly Lazy<T> Lazy = new(() =>
        Activator.CreateInstance(typeof(T), true) as T ??
        throw new InvalidOperationException($"{typeof(T)} could not be lazily instantiated"));

    public static T Instance => Lazy.Value;
}
