using BepInEx.Configuration;
using Eclipse.Core;
using MiraAPI.LocalSettings;
using MiraAPI.LocalSettings.Attributes;

namespace Eclipse;

internal sealed class EclipseLocalTabPlayers(ConfigFile config) : LocalSettingsTab(config)
{
    private const string Section = "Eclipse.Local.Section.Visuals";

    public override string TabName => "Eclipse.Local.Players";

    protected override bool ShouldCreateLabels => true;

    [LocalEnumSetting(names: ["Eclipse.Local.NameStyleTop", "Eclipse.Local.NameStyleTopSmall", "Eclipse.Local.NameStyleBottom", "Eclipse.Local.NameStyleBottomSmall"])]
    public ConfigEntry<RoleNameStyle> RoleNameStyle { get; private set; } =
        config.Bind(Section, "Eclipse.Local.RoleNameStyle", Core.RoleNameStyle.TopSmall);

    [LocalToggleSetting]
    public ConfigEntry<bool> ShowRoleIcons { get; private set; } =
        config.Bind(Section, "Eclipse.Local.ShowRoleIcons", true);

    [LocalToggleSetting]
    public ConfigEntry<bool> ColorPlayerNames { get; private set; } =
        config.Bind(Section, "Eclipse.Local.ColorPlayerNames", false);
}
