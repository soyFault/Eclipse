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
    // Stops the click of ANY button that targets a protected player (not only attacks, unlike the Medic's shield), BEFORE the button does anything.
    // It runs after the Medic's shield check (priority -900), so a click the shield already stopped is not reported twice.
    [RegisterEvent(-800)]
    public static void OnButtonClick(MiraButtonClickEvent click)
    {
        if (click.IsCancelled || click.Button is not CustomActionButton<PlayerControl> button)
        {
            return;
        }

        var target = button.Target;
        var local = PlayerControl.LocalPlayer;
        if (target == null || !button.CanClick() || Protections.Blocker(local, target, false) is not { } blocker)
        {
            return;
        }

        click.Cancel();

        // The short cooldown also stops a player from getting through by clicking very fast.
        button.SetTimer(ShieldGuard.BlockedCooldown);
        Announce(blocker, local, target, false);
    }

    // Stops a murder (a vanilla Impostor kill, a custom murder from any mod) on a protected player. Only the HOST announces it, so it is reported once.
    [RegisterEvent(-800)]
    public static void OnBeforeMurder(BeforeMurderEvent murder)
    {
        if (murder.IsCancelled || Protections.Blocker(murder.Source, murder.Target, murder.IgnoreDefense) is not { } blocker)
        {
            return;
        }

        murder.Cancel();
        if (AmongUsClient.Instance.AmHost)
        {
            Announce(blocker, murder.Source, murder.Target, true);
        }
    }

    // A protection cannot outlive the player it protects, and a Warden's fortification cannot outlive the Warden. Every game does this by itself.
    [RegisterEvent]
    public static void OnPlayerDeath(PlayerDeathEvent death)
    {
        Protections.Ledger.RemoveTarget(death.Player.PlayerId);
        Protections.Ledger.RemoveProtector(death.Player.PlayerId, ProtectionKind.Fortify);
    }

    // A player that leaves takes with them every protection they gave or had.
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

    // A barrier does not last into a meeting. Every game ends them by itself (a fortification, on the other hand, does last).
    [RegisterEvent]
    public static void OnStartMeeting(StartMeetingEvent meeting)
    {
        Protections.Ledger.RemoveKind(ProtectionKind.Barrier);
    }

    // A new match starts with no protections.
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
