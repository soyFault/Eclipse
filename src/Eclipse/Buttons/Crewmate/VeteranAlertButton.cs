using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Modifiers;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Killing;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Translation;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.Crewmate;

internal sealed class VeteranAlertButton : EclipseActionButton
{
    public override string Name => "Eclipse.Button.VeteranAlert";

    public override float Cooldown => Mathf.Clamp(OptionGroupSingleton<VeteranOptions>.Instance.AlertCooldown, 5f, 120f);

    public override float InitialCooldown => 10f;

    public override float EffectDuration => OptionGroupSingleton<VeteranOptions>.Instance.AlertDuration;

    public override int MaxUses => (int)OptionGroupSingleton<VeteranOptions>.Instance.MaxAlerts;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.ButtonIcon("VeteranAlert", MiraAssets.CrewmateFile);

    public override ButtonLocation Location => ButtonLocation.BottomRight;

    public override BaseKeybind? Keybind => MiraGlobalKeybinds.SecondaryAbility;

    public override Color TextOutlineColor => EclipseColors.Veteran;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is VeteranRole;
    }

    protected override void OnClick()
    {
        PlayerControl.LocalPlayer.RpcAddModifier<VeteranAlertModifier>();
        OverrideName(MiraLocaleManager.Get("Eclipse.Button.VeteranAlerting"));
    }

    public override void OnEffectEnd()
    {
        OverrideName(MiraLocaleManager.Get("Eclipse.Button.VeteranAlert"));
    }
}
