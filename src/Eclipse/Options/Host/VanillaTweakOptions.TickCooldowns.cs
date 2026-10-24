using MiraAPI.GameOptions.Attributes;

namespace Eclipse.Options.Host;

internal sealed partial class VanillaTweakOptions
{
    [ModdedToggleOption("Eclipse.Option.Tweak.TickCooldownsInMinigame")]
    public bool TickCooldownsInMinigame { get; set; } = true;
}
