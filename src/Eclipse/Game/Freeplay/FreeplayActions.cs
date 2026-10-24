using AmongUs.GameOptions;
using System.Linq;
using Eclipse.Core;
using Eclipse.Networking;
using MiraAPI.Networking;
using Reactor.Utilities;

namespace Eclipse.Game;

internal static class FreeplayActions
{
    public static void SetRole(PlayerControl target, RoleTypes role)
    {
        if (TutorialManager.InstanceExists)
        {
            RoleChanger.ChangeRole(target, role);
            return;
        }

        Ask(FreeplayAction.SetRole, target.PlayerId, 0, (ushort)role);
    }

    public static void Attack(PlayerControl attacker, PlayerControl victim)
    {
        if (TutorialManager.InstanceExists)
        {
            attacker.RpcCustomMurder(victim, MeetingCheck.OutsideMeeting);
            return;
        }

        Ask(FreeplayAction.RemoteKill, attacker.PlayerId, victim.PlayerId, 0);
    }

    public static void Revive(PlayerControl target)
    {
        if (!TutorialManager.InstanceExists)
        {
            Ask(FreeplayAction.Revive, target.PlayerId, 0, 0);
        }
    }

    public static void Reset()
    {
        if (!TutorialManager.InstanceExists)
        {
            Ask(FreeplayAction.Reset, 0, 0, 0);
            return;
        }

        var snapshot = PlayerControl.AllPlayerControls.ToArray()
            .Where(p => p != null && p.Data?.Role != null)
            .Select(p => (Player: p, Role: p.Data.Role.Role))
            .ToArray();

        HudManager.Instance.ShowPopUp(MiraAPI.Translation.MiraLocaleManager.Get("Eclipse.Freeplay.ResetPopup"));
        ShipStatus.Instance.Begin();
        if (GameManager.Instance)
        {
            GameManager.Instance.ReviveEveryoneFreeplay();
        }

        foreach (var (player, role) in snapshot)
        {
            RoleChanger.ChangeRole(player, role);
        }

        Logger<EclipsePlugin>.Info($"Freeplay reset: {snapshot.Length} role(s) handed out again.");
    }

    public static void ToggleModifier(PlayerControl target, uint typeId)
    {
        if (TutorialManager.InstanceExists)
        {
            MultiplayerFreeplay.ToggleModifier(target, typeId);
            return;
        }

        Ask(FreeplayAction.ToggleModifier, target.PlayerId, 0, typeId);
    }

    private static void Ask(FreeplayAction action, byte target, byte other, uint data)
    {
        var local = PlayerControl.LocalPlayer;
        if (local != null)
        {
            FreeplayRpcs.RpcFreeplayRequest(local, (byte)action, target, other, data);
        }
    }
}
