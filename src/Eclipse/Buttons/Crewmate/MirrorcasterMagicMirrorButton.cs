using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Networking;
using Eclipse.Options.Roles.Crewmate;
using Eclipse.Roles.Crewmate.Protective;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Translation;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.Crewmate;

internal sealed class MirrorcasterMagicMirrorButton : EclipseActionButton
{
    // What the mirror did, read when its effect ends.
    private bool _targetWasValid;
    private bool _hit;
    private byte _protectedId = byte.MaxValue;

    public override string Name => "Eclipse.Button.MirrorcasterMagicMirror";

    public override float Cooldown => Mathf.Clamp(OptionGroupSingleton<MirrorcasterOptions>.Instance.MirrorCooldown, 0.001f, 120f);

    public override float InitialCooldown => 10f;

    public override float EffectDuration => OptionGroupSingleton<MirrorcasterOptions>.Instance.MirrorDuration;

    public override int MaxUses => (int)OptionGroupSingleton<MirrorcasterOptions>.Instance.MaxMirrors;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.ButtonIcon("MirrorcasterMagicMirror", MiraAssets.CrewmateFile);

    public override ButtonLocation Location => ButtonLocation.BottomRight;

    public override BaseKeybind? Keybind => MiraGlobalKeybinds.SecondaryAbility;

    public override Color TextOutlineColor => EclipseColors.Mirrorcaster;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is MirrorcasterRole;
    }

    public override bool CanUse()
    {
        var local = PlayerControl.LocalPlayer;
        if (local == null || local.Data.IsDead || local.Data.Role is not MirrorcasterRole role || MeetingHud.Instance != null || !base.CanUse())
        {
            return false;
        }

        return MirrorcasterRules.CanPlace(
            Mirrors.HasMirrorOf(local.PlayerId),
            role.UnleashesAvailable,
            OptionGroupSingleton<MirrorcasterOptions>.Instance.MultiUnleash,
            EffectActive,
            Timer);
    }

    // The button does not use the effect of Mira API when it is clicked: it opens a menu, and only once somebody is chosen the effect is started by
    // hand (see OnClick). So a click with the menu closed without choosing costs nothing.
    public override void ClickHandler()
    {
        if (CanUse())
        {
            OnClick();
        }
    }

    protected override void OnClick()
    {
        var menu = CustomPlayerMenu.Create(EclipseColors.Mirrorcaster);
        menu.Begin(
            player => player.Data != null && !player.Data.IsDead && !player.Data.Disconnected && !player.AmOwner,
            chosen =>
            {
                menu.ForceClose();
                if (chosen == null)
                {
                    return;
                }

                MirrorcasterRpcs.RpcMirrorPlace(PlayerControl.LocalPlayer, chosen);
                EffectActive = true;
                Timer = EffectDuration;
                _targetWasValid = true;
                _hit = false;
                _protectedId = chosen.PlayerId;
                OverrideName(MiraLocaleManager.Get("Eclipse.Button.MirrorcasterMagicMirrorProtecting"));
            });
    }

    // An attack broke the mirror: the effect ends now, and its use is spent.
    public void BreakByAttack()
    {
        _hit = true;
        ResetCooldownAndOrEffect();
    }

    public override void OnEffectEnd()
    {
        OverrideName(MiraLocaleManager.Get("Eclipse.Button.MirrorcasterMagicMirror"));
        var local = PlayerControl.LocalPlayer;
        var wasValid = _targetWasValid;
        var wasHit = _hit;
        var protectedId = _protectedId;
        _targetWasValid = false;
        _hit = false;
        _protectedId = byte.MaxValue;
        if (local == null || local.Data.Role is not MirrorcasterRole)
        {
            return;
        }

        if (wasValid)
        {
            DecreaseUses();
        }

        // If no attack broke the mirror, say whether the protected player died anyway.
        if (!wasHit && GameData.Instance.GetPlayerById(protectedId) is { } info && !MeetingHud.Instance)
        {
            var key = info.IsDead ? "Eclipse.Role.Mirrorcaster.TargetDiedNotif" : "Eclipse.Role.Mirrorcaster.TargetDidNotDieNotif";
            Helpers.CreateAndShowNotification(
                $"<b>{MiraLocaleManager.Get(key).Replace("<player>", info.PlayerName)}</b>",
                Color.white,
                new Vector3(0f, 1f, -20f),
                spr: EclipseAssets.RoleIcon("Mirrorcaster")?.LoadAsset());
        }

        MirrorcasterRpcs.RpcMirrorClear(local);
    }
}
