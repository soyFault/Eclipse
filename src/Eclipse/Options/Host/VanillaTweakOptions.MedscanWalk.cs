using MiraAPI.GameOptions.Attributes;

namespace Eclipse.Options.Host;

internal sealed partial class VanillaTweakOptions
{
    [ModdedToggleOption("Eclipse.Option.Tweak.MedscanWalk")]
    public bool MedscanWalk { get; set; } = true;
}
