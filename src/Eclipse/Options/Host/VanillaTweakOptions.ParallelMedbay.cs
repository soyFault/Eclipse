using MiraAPI.GameOptions.Attributes;

namespace Eclipse.Options.Host;

internal sealed partial class VanillaTweakOptions
{
    [ModdedToggleOption("Eclipse.Option.Tweak.ParallelMedbay")]
    public bool ParallelMedbay { get; set; } = true;
}
