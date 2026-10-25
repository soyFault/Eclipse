using System.Linq;
using Eclipse.Assets;
using Eclipse.Colors;
using Eclipse.Core;
using Eclipse.Game;
using Eclipse.Networking;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace Eclipse.Buttons.Neutral;

internal sealed class JesterHauntButton : EclipseActionButton
{
    public override string Name => "Eclipse.Button.JesterHaunt";

    public override float Cooldown => 0.001f;

    public override float InitialCooldown => 0.001f;

    public override int MaxUses => 0;

    public override LoadableAsset<Sprite> Sprite => EclipseAssets.ButtonIcon("JesterHaunt", MiraAssets.CrewmateFile);

    public override ButtonLocation Location => ButtonLocation.BottomRight;

    public override BaseKeybind? Keybind => MiraGlobalKeybinds.PrimaryAbility;

    public override Color TextOutlineColor => EclipseColors.Jester;

    public override bool Enabled(RoleBehaviour? role)
    {
        var local = PlayerControl.LocalPlayer;
        return local != null && local.Data != null && local.Data.IsDead && JesterBook.IsHauntOpen(local.PlayerId);
    }

    public override bool CanUse()
    {
        var local = PlayerControl.LocalPlayer;
        if (local == null || !base.CanUse())
        {
            return false;
        }

        return JesterRules.CanHaunt(JesterBook.IsHauntOpen(local.PlayerId), JesterBook.HasHaunted(local.PlayerId), MeetingHud.Instance != null)
               && JesterBook.AliveVoterCount(local.PlayerId) > 0;
    }

    protected override void OnClick()
    {
        var local = PlayerControl.LocalPlayer;
        var voters = JesterBook.AliveVoters(local.PlayerId);
        var menu = CustomPlayerMenu.Create(EclipseColors.Jester);
        menu.Begin(
            player => voters.Contains(player.PlayerId) && player.Data != null && !player.Data.IsDead && !player.Data.Disconnected && !player.AmOwner,
            chosen =>
            {
                menu.ForceClose();
                if (chosen == null)
                {
                    return;
                }

                JesterRpcs.RpcJesterHaunt(local, chosen);
                local.RpcAdvancedCustomMurder(chosen, MeetingCheck.OutsideMeeting, isIndirect: true, ignoreDefense: true, resetKillTimer: false, teleportMurderer: false);
            });
    }
}
