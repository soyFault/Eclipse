using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace Eclipse.Options.Host;

internal sealed class HostOptions : AbstractOptionGroup
{
    public override string GroupName => "Eclipse.Options.Host";

    public override uint GroupPriority => 0;

    [ModdedToggleOption("Eclipse.Option.Host.MultiplayerFreeplay")]
    public bool MultiplayerFreeplay { get; set; }

    [ModdedToggleOption("Eclipse.Option.Host.FreeplayHostOnly")]
    public bool FreeplayHostOnly { get; set; } = true;

    [ModdedToggleOption("Eclipse.Option.Host.FreeplayNoEnd")]
    public bool FreeplayNoEnd { get; set; } = true;
}
