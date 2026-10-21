using Eclipse.Buttons;
using Eclipse.Networking;
using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.Hud;

namespace Eclipse.Game;

internal static class MirrorcasterEvents
{
    // The mirror goes FIRST (priority -1000, before the Medic's shield at -900 and the protections at -800): it stops the attack, and everything that
    // runs after it finds it already cancelled and does nothing. A player with a shield AND a mirror keeps the shield and loses the mirror.
    [RegisterEvent(-1000)]
    public static void OnButtonClick(MiraButtonClickEvent click)
    {
        if (click.IsCancelled || click.Button is not IKillButton || click.Button is not CustomActionButton<PlayerControl> button)
        {
            return;
        }

        var target = button.Target;
        var local = PlayerControl.LocalPlayer;
        if (target == null || !button.CanClick() || Mirrors.Stopping(local, target, false) is not { } mirror)
        {
            return;
        }

        click.Cancel();
        button.SetTimer(ShieldGuard.BlockedCooldown);
        Report(local, mirror, target, false);
    }

    // Any other attack (a vanilla Impostor kill, a custom murder from any mod). Only the HOST reports it, so it is told once; the message also gives
    // the attacker back their normal state.
    [RegisterEvent(-1000)]
    public static void OnBeforeMurder(BeforeMurderEvent murder)
    {
        if (murder.IsCancelled || Mirrors.Stopping(murder.Source, murder.Target, murder.IgnoreDefense) is not { } mirror)
        {
            return;
        }

        murder.Cancel();
        if (AmongUsClient.Instance.AmHost)
        {
            Report(murder.Source, mirror, murder.Target, true);
        }
    }

    // A mirror cannot outlive the player who carries it, nor the Mirrorcaster who made it. Every game does this by itself.
    [RegisterEvent]
    public static void OnPlayerDeath(PlayerDeathEvent death)
    {
        Mirrors.Carried.Remove(death.Player.PlayerId);
        Mirrors.RemoveOf(death.Player.PlayerId);
    }

    [RegisterEvent]
    public static void OnPlayerLeave(PlayerLeaveEvent leave)
    {
        if (leave.ClientData.Character is { } leaver)
        {
            Mirrors.Carried.Remove(leaver.PlayerId);
            Mirrors.RemoveOf(leaver.PlayerId);
        }
    }

    [RegisterEvent]
    public static void OnStartMeeting(StartMeetingEvent meeting)
    {
        Mirrors.Reset();
    }

    [RegisterEvent(-1)]
    public static void OnRoundStart(RoundStartEvent roundStart)
    {
        if (roundStart.TriggeredByIntro)
        {
            Mirrors.Reset();
        }
    }

    private static void Report(PlayerControl source, Mirrors.Mirror mirror, PlayerControl target, bool wasAttack)
    {
        var mirrorcaster = GameData.Instance.GetPlayerById(mirror.Mirrorcaster)?.Object;
        if (mirrorcaster != null)
        {
            MirrorcasterRpcs.RpcMirrorHit(source, mirrorcaster, target, wasAttack);
        }
    }
}
