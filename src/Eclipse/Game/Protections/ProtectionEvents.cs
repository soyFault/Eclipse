using Eclipse.Buttons;
using Eclipse.Core;
using Eclipse.Networking;
using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.Hud;

namespace Eclipse.Game;

internal static class ProtectionEvents
{
    [RegisterEvent(-800)]
    public static void OnButtonClick(MiraButtonClickEvent click)
    {
        if (click.IsCancelled || click.Button is not CustomActionButton<PlayerControl> button)
        {
            return;
        }

        var target = button.Target;
        var local = PlayerControl.LocalPlayer;
        if (target == null || !button.CanClick() || Protections.Blocker(local, target, false, button is IKillButton) is not { } blocker)
        {
            return;
        }

        click.Cancel();

        button.SetTimer(ShieldGuard.BlockedCooldown);
        Announce(blocker, local, target, false);
    }

    [RegisterEvent(-800)]
    public static void OnBeforeMurder(BeforeMurderEvent murder)
    {
        if (murder.IsCancelled || Protections.Blocker(murder.Source, murder.Target, murder.IgnoreDefense, true) is not { } blocker)
        {
            return;
        }

        murder.Cancel();
        if (AmongUsClient.Instance.AmHost)
        {
            Announce(blocker, murder.Source, murder.Target, true);
        }
    }

    [RegisterEvent]
    public static void OnPlayerDeath(PlayerDeathEvent death)
    {
        Protections.Ledger.RemoveTarget(death.Player.PlayerId);
        Protections.Ledger.RemoveProtector(death.Player.PlayerId, ProtectionKind.Fortify);
        Protections.Ledger.RemoveProtector(death.Player.PlayerId, ProtectionKind.Guardian);
    }

    [RegisterEvent]
    public static void OnPlayerLeave(PlayerLeaveEvent leave)
    {
        if (leave.ClientData.Character is not { } leaver)
        {
            return;
        }

        Protections.Ledger.RemoveTarget(leaver.PlayerId);
        Protections.Ledger.RemoveProtector(leaver.PlayerId);
    }

    [RegisterEvent]
    public static void OnStartMeeting(StartMeetingEvent meeting)
    {
        Protections.Ledger.RemoveKind(ProtectionKind.Barrier);
        Protections.Ledger.RemoveKind(ProtectionKind.Vest);
        Protections.Ledger.RemoveKind(ProtectionKind.Guardian);
    }

    [RegisterEvent(-1)]
    public static void OnRoundStart(RoundStartEvent roundStart)
    {
        if (roundStart.TriggeredByIntro)
        {
            Protections.Reset();
        }
    }

    private static void Announce(Protection blocker, PlayerControl source, PlayerControl target, bool wasAttack)
    {
        var protector = GameData.Instance.GetPlayerById(blocker.ProtectorId)?.Object;
        if (protector != null)
        {
            WardenRpcs.RpcInteractionBlocked(source, (byte)blocker.Kind, protector, target, wasAttack);
        }
    }
}
