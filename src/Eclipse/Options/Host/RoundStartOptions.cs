using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace Eclipse.Options.Host;

internal sealed class RoundStartOptions : AbstractOptionGroup
{
    public override string GroupName => "Eclipse.Options.RoundStart";

    public override uint GroupPriority => 1;

    [ModdedToggleOption("Eclipse.Option.RoundStart.FirstDeathShield")]
    public bool FirstDeathShield { get; set; } = true;
}
