using MiraAPI.GameOptions.Attributes;

namespace Eclipse.Options.Host;

internal sealed partial class VanillaTweakOptions
{
    [ModdedToggleOption("Eclipse.Option.Tweak.HideVentAnimation")]
    public bool HideVentAnimationNotInVision { get; set; } = true;
}
