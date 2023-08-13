using AssettoServer.Server.Configuration;
using JetBrains.Annotations;

namespace OvertakerPlugin;

[UsedImplicitly(ImplicitUseKindFlags.Assign, ImplicitUseTargetFlags.WithMembers)]
public class OvertakerConfiguration : IValidateConfiguration<OvertakerConfigurationValidator>
{
    public int MinimumSpeedKmh { get; init; } = 85;
    public int MinimumDistanceMeters { get; init; } = 7;
}
