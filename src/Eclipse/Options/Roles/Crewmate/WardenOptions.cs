using Eclipse.Core;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace Eclipse.Options.Roles.Crewmate;

internal sealed class WardenOptions : AbstractRoleOptionGroup<WardenRole>
{
    public override string GroupName => "Eclipse.Role.Warden";

    [ModdedEnumOption(
        "Eclipse.Option.Warden.ShowFortified",
        typeof(FortifyView),
        ["Eclipse.Option.Warden.ShowFortified.Self", "Eclipse.Option.Warden.ShowFortified.Warden", "Eclipse.Option.Warden.ShowFortified.SelfAndWarden", "Eclipse.Option.Warden.ShowFortified.Everyone"])]
    public FortifyView ShowFortified { get; set; } = FortifyView.SelfAndWarden;
}
