using FluentValidation;
using JetBrains.Annotations;

namespace OvertakerPlugin;

[UsedImplicitly]
public class OvertakerConfigurationValidator : AbstractValidator<OvertakerConfiguration>
{
    public OvertakerConfigurationValidator()
    {
        RuleFor(cfg => cfg.MinimumSpeedKmh).GreaterThanOrEqualTo(0);
    }
}
