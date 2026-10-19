using Eclipse.Roles.Crewmate.Investigative;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace Eclipse.Options.Roles.Crewmate;

internal sealed class AurialOptions : AbstractRoleOptionGroup<AurialRole>
{
    public override string GroupName => "Eclipse.Role.Aurial";

    [ModdedNumberOption("Eclipse.Option.Aurial.AuraInnerRadius", 0f, 1f, 0.25f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float AuraInnerRadius { get; set; } = 0.5f;

    [ModdedNumberOption("Eclipse.Option.Aurial.AuraOuterRadius", 1f, 5f, 0.25f, MiraNumberSuffixes.Multiplier, "0.00")]
    public float AuraOuterRadius { get; set; } = 1.5f;

    [ModdedNumberOption("Eclipse.Option.Aurial.SenseDuration", 1f, 15f, 1f, MiraNumberSuffixes.Seconds, "0")]
    public float SenseDuration { get; set; } = 10f;
}
