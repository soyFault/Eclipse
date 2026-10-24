using BepInEx.Configuration;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Assets;
using MiraAPI.LocalSettings;
using MiraAPI.LocalSettings.Attributes;
using MiraAPI.Utilities;

namespace Eclipse;

internal sealed class EclipseLocalTabPreferences(ConfigFile config) : LocalSettingsTab(config)
{
    private const string EndGame = "Eclipse.Local.Section.EndGame";

    private const string Summary = "Eclipse.Local.Section.Summary";
    public override string TabName => "Eclipse.Local.Preferences";

    protected override bool ShouldCreateLabels => true;

    public override LocalSettingTabAppearance TabAppearance => new()
    {
        TabIcon = EclipseAssets.LocalTabIcon("Preferences"),
        HideIconOnHover = false,
    };

    [LocalEnumSetting(names: ["Eclipse.Local.RejoinNever", "Eclipse.Local.RejoinAlways", "Eclipse.Local.RejoinHost", "Eclipse.Local.RejoinClient"])]
    public ConfigEntry<AutoRejoinMode> AutoRejoin { get; private set; } =
        config.Bind(EndGame, "Eclipse.Local.AutoRejoin", AutoRejoinMode.Always);

    [LocalSliderSetting(min: 4f, max: 15f, suffixType: MiraNumberSuffixes.Seconds, formatString: "0", displayValue: true, roundValue: true)]
    public ConfigEntry<float> AutoRejoinDelay { get; private set; } =
        config.Bind(EndGame, "Eclipse.Local.AutoRejoinDelay", 6f);

    [LocalEnumSetting(names: ["Eclipse.Local.SummaryHidden", "Eclipse.Local.SummaryLeft"])]
    public ConfigEntry<EndSummaryMode> EndSummary { get; private set; } =
        config.Bind(Summary, "Eclipse.Local.EndSummary", EndSummaryMode.Left);
}
