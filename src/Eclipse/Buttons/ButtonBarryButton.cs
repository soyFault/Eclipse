using Eclipse.Assets;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Modifiers;
using Eclipse.Networking;
using Eclipse.Options.Host;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons;

internal sealed class ButtonBarryButton : EclipseActionButton
{
    public override string Name => "Eclipse.Button.ButtonBarry";

    public override float Cooldown => Mathf.Clamp(OptionGroupSingleton<ButtonBarryOptions>.Instance.Cooldown, 2.5f, 120f);

    public override float InitialCooldown => 10f;

    public override int MaxUses => (int)OptionGroupSingleton<ButtonBarryOptions>.Instance.MaxUses;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.ButtonIcon("ButtonBarry", MiraAssets.CustomTeamFile);

    public override ButtonLocation Location => ButtonLocation.BottomLeft;

    public override BaseKeybind? Keybind => MiraGlobalKeybinds.SecondaryAbility;

    protected override bool IsRoundLocked => MatchRounds.IsFirstRoundLocked(OptionGroupSingleton<ButtonBarryOptions>.Instance.FirstRound);

    public override bool Enabled(RoleBehaviour? role)
    {
        var local = PlayerControl.LocalPlayer;
        return local != null && local.Data != null && !local.Data.IsDead && local.HasModifier<ButtonBarryModifier>() && local.RemainingEmergencies > 0;
    }

    public override bool CanUse()
    {
        if (!base.CanUse() || IsRoundLocked)
        {
            return false;
        }

        var sabotage = ShipStatus.Instance != null ? ShipStatus.Instance.Systems[SystemTypes.Sabotage].TryCast<SabotageSystemType>() : null;
        return UniversalRules.CanCallMeeting(
            PlayerControl.LocalPlayer.RemainingEmergencies,
            sabotage != null && sabotage.AnyActive,
            OptionGroupSingleton<ButtonBarryOptions>.Instance.IgnoreSabotage,
            PlayerControl.LocalPlayer.Data.IsDead);
    }

    protected override void OnClick()
    {
        ButtonBarryRpcs.RpcCallMeeting(PlayerControl.LocalPlayer);
    }
}
